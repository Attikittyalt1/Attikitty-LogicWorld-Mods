using CondensedCablingBasicLogicComponents.Shared;
using JimmysUnityUtilities;
using LogicWorld.Server.Circuitry;
using SkysCondensedCablingLib.Server;

namespace CondensedCablingBasicLogicComponents.Server;

public abstract class GenericSingleSliderSuperGate : LogicComponent<SuperSizeSingleSliderData>, IHasSuperPegs
{
    public int PreviousSize = default;

    public virtual int InputSuperSize(int index) => Data.BitSize;
    public virtual int OutputSuperSize(int index) => Data.BitSize;

    protected override void SetDataDefaultValues()
    {
        Data.SetDataDefaultValues();
        PreviousSize = Data.BitSize;
    }

    protected override void Initialize()
    {
        // whyyy do i need to do this??? fix please sky
        this.EnsureSuperPegsAreCorrect();
    }

    protected override void OnCustomDataUpdated()
    {
        // not really needed but its nice to have server side checks
        if (!Data.BitSize.IsBetween(1, MyServer.MaxSuperSize))
        {
            Data.BitSize = Data.BitSize.Clamp(1, MyServer.MaxSuperSize);
        }

        if (PreviousSize == Data.BitSize)
        {
            return;
        }

        this.EnsureSuperPegsAreCorrect();

        PreviousSize = Data.BitSize;
    }
}
