using CondensedCablingBasicLogicComponents.Shared;
using JimmysUnityUtilities;
using LogicWorld.Server.Circuitry;
using SkysCondensedCablingLib.Server;

namespace CondensedCablingBasicLogicComponents.Server;

public abstract class GenericDoubleSliderSuperGate : LogicComponent<SuperSizeDoubleSliderData>, IHasSuperPegs
{
    public (int, int) PreviousSize = default;

    public virtual int InputSuperSize(int index) => (index & 1) == 0 ? Data.BitSizeA : Data.BitSizeB;
    public virtual int OutputSuperSize(int index) => (index & 1) == 0 ? Data.BitSizeA : Data.BitSizeB;

    protected override void SetDataDefaultValues()
    {
        Data.SetDataDefaultValues();
        PreviousSize = (Data.BitSizeA, Data.BitSizeB);
    }

    protected override void Initialize()
    {
        // whyyy do i need to do this??? fix please sky
        this.EnsureSuperPegsAreCorrect();
    }

    protected override void OnCustomDataUpdated()
    {
        // not really needed but its nice to have server side checks
        if (!Data.BitSizeA.IsBetween(1, MyServer.MaxSuperSize))
        {
            Data.BitSizeA = Data.BitSizeA.Clamp(1, MyServer.MaxSuperSize);
        }

        if (!Data.BitSizeB.IsBetween(1, MyServer.MaxSuperSize))
        {
            Data.BitSizeB = Data.BitSizeB.Clamp(1, MyServer.MaxSuperSize);
        }

        if (PreviousSize == (Data.BitSizeA, Data.BitSizeB))
        {
            return;
        }

        this.EnsureSuperPegsAreCorrect();

        PreviousSize = (Data.BitSizeA, Data.BitSizeB);
    }
}
