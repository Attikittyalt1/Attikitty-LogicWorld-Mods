using LogicAPI.Data;
using LogicAPI.Server.Components;
using UnityEngine;


namespace BoardPegs.Logic;

public interface IBoardPeg
{
    // it feels slightly wrong to have the assigned address stored here instead of in the BoardPegTracker, but I can't think of a better way
    public ComponentAddress? AssignedTrackerAddress { get; set; }
    public ComponentAddress GenerateTrackerAddress();
    public Vector2Int GetLinkingPosition();
    public bool ShouldBeLinkedHorizontally();
    public bool ShouldBeLinkedVertically();
    public void LinkPeg(IInputPeg peg);
    public void UnlinkPeg(IInputPeg peg);
}
