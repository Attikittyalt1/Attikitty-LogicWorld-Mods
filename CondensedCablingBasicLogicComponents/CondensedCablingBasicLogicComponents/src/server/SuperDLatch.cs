using SkysCondensedCablingLib.Server;
using System.Linq;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperDLatch : GenericSuperGate
{
    public override int InputSuperSize(int index) => index == 1 ? 0 : base.InputSuperSize(index);

    protected override void DoLogicUpdate()
    {
        if (Inputs[1].On)
        {
            var output = Outputs[0] as SuperOutputPeg;
            var input = Inputs[0] as SuperInputPeg;

            for (int i = 0; i < output.Size; i++)
            {
                output[i] = input[i];
            }
        }
    }
}
