using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;
using UnityEngine;


namespace MorePegs.LogicCode.LinkableHandling;

public class LinkableContainer2D : IEquatable<LinkableContainer2D>, IComparable<LinkableContainer2D>
{
    public required Func<Vector2Int> GetLinkingPosition { get; init; }
    public required Func<(bool x, bool y)> GetAxisStatus { get; init; }
    public required Func<bool> HasBeenMoved { get; init; }
    public required ILinkable Linkable { get; init; }
    public required ComponentAddress Address { get; init; }

    public (LinkableContainer x, LinkableContainer y) To1DContainers() => (new LinkableContainer
    {
        GetLinkingPosition = () => GetLinkingPosition().x,
        HasBeenMoved = HasBeenMoved,
        Linkable = Linkable,
        Address = Address,
    }, new LinkableContainer
    {
        GetLinkingPosition = () => GetLinkingPosition().y,
        HasBeenMoved = HasBeenMoved,
        Linkable = Linkable,
        Address = Address,
    });

    public bool Equals(LinkableContainer2D container)
    {
        return Address.Equals(container.Address);
    }

    public int CompareTo(LinkableContainer2D container)
    {
        return Address.CompareTo(container.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}