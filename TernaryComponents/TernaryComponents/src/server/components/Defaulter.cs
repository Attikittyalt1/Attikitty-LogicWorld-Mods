using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class Defaulter : TernaryComponent
{
    protected override void DoLogicUpdate()
    {
        var defaultState = ((SuperInputPeg)Inputs[0]).GetInTernary();

        for (int i = 0; i < Outputs.Count; i++)
        {
            var output = (SuperOutputPeg)Outputs[i];
            var input = (SuperInputPeg)Inputs[i + 1];

            var currentState = input.GetInTernary();

            output.SetInTernary(currentState == TernaryState.Invalid ? defaultState : currentState);
        }
    }
}