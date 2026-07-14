using EccsLogicWorldAPI.Server;
using LogicAPI.Server;
using LogicAPI.Services;
using LogicAPI.Server.Components;
using LogicWorld.Server;
using LogicWorld.Server.Circuitry;
using LogicWorld.Server.Modules;
using LogicWorld.SharedCode.Components;
using System;
using EccsLogicWorldAPI.Shared.AccessHelper;
using LogicAPI.Data;

namespace MorePegs.Server;

public class MyServer : ServerMod
{
    public const bool DEBUG = false;

    public static IWorldData WorldData { get; private set; }
    public static ComponentTypesManager ComponentTypesManager { get; private set; }

    protected override void Initialize()
    {
        WorldData = ServiceGetter.getService<IWorldData>();
        ComponentTypesManager = ServiceGetter.getService<ComponentTypesManager>();

        VirtualInputPegPool.ensureInitialized();

        ILogicComponentHooks.Init();
    }
}
