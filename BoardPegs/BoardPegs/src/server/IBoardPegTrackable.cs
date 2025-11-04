using LogicAPI.Server.Components;
using System;
using UnityEngine;


namespace BoardPegs.Logic;

public interface IBoardPegTrackable
{
    public Vector2Int GetLinkingPosition();
    public bool ShouldBeLinkedHorizontally();
    public bool ShouldBeLinkedVertically();
    public void LinkPeg(IInputPeg peg);
    public void UnlinkPeg(IInputPeg peg);
}
