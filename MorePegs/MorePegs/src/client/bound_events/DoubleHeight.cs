using FancyInput;
using MorePegs.Client.Inputs;

namespace MorePegs.Client.BoundEvents;

public class DoubleHeight : ModifyHeight
{
    public override InputTrigger OperationStarter => Triggers.DoubleHeight;

    protected override int GetNewInputCount(int inputCount) => inputCount * 2;
}