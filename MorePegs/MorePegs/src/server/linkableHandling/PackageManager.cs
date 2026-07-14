using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace MorePegs.LogicCode.LinkableHandling;

public class PackageManager
{
    private readonly Dictionary<ComponentAddress, RowPackage> PackagesByAddress = [];
    public void StartTrackingLinkable(LinkableContainer container, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var package))
        {
            package = new RowPackage();
            PackagesByAddress.Add(address, package);
        }

        package.AddLinkable(container);
    }
    public void StopTrackingLinkable(LinkableContainer container, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var package))
        {
            throw new Exception("Failed to find Package at provided address");
        }

        package.TryRemoveLinkable(container);

        if (package.IsEmpty())
        {
            package.Uninitialize();
            PackagesByAddress.Remove(address);
        }
    }
}