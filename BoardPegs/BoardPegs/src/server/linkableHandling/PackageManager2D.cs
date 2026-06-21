using LICC;
using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace BoardPegs.LogicCode.LinkableHandling;

public class PackageManager2D<T>
    where T : ILinkable<T>
{
    private readonly Dictionary<ComponentAddress, (RowPackage<T> x, RowPackage<T> y)> PackagesByAddress = [];

    public void StartTrackingLinkable(LinkableContainer2D<T> container, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var packages))
        {
            packages = (new RowPackage<T>(), new RowPackage<T>());
            PackagesByAddress.Add(address, packages);
        }

        (bool x, bool y) axisStatus = container.GetAxisStatus();
        (LinkableContainer<T> x, LinkableContainer<T> y) containers = container.To1DContainers();

        if (axisStatus.x)
        {
            packages.x.AddLinkable(containers.x);
        }

        if (axisStatus.y)
        {
            packages.y.AddLinkable(containers.y);
        }
    }
    public void StopTrackingLinkable(LinkableContainer2D<T> container, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var packages))
        {
            throw new Exception("Failed to find Packages at provided address");
        }

        (bool x, bool y) axisStatus = container.GetAxisStatus();
        (LinkableContainer<T> x, LinkableContainer<T> y) containers = container.To1DContainers();


        packages.x.TryRemoveLinkable(containers.x);
        packages.y.TryRemoveLinkable(containers.y);

        if (packages.x.IsEmpty() && packages.y.IsEmpty())
        {
            packages.x.Uninitialize();
            packages.y.Uninitialize();
            PackagesByAddress.Remove(address);
        }
    }
}