using LogicAPI.Data;
using LogicAPI.Server.Components;
using System;


namespace BoardPegs.Logic.BoardPegHandling;

public class Linkable : IEquatable<Linkable>, IComparable<Linkable>
{
    public required Func<int> GetLinkingPosition { get; init; }
    public required IInputPeg LinkablePeg { get; init; }
    public required ComponentAddress Address { get; init; }

    public bool Equals(Linkable linkable)
    {
        return Address.Equals(linkable.Address);
    }

    public int CompareTo(Linkable linkable)
    {
        return Address.CompareTo(linkable.Address);
    }

    public override int GetHashCode()
    {
        return Address.GetHashCode();
    }
}