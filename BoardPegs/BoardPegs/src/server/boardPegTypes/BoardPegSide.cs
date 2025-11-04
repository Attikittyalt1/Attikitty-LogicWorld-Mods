using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegSide : BoardPeg
{
    protected override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.z) >= Epsilon;
    }

    protected override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.x) >= Epsilon;
    }
}