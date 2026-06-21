using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace BoardPegs.LogicCode.LinkableHandling;

public class PackageManager<T>
    where T : ILinkable<T>
{
    private readonly Dictionary<ComponentAddress, RowPackage<T>> PackagesByAddress = [];
    public void StartTrackingLinkable(LinkableContainer<T> container, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var package))
        {
            package = new RowPackage<T>();
            PackagesByAddress.Add(address, package);
        }

        package.AddLinkable(container);
    }
    public void StopTrackingLinkable(LinkableContainer<T> container, ComponentAddress address)
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