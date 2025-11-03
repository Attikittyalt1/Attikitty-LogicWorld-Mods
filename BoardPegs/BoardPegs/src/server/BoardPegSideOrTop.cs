using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegSideOrTop : BoardPeg
{
    public override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.z) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon;
    }

    public override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.x) >= Epsilon || Mathf.Abs(Component.localUp.y) >= Epsilon;
    }

}
