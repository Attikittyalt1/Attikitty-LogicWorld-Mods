using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class ConGate : TernaryComponent
{
    protected override void DoLogicUpdate()
    {
        TernaryState? consensus = null;

        foreach (SuperInputPeg input in Inputs)
        {
            var value = input.GetInTernary();

            if (value == TernaryState.Invalid)
            {
                consensus = TernaryState.Invalid;
                break;
            } else if (consensus == null)
            {
                consensus = value;
            } else if (consensus != value)
            {
                consensus = TernaryState.Neutral;
                break;
            }
        }

        (Outputs[0] as SuperOutputPeg).SetInTernary(consensus ?? TernaryState.Neutral);
    }
}