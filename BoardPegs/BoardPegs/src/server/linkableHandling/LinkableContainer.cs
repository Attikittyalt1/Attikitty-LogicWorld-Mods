using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;


namespace BoardPegs.LogicCode.LinkableHandling;

public class LinkableContainer<T> : IEquatable<LinkableContainer<T>>, IComparable<LinkableContainer<T>>
    where T : ILinkable<T>
{
    public required Func<int> GetLinkingPosition { get; init; }
    public required Func<bool> HasBeenMoved { get; init; }
    public required T Linkable { get; init; }
    public required ComponentAddress Address { get; init; }

    public bool Equals(LinkableContainer<T> container)
    {
        return Address.Equals(container.Address);
    }

    public int CompareTo(LinkableContainer<T> container)
    {
        return Address.CompareTo(container.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}