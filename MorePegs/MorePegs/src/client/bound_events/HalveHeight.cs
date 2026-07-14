using FancyInput;
using MorePegs.Client.Inputs;

namespace MorePegs.Client.BoundEvents;

public class HalveHeight : ModifyHeight
{
    public override InputTrigger OperationStarter => Triggers.HalveHeight;

    protected override int GetNewInputCount(int inputCount) => inputCount / 2;
}