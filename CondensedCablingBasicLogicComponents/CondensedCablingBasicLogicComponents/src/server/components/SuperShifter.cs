using LICC;
using SkysCondensedCablingLib.Server;
using System;
using System.Linq;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperShifter : GenericSuperGate
{
    public override int InputSuperSize(int index) => index > 1 ? 0 : base.InputSuperSize(index);

    public override bool InputAtIndexShouldTriggerComponentLogicUpdates(int inputIndex) => false;

    protected override void OnCustomDataUpdated()
    {
        base.OnCustomDataUpdated();

        CreateLinks();
    }

    protected override void Initialize()
    {
        base.Initialize();
        CreateLinks();
    }

    private void CreateLinks()
    {
        var dataA = Inputs[0] as SuperInputPeg;
        var dataB = Inputs[1] as SuperInputPeg;
        var carryA = Inputs[2];
        var carryB = Inputs[3];

        var size = Math.Min(dataA.BaseSize, dataB.BaseSize);

        for (int i = 0; i < size - 1; i++)
        {
            dataA.AddPhasicLinkWith(dataB, (i, i+1));
        }

        LConsole.WriteLine(size);
        //dataA.AddPhasicLinkWith(carryB, size - 1);
        dataB.AddPhasicLinkWith(carryA, 0);
    }
}
