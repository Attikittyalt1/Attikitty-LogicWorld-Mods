using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;
using UnityEngine;


namespace BoardPegs.LogicCode.LinkableHandling;

public class LinkableContainer2D<T> : IEquatable<LinkableContainer2D<T>>, IComparable<LinkableContainer2D<T>>
    where T : ILinkable<T>
{
    public required Func<Vector2Int> GetLinkingPosition { get; init; }
    public required Func<(bool x, bool y)> GetAxisStatus { get; init; }
    public required Func<bool> HasBeenMoved { get; init; }
    public required T Linkable { get; init; }
    public required ComponentAddress Address { get; init; }

    public (LinkableContainer<T> x, LinkableContainer<T> y) To1DContainers() => (new LinkableContainer<T>
    {
        GetLinkingPosition = () => GetLinkingPosition().x,
        HasBeenMoved = HasBeenMoved,
        Linkable = Linkable,
        Address = Address,
    }, new LinkableContainer<T>
    {
        GetLinkingPosition = () => GetLinkingPosition().y,
        HasBeenMoved = HasBeenMoved,
        Linkable = Linkable,
        Address = Address,
    });

    public bool Equals(LinkableContainer2D<T> container)
    {
        return Address.Equals(container.Address);
    }

    public int CompareTo(LinkableContainer2D<T> container)
    {
        return Address.CompareTo(container.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}