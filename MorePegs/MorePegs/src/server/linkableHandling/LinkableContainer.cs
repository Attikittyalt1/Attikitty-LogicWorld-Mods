using LogicAPI.Data;
using System;


namespace MorePegs.LogicCode.LinkableHandling;

public class LinkableContainer : IEquatable<LinkableContainer>, IComparable<LinkableContainer>
{
    public required ILinkable Linkable { get; init; }
    public required ComponentAddress Address { get; init; }
    public required int Position { get; init; }

    public bool Equals(LinkableContainer container)
    {
        return Address.Equals(container.Address);
    }

    public int CompareTo(LinkableContainer container)
    {
        return Address.CompareTo(container.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}