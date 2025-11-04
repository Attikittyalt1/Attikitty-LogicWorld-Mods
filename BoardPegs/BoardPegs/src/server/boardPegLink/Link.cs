using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;


namespace BoardPegs.Logic.BoardPegLink;

public class Link : IEquatable<Link>, IComparable<Link>
{
    public required Func<int> GetLinkingPosition { get; init; }
    public required Action<IInputPeg> LinkPeg { get; init; }
    public required Action<IInputPeg> UnlinkPeg { get; init; }
    public required ComponentAddress Address { get; init; }

    public bool Equals(Link link)
    {
        return Address.Equals(link.Address);
    }

    public int CompareTo(Link link)
    {
        return Address.CompareTo(link.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}