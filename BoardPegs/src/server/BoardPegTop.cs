using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegTop : BoardPeg
{
    public override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.y) >= MinimumValue;
    }

    public override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.y) >= MinimumValue;
    }
}
