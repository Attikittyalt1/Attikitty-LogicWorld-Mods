using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegTop : BoardPegTrackable
{
    public override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.y) >= Epsilon;
    }

    public override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.y) >= Epsilon;
    }
}
