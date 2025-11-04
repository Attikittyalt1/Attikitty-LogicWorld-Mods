using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegWalled : BoardPeg
{
    public override bool ShouldBeLinkedHorizontally()
    {
        return 
            (Mathf.Abs(Component.localUp.z) >= Epsilon && Mathf.Abs(Component.localForward.y) >= Epsilon) || 
            (Mathf.Abs(Component.localUp.y) >= Epsilon && Mathf.Abs(Component.localForward.z) >= Epsilon);
    }

    public override bool ShouldBeLinkedVertically()
    {
        return 
            (Mathf.Abs(Component.localUp.x) >= Epsilon && Mathf.Abs(Component.localForward.y) >= Epsilon) || 
            (Mathf.Abs(Component.localUp.y) >= Epsilon && Mathf.Abs(Component.localForward.x) >= Epsilon);
    }
}
