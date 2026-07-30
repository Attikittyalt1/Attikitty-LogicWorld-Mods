using LICC;
using SkysCondensedCablingLib.Server;
using System.Linq;

namespace CondensedCablingBasicLogicComponents.Server;

public class SuperRelay : GenericSuperGate
{
    private bool[] _previousState = [];

    public override bool InputAtIndexShouldTriggerComponentLogicUpdates(int inputIndex)
    {
        return inputIndex == 0;
    }

    protected override void DoLogicUpdate()
    {
        UpdateLinks(false);
    }

    protected override void OnCustomDataUpdated()
    {
        base.OnCustomDataUpdated();

        UpdateLinks(true);
    }

    protected override void Initialize()
    {
        base.Initialize();
        UpdateLinks(true);
    }

    private void UpdateLinks(bool isFresh)
    {
        var input0 = Inputs[0] as SuperInputPeg;
        var input1 = Inputs[1] as SuperInputPeg;
        var input2 = Inputs[2] as SuperInputPeg;

        var size = input0.BaseSize;

        if (isFresh)
        {
            if (_previousState.Length != size)
            {
                _previousState = new bool[size];
            }
        }

        for (int i = 0; i < size; i++)
        {
            var newValue = input0[i];

            if (isFresh || _previousState[i] != newValue)
            {
                if (newValue)
                {
                    input1.AddPhasicLinkWith(input2, (i, i));
                }
                else if (!isFresh)
                {
                    input1.RemovePhasicLinkWith(input2, (i, i));
                }

                _previousState[i] = newValue;
            }
        }
    }
}
