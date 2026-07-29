using JimmysUnityUtilities;
using LogicWorld.Server.Circuitry;
using LogicWorld.SharedCode.ComponentCustomData;
using System.Diagnostics;

namespace AttikittyMiscellaneousComponents.LogicCode;

public class StreamDelayer : LogicComponent<IDelayerData>
{
    protected override void OnCustomDataUpdated()
    {
        if (Data.DelayCounter == 0)
        {
            return;
        }

        var diff = Data.DelayLengthInTicks - prevLength;

        if (diff == 0)
        {
            return;
        }

        if (diff > 0)
        {
            Data.DelayCounter = Data.DelayCounter << diff;
        }

        if (diff < 0)
        {
            Data.DelayCounter = Data.DelayCounter >>> -diff;
        }

        prevLength = Data.DelayLengthInTicks;
    }

    int prevLength;

    protected override void Initialize()
    {
        
    }

    protected override void DoLogicUpdate()
    {
        if (Data.DelayCounter != 0)
        {
            Data.DelayCounter = Data.DelayCounter >>> 1;

            Outputs[0].On = (Data.DelayCounter & 1) > 0;

        }

        if (Inputs[0].On)
        {
            Data.DelayCounter = Data.DelayCounter | (1 << (Data.DelayLengthInTicks - 1));
        }

        if (Data.DelayCounter != 0)
        {
            QueueLogicUpdate();
        }
    }

    protected override void SetDataDefaultValues()
    {
        Data.SetDefaultValues();
        prevLength = Data.DelayLengthInTicks;
    }
}