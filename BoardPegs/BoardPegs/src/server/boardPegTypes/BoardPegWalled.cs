using BoardPegs.LogicCode.LinkableHandling;
using LICC;
using System.Collections.Generic;
using UnityEngine;

namespace BoardPegs.LogicCode;

public class BoardPegWalled : BoardPeg
{
    protected override List<PackageManager2D<LinkablePeg>> FindManagers() => (Component.LocalPositionFixed.y - 75) switch
    {
        > 0 => [ManagerAboveBoard],
        < 0 => [ManagerBelowBoard],
        _ => Mathf.Abs(Component.localForward.y) >= Epsilon ? [ManagerAtBoardHeight, ManagerAboveBoard, ManagerBelowBoard] : [ManagerAtBoardHeight],
    };

    protected override (bool x, bool y) GetAxisStatus() => (
        Mathf.Abs(Component.localForward.z) >= Epsilon && Mathf.Abs(Component.localUp.y) >= Epsilon || Mathf.Abs(Component.localUp.z) >= Epsilon,
        Mathf.Abs(Component.localForward.x) >= Epsilon && Mathf.Abs(Component.localUp.y) >= Epsilon || Mathf.Abs(Component.localUp.x) >= Epsilon
    );
}
