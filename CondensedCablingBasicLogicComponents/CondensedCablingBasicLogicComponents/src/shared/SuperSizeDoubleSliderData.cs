namespace CondensedCablingBasicLogicComponents.Shared;

public interface SuperSizeDoubleSliderData
{
    int BitSizeA { get; set; }

    int BitSizeB { get; set; }

    void SetDataDefaultValues()
    {
        BitSizeA = 1;
        BitSizeB = 1;
    }

}