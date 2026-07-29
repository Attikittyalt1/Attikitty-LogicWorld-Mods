using JimmysUnityUtilities;
using LogicWorld.Server.Circuitry;
using LogicWorld.SharedCode.ComponentCustomData;
using System.Diagnostics;

namespace AttikittyMiscellaneousComponents.LogicCode;

public class StreamDelayer : LogicComponent<IDelayerData>
{
    protected override void DoLogicUpdate()
    {
        if (Data.DelayCounter != 0)
        {
            if (Data.DelayLengthInTicks > 1)
            {
                Outputs[0].On = (Data.DelayCounter & (1 << (Data.DelayLengthInTicks - 2))) != 0;
            }

            Data.DelayCounter = Data.DelayCounter << 1;
        }

        if (Inputs[0].On)
        {
            if (Data.DelayLengthInTicks == 1)
            {
                Outputs[0].On = Inputs[0].On;
            }

            Data.DelayCounter = Data.DelayCounter | 1;
        }

        if (Data.DelayCounter != 0)
        {
            QueueLogicUpdate();
        }
    }

    protected override void SetDataDefaultValues()
    {
        Data.SetDefaultValues();
    }
}