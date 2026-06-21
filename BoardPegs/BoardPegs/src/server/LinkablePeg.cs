using BoardPegs.LogicCode.LinkableHandling;
using LICC;
using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;


namespace BoardPegs.LogicCode;

public class LinkablePeg : ILinkable<LinkablePeg>
{
    private readonly IInputPeg _inputPeg;

    public LinkablePeg(IInputPeg peg)
    {
        _inputPeg = peg;
    }

    public void Link(LinkablePeg linkable)
    {
        _inputPeg.AddSecretLinkWith(linkable._inputPeg);
    }

    public void Unlink(LinkablePeg linkable)
    {
        _inputPeg.RemoveSecretLinkWith(linkable._inputPeg);
    }
}