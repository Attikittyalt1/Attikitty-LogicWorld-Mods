using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;


namespace MorePegs.LogicCode.LinkableHandling;

public interface ILinkable
{
    public void Link(ILinkable linkable);
    public void Unlink(ILinkable linkable);
}