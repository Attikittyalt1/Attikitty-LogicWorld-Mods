using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegSideOrTop : BoardPeg
{
    protected override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.z) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon;
    }

    protected override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.x) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon;
    }
}
