using BoardPegs.LogicCode.LinkableHandling;
using System.Collections.Generic;
using UnityEngine;

namespace BoardPegs.LogicCode;

public class BoardPegTop : BoardPeg
{
    protected override List<PackageManager2D<LinkablePeg>> FindManagers() => (Component.LocalPositionFixed.y - 75) switch
    {
        > 0 => [ManagerAboveBoard],
        < 0 => [ManagerBelowBoard],
        _ => []
    };

    protected override (bool x, bool y) GetAxisStatus() => (
        Mathf.Abs(Component.localUp.y) >= Epsilon,
        Mathf.Abs(Component.localUp.y) >= Epsilon
    );
}
