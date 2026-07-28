using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public class DirectionalRelay : TernaryComponent
{
    public override int InputSuperSize(int index) => index != 0 ? 0 : base.InputSuperSize(index);

    TernaryState prevState;

    protected override void Initialize()
    {
        prevState = TernaryState.Neutral;
    }

    protected override void DoLogicUpdate()
    {
        var state = ((SuperInputPeg)Inputs[0]).GetInTernary();

        if (state == TernaryState.Invalid || state == prevState)
        {
            return;
        }

        if (prevState == TernaryState.Positive)
        {
            for (int i = 1; i < Inputs.Count - 1; i += 2)
            {
                Inputs[i].RemoveOneWayPhasicLinkTo(Inputs[i + 1]);
            }
        }

        if (prevState == TernaryState.Negative)
        {
            for (int i = 1; i < Inputs.Count - 1; i += 2)
            {
                Inputs[i + 1].RemoveOneWayPhasicLinkTo(Inputs[i]);
            }
        }

        if (state == TernaryState.Positive)
        {
            for (int i = 1; i < Inputs.Count - 1; i += 2)
            {
                Inputs[i].AddOneWayPhasicLinkTo(Inputs[i + 1]);
            }
        }

        if (state == TernaryState.Negative)
        {
            for (int i = 1; i < Inputs.Count - 1; i += 2)
            {
                Inputs[i + 1].AddOneWayPhasicLinkTo(Inputs[i]);
            }
        }

        prevState = state;
    }
}