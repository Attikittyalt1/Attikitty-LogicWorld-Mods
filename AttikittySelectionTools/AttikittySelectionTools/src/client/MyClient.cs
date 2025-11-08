using FancyInput;
using LogicAPI.Client;

namespace AttikittySelectionTools.Client;

public class MyClient : ClientMod
{
    public readonly static SelectionManager SelectionManager = new();

    protected override void Initialize()
    {
        CustomInput.Register<Inputs.Context, Inputs.Triggers>("AttikittySelectionTools");
    }
}
