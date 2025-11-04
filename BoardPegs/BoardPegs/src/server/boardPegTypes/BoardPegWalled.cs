using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegWalled : BoardPeg
{
    protected override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localForward.z) >= Epsilon;
    }

    protected override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localForward.x) >= Epsilon;
    }
}
