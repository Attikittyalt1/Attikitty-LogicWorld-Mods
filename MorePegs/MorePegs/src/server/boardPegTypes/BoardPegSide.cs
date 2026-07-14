using MorePegs.LogicCode.LinkableHandling;
using System.Collections.Generic;
using UnityEngine;

namespace MorePegs.LogicCode;

public class BoardPegSide : BoardPeg
{
    protected override List<PackageManager2D> FindManagers() => (Component.LocalPositionFixed.y - 75) switch
    {
        > 0 => [],
        < 0 => [],
        _ => [ManagerAtBoardHeight]
    };

    protected override (bool x, bool y) GetAxisStatus() => (
        Mathf.Abs(Component.localUp.z) >= Epsilon,
        Mathf.Abs(Component.localUp.x) >= Epsilon
    );
}