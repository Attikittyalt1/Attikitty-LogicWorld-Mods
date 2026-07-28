using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class MinGate : TernaryComponent
{
    protected override void DoLogicUpdate()
    {
        var minValue = TernaryState.Negative;

        foreach (SuperInputPeg input in Inputs)
        {
            var value = input.GetInTernary();

            minValue = (value, minValue) switch
            {
                (TernaryState.Invalid, _) or
                (TernaryState.Positive, _) or
                (_, TernaryState.Negative) => value,
                _ => minValue
            };

            if (minValue == TernaryState.Invalid)
            {
                break;
            }
        }

        (Outputs[0] as SuperOutputPeg).SetInTernary(minValue);
    }
}