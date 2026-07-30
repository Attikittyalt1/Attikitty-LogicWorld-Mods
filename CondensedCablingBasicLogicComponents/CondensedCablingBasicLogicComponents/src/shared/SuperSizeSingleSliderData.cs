namespace CondensedCablingBasicLogicComponents.Shared;

public interface SuperSizeSingleSliderData
{

    int BitSize { get; set; }

    void SetDataDefaultValues()
    {
        BitSize = 1;
    }

}