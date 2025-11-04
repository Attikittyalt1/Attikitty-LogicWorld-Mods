using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;
using UnityEngine;


namespace BoardPegs.Logic.BoardPegLink;

public class LinkWrapper2D : IEquatable<LinkWrapper2D>, IComparable<LinkWrapper2D>
{
    public required Func<Vector2Int> GetLinkingPosition { get; init; }
    public required Func<bool> ShouldBeLinkedHorizontally { get; init; }
    public required Func<bool> ShouldBeLinkedVertically { get; init; }
    public required Action<IInputPeg> LinkPeg { get; init; }
    public required Action<IInputPeg> UnlinkPeg { get; init; }
    public required ComponentAddress Address { get; init; }

    public Link ToHorizontalLink() => new Link
    {
        GetLinkingPosition = () => GetLinkingPosition().x,
        LinkPeg = LinkPeg,
        UnlinkPeg = UnlinkPeg,
        Address = Address
    };

    public Link ToVerticalLink() => new Link
    {
        GetLinkingPosition = () => GetLinkingPosition().y,
        LinkPeg = LinkPeg,
        UnlinkPeg = UnlinkPeg,
        Address = Address
    };

    public bool Equals(LinkWrapper2D link)
    {
        return Address.Equals(link.Address);
    }

    public int CompareTo(LinkWrapper2D link)
    {
        return Address.CompareTo(link.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}