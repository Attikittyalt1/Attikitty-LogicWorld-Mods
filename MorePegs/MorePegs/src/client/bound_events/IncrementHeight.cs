using FancyInput;
using MorePegs.Client.Inputs;

namespace MorePegs.Client.BoundEvents;

public class IncrementHeight : ModifyHeight
{
    public override InputTrigger OperationStarter => Triggers.IncrementHeight;

    protected override int GetNewInputCount(int inputCount) => inputCount + 1;
}