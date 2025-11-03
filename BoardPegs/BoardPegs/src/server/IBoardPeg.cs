using LogicAPI.Server.Components;
using UnityEngine;
using System;


namespace BoardPegs.Logic;

public interface IBoardPeg<T>
    where T : IComparable<T>, IEquatable<T>
{
    public T AssignedTrackerKey { get; set; }
    public bool IsTracked { get; set; }
    public T GenerateTrackerAddress();
    public Vector2Int GetLinkingPosition();
    public bool ShouldBeLinkedHorizontally();
    public bool ShouldBeLinkedVertically();
    public void LinkPeg(IInputPeg peg);
    public void UnlinkPeg(IInputPeg peg);
}
