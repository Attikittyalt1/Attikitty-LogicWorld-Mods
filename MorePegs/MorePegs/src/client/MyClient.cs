using FancyInput;
using LogicAPI.Client;
using LogicSettings;
using MorePegs.Client.Inputs;

namespace MorePegs.Client;

public class MyClient : ClientMod
{
    protected override void Initialize()
    {
        CustomInput.Register<Context, Triggers>(Manifest.ID);
    }
}
