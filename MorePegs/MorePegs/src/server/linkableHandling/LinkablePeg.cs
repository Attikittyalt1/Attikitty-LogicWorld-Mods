using LogicAPI.Server.Components;
using System;


namespace MorePegs.LogicCode.LinkableHandling;

public class LinkablePeg : ILinkable
{
    public readonly IInputPeg InputPeg;
    private bool _trySwap = false;

    public LinkablePeg(IInputPeg peg)
    {
        InputPeg = peg;
    }

    public void Link(ILinkable linkable)
    {
        switch (linkable)
        {
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

    private void Link(LinkablePeg linkable)
    {
        InputPeg.AddSecretLinkWith(linkable.InputPeg);
    }

    private void Unlink(LinkablePeg linkable)
    {
        InputPeg.RemoveSecretLinkWith(linkable.InputPeg);
    }
}