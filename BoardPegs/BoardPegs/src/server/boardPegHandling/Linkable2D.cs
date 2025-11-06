using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;
using UnityEngine;


namespace BoardPegs.Logic.BoardPegHandling;

public class Linkable2D : IEquatable<Linkable2D>, IComparable<Linkable2D>
{
    public required Func<Vector2Int> GetLinkingPosition { get; init; }
    public required Func<bool> HasBeenMoved { get; init; }
    public required Func<bool> ShouldBeLinkedHorizontally { get; init; }
    public required Func<bool> ShouldBeLinkedVertically { get; init; }
    public required IInputPeg LinkablePeg { get; init; }
    public required ComponentAddress Address { get; init; }

    public Linkable ToHorizontalLinkable() => new Linkable
    {
        GetLinkingPosition = () => GetLinkingPosition().x,
        HasBeenMoved = HasBeenMoved,
        LinkablePeg = LinkablePeg,
        Address = Address
    };

    public Linkable ToVerticalLinkable() => new Linkable
    {
        GetLinkingPosition = () => GetLinkingPosition().y,
        HasBeenMoved = HasBeenMoved,
        LinkablePeg = LinkablePeg,
        Address = Address
    };

    public bool Equals(Linkable2D linkable)
    {
        return Address.Equals(linkable.Address);
    }

    public int CompareTo(Linkable2D linkable)
    {
        return Address.CompareTo(linkable.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}