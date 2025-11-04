using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace BoardPegs.Logic.BoardPegHandling;

public class PackageManager1D : IPackageManager<Linkable>
{
    private readonly Dictionary<ComponentAddress, IRowPackage> PackagesByAddress = [];
    public void StartTrackingBoardPeg(Linkable linkable, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var package))
        {
            package = new RowPackage();
            PackagesByAddress.Add(address, package);
        }

        package.AddLinkable(linkable);
    }
    public void StopTrackingBoardPeg(Linkable linkable, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var package))
        {
            throw new Exception("Failed to find BoardPegLinkPackage at provided address");
        }

        package.TryRemoveLinkable(linkable);

        if (package.IsEmpty())
        {
            package.Uninitialize();
            PackagesByAddress.Remove(address);
        }
    }
}