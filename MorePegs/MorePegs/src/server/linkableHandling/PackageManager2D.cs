using LICC;
using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace MorePegs.LogicCode.LinkableHandling;

public class PackageManager2D
{
    private readonly Dictionary<ComponentAddress, (RowPackage x, RowPackage y)> PackagesByAddress = [];

    public void StartTrackingLinkable(LinkableContainer2D container, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var packages))
        {
            packages = (new RowPackage(), new RowPackage());
            PackagesByAddress.Add(address, packages);
        }

        (bool x, bool y) axisStatus = container.GetAxisStatus();
        (LinkableContainer x, LinkableContainer y) containers = container.To1DContainers();

        if (axisStatus.x)
        {
            packages.x.AddLinkable(containers.x);
        }

        if (axisStatus.y)
        {
            packages.y.AddLinkable(containers.y);
        }
    }
    public void StopTrackingLinkable(LinkableContainer2D container, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var packages))
        {
            throw new Exception("Failed to find Packages at provided address");
        }

        (bool x, bool y) axisStatus = container.GetAxisStatus();
        (LinkableContainer x, LinkableContainer y) containers = container.To1DContainers();


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