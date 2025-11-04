using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegSide : BoardPegTrackable
{
    public override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localUp.z) >= Epsilon;
    }

    public override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localUp.x) >= Epsilon;
    }
}