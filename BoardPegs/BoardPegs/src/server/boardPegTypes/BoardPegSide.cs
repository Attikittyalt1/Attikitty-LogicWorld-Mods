using BoardPegs.Logic.BoardPegHandling;
using System.Collections.Generic;
using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegSide : BoardPeg
{
    protected override List<PackageManager2D> FindManagers() => (Component.LocalPositionFixed.y - 75) switch
    {
        > 0 => [],
        < 0 => [],
        _ => [ManagerAtBoardHeight]
    };

    protected override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.z) >= Epsilon;
    }

    protected override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.x) >= Epsilon;
    }
}