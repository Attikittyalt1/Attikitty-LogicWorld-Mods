using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace BoardPegs.Logic.BoardPegLink;

public class PackageManager1D : IPackageManager<Link>
{
    private readonly Dictionary<ComponentAddress, IPackage> PackagesByAddress = [];
    public void StartTrackingBoardPeg(Link link, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var package))
        {
            package = new Package();
            PackagesByAddress.Add(address, package);
        }

        package.AddLink(link);
    }
    public void StopTrackingBoardPeg(Link link, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var package))
        {
            throw new Exception("Failed to find BoardPegLinkPackage at provided address");
        }

        package.TryRemoveLink(link);

        if (package.IsEmpty())
        {
            package.Uninitialize();
            PackagesByAddress.Remove(address);
        }
    }
}