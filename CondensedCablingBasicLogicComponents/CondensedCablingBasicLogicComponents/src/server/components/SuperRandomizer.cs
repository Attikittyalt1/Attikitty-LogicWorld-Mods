using JimmysUnityUtilities.Random;
using SkysCondensedCablingLib.Server;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperRandomizer : GenericSuperGate
{
    private static readonly JRandom random = new JRandom();

    public override int InputSuperSize(int index) => 0;

    private bool inputPreviouslyOn;
    protected override void DoLogicUpdate()
    {
        var output = Outputs[0] as SuperOutputPeg;

        if (Inputs[0].On)
        {
            if (!inputPreviouslyOn)
            {
                inputPreviouslyOn = true;

                for (int i = 0; i < output.Size; i++)
                {
                    bool state = random.FiftyFifty();
                    output[i] = state;
                }
            }
        }
        else
        {
            inputPreviouslyOn = false;

            for (int i = 0; i < output.Size; i++)
            {
                output[0] = false;
            }
        }
    }
}
