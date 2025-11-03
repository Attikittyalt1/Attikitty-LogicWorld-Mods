using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegSideOrTop : BoardPeg
{
    public override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.z) >= MinimumValue || Mathf.Abs(Component.localUp.y) >= MinimumValue;
    }

    public override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.x) >= MinimumValue || Mathf.Abs(Component.localUp.y) >= MinimumValue;
    }

}
