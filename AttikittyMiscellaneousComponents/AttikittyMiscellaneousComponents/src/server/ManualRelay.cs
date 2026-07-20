using LogicWorld.Server.Circuitry;
using LogicWorld.SharedCode.ComponentCustomData;

namespace AttikittyMiscellaneousComponents.LogicCode;

public class ManualRelay : LogicComponent<ISwitchData>
{
    private bool PreviouslyOn;

    protected override void OnCustomDataUpdated()
    {
        QueueLogicUpdate();
    }

    protected override void Initialize()
    {

        int addIndexDelta = Data.On ? 1 : 0;

        for (int i = 0; i < Inputs.Count / 4; i++)
        {
            int iFrom = 4 * i;
            int iTo = iFrom + 2;

            Inputs[iFrom + addIndexDelta].AddSecretLinkWith(Inputs[iTo + addIndexDelta]);
        }
    }

    protected override void DoLogicUpdate()
    {
        if (Data.On == PreviouslyOn)
        {
            return;
        }

        int removeIndexDelta;
        int addIndexDelta;

        if (Data.On)
        {
            removeIndexDelta = 0;
            addIndexDelta = 1;
        }
        else
        {
            removeIndexDelta = 1;
            addIndexDelta = 0;
        }

        for (int i = 0; i < Inputs.Count / 4; i++)
        {
            int iFrom = 4 * i;
            int iTo = iFrom + 2;

            Inputs[iFrom + removeIndexDelta].RemoveSecretLinkWith(Inputs[iTo + removeIndexDelta]);
            Inputs[iFrom + addIndexDelta].AddSecretLinkWith(Inputs[iTo + addIndexDelta]);
        }

        PreviouslyOn = Data.On;
    }

    protected override void SetDataDefaultValues()
    {
        Data.SetDefaultValues();
        PreviouslyOn = Data.On;
    }
}