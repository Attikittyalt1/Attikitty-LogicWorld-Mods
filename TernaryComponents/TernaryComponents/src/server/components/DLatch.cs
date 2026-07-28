using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class DLatch : TernaryComponent
{
    protected override void DoLogicUpdate()
    {
        var state = ((SuperInputPeg)Inputs[0]).GetInTernary();

        
        if (state == TernaryState.Positive)
        {
            for (int i = 0; i < Outputs.Count; i++)
            {
                var output = (SuperOutputPeg)Outputs[i];
                var input = (SuperInputPeg)Inputs[i + 1];

                var currentState = input.GetInTernary();

                output.SetInTernary(currentState);
            }
        }

        if (state == TernaryState.Negative)
        {
            for (int i = 0; i < Outputs.Count; i++)
            {
                var output = (SuperOutputPeg)Outputs[i];

                output.SetInTernary(TernaryState.Neutral);
            }
        }

        if (state == TernaryState.Invalid)
        {
            for (int i = 0; i < Outputs.Count; i++)
            {
                var output = (SuperOutputPeg)Outputs[i];

                output.SetInTernary(TernaryState.Invalid);
            }
        }
    }
}