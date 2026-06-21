using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;


namespace BoardPegs.LogicCode.LinkableHandling;

public interface ILinkable<T>
    where T : ILinkable<T>
{
    public void Link(T linkable);
    public void Unlink(T linkable);
}