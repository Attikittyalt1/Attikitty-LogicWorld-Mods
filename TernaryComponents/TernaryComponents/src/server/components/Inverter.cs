using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class Inverter : TernaryComponent
{
    protected override void DoLogicUpdate()
    {
       for (int i = 0; i < Outputs.Count; i++)
        {
            var input = (SuperInputPeg)Inputs[i];
            var output = (SuperOutputPeg)Outputs[i];

            var value = input.GetInTernary() switch
            {
                TernaryState.Neutral => TernaryState.Neutral,
                TernaryState.Negative => TernaryState.Positive,
                TernaryState.Positive => TernaryState.Negative,
                _ => TernaryState.Invalid
            };

            output.SetInTernary(value);
        }
    }
}