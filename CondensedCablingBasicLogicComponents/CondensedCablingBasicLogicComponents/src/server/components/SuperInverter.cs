using SkysCondensedCablingLib.Server;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperInverter : GenericSuperGate
{
    protected override void DoLogicUpdate()
    {
        var output = Outputs[0] as SuperOutputPeg;
        var input = Inputs[0] as SuperInputPeg;

        for (int i = 0; i < output.Size; i++)
        {
            output[i] = !input[i];
        }
    }
}
