using LogicAPI.Server.Components;
using System;
using UnityEngine;


namespace BoardPegs.Logic;

public interface IBoardPegTrackable<T>
    where T : IComparable<T>, IEquatable<T>
{
    public T AssignedTrackerKey { get; set; }
    public bool IsTracked { get; set; }
    public T GenerateTrackerKey();
    public Vector2Int GetLinkingPosition();
    public bool ShouldBeLinkedHorizontally();
    public bool ShouldBeLinkedVertically();
    public void LinkPeg(IInputPeg peg);
    public void UnlinkPeg(IInputPeg peg);
}
