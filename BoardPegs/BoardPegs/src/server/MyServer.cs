using EccsLogicWorldAPI.Server;
using LogicAPI.Server;
using LogicAPI.Services;
using LogicWorld.SharedCode.Components;

namespace BoardPegs.Server;

public class MyServer : ServerMod
{
    public const bool DEBUG = true;

    public static IWorldData WorldData { get; private set; }
    public static ComponentTypesManager ComponentTypesManager { get; private set; }

    protected override void Initialize()
    {
        WorldData = ServiceGetter.getService<IWorldData>();
        ComponentTypesManager = ServiceGetter.getService<ComponentTypesManager>();

        VirtualInputPegPool.ensureInitialized();
    }
}
