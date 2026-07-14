using LICC;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using MorePegs.LogicCode.LinkableHandling;
using System;
using System.Collections;
using System.Collections.Generic;


namespace MorePegs.LogicCode;

public class LinkableMultiPeg : ILinkable
{
    public readonly IReadOnlyList<IInputPeg> InputPegs;
    private bool _trySwap = false;

    public LinkableMultiPeg(IReadOnlyList<IInputPeg> pegs)
    {
        InputPegs = pegs;
    }

    public void Link(ILinkable linkable)
    {
        switch (linkable)
        {
            case LinkableMultiPeg linkableMultiPeg:
                Link(linkableMultiPeg);
                break;
            case LinkablePeg linkablePeg:
                Link(linkablePeg);
                break;
            default:
                if (_trySwap)
                {
                    throw new ArgumentException("Cannot link with linkable of type: " + linkable.GetType());
                }

                _trySwap = true;
                linkable.Link(this);
                _trySwap = false;
                break;
        }
    }

    public void Unlink(ILinkable linkable)
    {
        switch (linkable)
        {
            case LinkableMultiPeg linkableMultiPeg:
                Unlink(linkableMultiPeg);
                break;
            case LinkablePeg linkablePeg:
                Unlink(linkablePeg);
                break;
            default:
                if (_trySwap)
                {
                    throw new ArgumentException("Cannot unlink with linkable of type: " + linkable.GetType());
                }

                _trySwap = true;
                linkable.Unlink(this);
                _trySwap = false;
                break;
        }
    }

    private void Link(LinkableMultiPeg linkable)
    {
        LConsole.WriteLine(String.Format("linking with count: {0}, {1}", InputPegs.Count, linkable.InputPegs.Count));

        for (int i = 0; i < Math.Min(InputPegs.Count, linkable.InputPegs.Count); i++)
        {
            InputPegs[i].AddSecretLinkWith(linkable.InputPegs[i]);
        }
    }

    private void Unlink(LinkableMultiPeg linkable)
    {
        LConsole.WriteLine(String.Format("unlinking with count: {0}, {1}", InputPegs.Count, linkable.InputPegs.Count));

        for (int i = 0; i < Math.Min(InputPegs.Count, linkable.InputPegs.Count); i++)
        {
            InputPegs[i].RemoveSecretLinkWith(linkable.InputPegs[i]);
        }
    }

    private void Link(LinkablePeg linkable)
    {
        InputPegs[0].AddSecretLinkWith(linkable.InputPeg);
    }

    private void Unlink(LinkablePeg linkable)
    {
        InputPegs[0].RemoveSecretLinkWith(linkable.InputPeg);
    }
}