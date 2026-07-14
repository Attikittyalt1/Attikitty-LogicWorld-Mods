using FancyInput;
using MorePegs.Client.Inputs;

namespace MorePegs.Client.BoundEvents;

public class DecrementHeight : ModifyHeight
{
    public override InputTrigger OperationStarter => Triggers.DecrementHeight;

    protected override int GetNewInputCount(int inputCount) => inputCount - 1;
}