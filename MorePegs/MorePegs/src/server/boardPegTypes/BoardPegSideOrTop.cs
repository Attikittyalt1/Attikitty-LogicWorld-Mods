using MorePegs.LogicCode.LinkableHandling;
using System.Collections.Generic;
using UnityEngine;

namespace MorePegs.LogicCode;

public class BoardPegSideOrTop : BoardPeg
{
    protected override List<PackageManager2D> FindManagers() => (Component.LocalPositionFixed.y - 75) switch
    {
        > 0 => [ManagerAboveBoard],
        < 0 => [ManagerBelowBoard],
        _ => [ManagerAtBoardHeight, ManagerAboveBoard, ManagerBelowBoard]
    };

    protected override (bool x, bool y) GetAxisStatus() => (
        Mathf.Abs(Component.localUp.z) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon,
        Mathf.Abs(Component.localUp.x) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon
    );
}
