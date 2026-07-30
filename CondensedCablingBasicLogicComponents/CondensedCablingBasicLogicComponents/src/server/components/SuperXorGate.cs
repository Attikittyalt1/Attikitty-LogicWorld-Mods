using SkysCondensedCablingLib.Server;
using System.Linq;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperXorGate : GenericSingleSliderSuperGate
{
    protected override void DoLogicUpdate()
    {
        var output = Outputs[0] as SuperOutputPeg;

        for (int i = 0; i < output.Size; i++)
        {
            output[i] = Inputs.Aggregate(false, (value, input) => value ^ (input as SuperInputPeg)[i]);
        }
    }
}
