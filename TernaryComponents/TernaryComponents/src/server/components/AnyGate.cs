using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class AnyGate : TernaryComponent
{
    protected override void DoLogicUpdate()
    {
        TernaryState consensus = TernaryState.Neutral;

        foreach (SuperInputPeg input in Inputs)
        {
            var value = input.GetInTernary();

            if (value == TernaryState.Invalid)
            {
                consensus = TernaryState.Invalid;
                break;
            } else if (consensus == TernaryState.Neutral)
            {
                consensus = value;
            } else if (consensus != value && value != TernaryState.Neutral)
            {
                consensus = TernaryState.Neutral;
                break;
            }
        }

        (Outputs[0] as SuperOutputPeg).SetInTernary(consensus);
    }
}