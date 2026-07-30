using LICC;
using SkysCondensedCablingLib.Server;
using System;
using System.Linq;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperShifter : GenericDoubleSliderSuperGate
{
    public override bool InputAtIndexShouldTriggerComponentLogicUpdates(int inputIndex) => false;

    protected override void OnCustomDataUpdated()
    {
        base.OnCustomDataUpdated();

        foreach (SuperInputPeg input in Inputs)
        {
            foreach (var (other, channel) in input?.PartialPhasicLinks.ToList() ?? [])
                input.RemovePhasicLinkWith(other, channel);

        }
        CreateLinks();
    }

    protected override void Initialize()
    {
        base.Initialize();
        CreateLinks();
    }

    private void CreateLinks()
    {
        //LConsole.WriteLine("sizeA: {0}, sizeB: {1}", Data.BitSizeA, Data.BitSizeB);

        var centerA = Inputs[0] as SuperInputPeg;
        var sideA = Inputs[1] as SuperInputPeg;
        var centerB = Inputs[2] as SuperInputPeg;
        var sideB = Inputs[3] as SuperInputPeg;

        var sideSize = Math.Min(sideA.BaseSize, sideB.BaseSize);
        var centerSize = Math.Min(centerA.BaseSize, centerB.BaseSize);

        for (int i = 0; i < sideSize + centerSize; i++)
        {
            SuperInputPeg pegA;
            SuperInputPeg pegB;
            int iA;
            int iB;

            if (i < sideSize)
            {
                pegA = sideA;
                iA = i;
            }
            else
            {
                pegA = centerA;
                iA = i - sideSize;
            }

            if (i < centerSize)
            {
                pegB = centerB;
                iB = i;
            }
            else
            {
                pegB = sideB;
                iB = i - centerSize;
            }

            pegA.AddPhasicLinkWith(pegB, (iA, iB));
        }
    }
}
