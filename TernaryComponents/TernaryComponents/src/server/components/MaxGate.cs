using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class MaxGate : TernaryComponent
{
    protected override void DoLogicUpdate()
    {
        var maxValue = TernaryState.Positive;

        foreach (SuperInputPeg input in Inputs)
        {
            var value = input.GetInTernary();

            maxValue = (value, maxValue) switch
            {
                (TernaryState.Invalid, _) or
                (TernaryState.Negative, _) or
                (_, TernaryState.Positive) => value,
                _ => maxValue
            };

            if (maxValue == TernaryState.Invalid)
            {
                break;
            }
        }

        (Outputs[0] as SuperOutputPeg).SetInTernary(maxValue);
    }
}