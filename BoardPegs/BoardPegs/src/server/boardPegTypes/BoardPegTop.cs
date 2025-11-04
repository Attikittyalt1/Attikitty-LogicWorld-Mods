using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegTop : BoardPeg
{
    protected override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.y) >= Epsilon;
    }

    protected override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.y) >= Epsilon;
    }
}
