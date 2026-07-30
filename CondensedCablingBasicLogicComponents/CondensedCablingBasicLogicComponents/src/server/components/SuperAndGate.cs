using SkysCondensedCablingLib.Server;
using System.Linq;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperAndGate : GenericSingleSliderSuperGate
{
    protected override void DoLogicUpdate()
    {
        var output = Outputs[0] as SuperOutputPeg;

        for (int i = 0; i < output.Size; i++)
        {
            output[i] = Inputs.All(input => (input as SuperInputPeg)[i]);
        }
    }
}
