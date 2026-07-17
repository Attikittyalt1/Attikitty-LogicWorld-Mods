using HarmonyLib;
using LICC;
using LogicAPI.Data;
using LogicAPI.Services;
using LogicAPI.WorldDataMutations;
using LogicWorld.Server;
using MorePegs.LogicCode.LinkableHandling;
using SkysGeneralLib.Server.TypeExtensions;
using System;
using System.Collections.Generic;

namespace MorePegs.Server;

public interface IHasParentWithPackageManager
{
    public (List<PackageManager> x, List<PackageManager> y) GetActiveManagers();
}