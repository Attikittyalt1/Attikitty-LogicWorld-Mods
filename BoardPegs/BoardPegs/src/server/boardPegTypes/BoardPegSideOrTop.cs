using BoardPegs.Logic.BoardPegHandling;
using System.Collections.Generic;
using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegSideOrTop : BoardPeg
{
    protected override List<PackageManager2D> FindManagers() => (Component.LocalPositionFixed.y - 75) switch
    {
        > 0 => [ManagerAboveBoard],
        < 0 => [ManagerBelowBoard],
        _ => [ManagerAtBoardHeight, ManagerAboveBoard, ManagerBelowBoard]
    };

    protected override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.z) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon;
    }

    protected override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.x) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon;
    }
}
