using LogicAPI.Data;
using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegWalled : BoardPegTrackable
{

    public override bool ShouldBeLinkedHorizontally()
    {
        return Mathf.Abs(Component.localForward.z) >= Epsilon;
    }

    public override bool ShouldBeLinkedVertically()
    {
        return Mathf.Abs(Component.localForward.x) >= Epsilon;
    }
}
