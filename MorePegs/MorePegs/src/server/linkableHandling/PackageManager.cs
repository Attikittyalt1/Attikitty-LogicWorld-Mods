using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace MorePegs.LogicCode.LinkableHandling;

public class PackageManager
{
    private readonly Dictionary<ComponentAddress, RowPackage> _packagesByAddress = [];
    public void StartTrackingLinkable(LinkableContainer container, ComponentAddress address)
    {
        if (!_packagesByAddress.TryGetValue(address, out var package))
        {
            package = new RowPackage();
            _packagesByAddress.Add(address, package);
        }

        package.AddLinkable(container);
    }
    public void StopTrackingLinkable(LinkableContainer container, ComponentAddress address)
    {
        if (!_packagesByAddress.TryGetValue(address, out var package))
        {
            throw new Exception("Failed to find Package at provided address");
        }

        package.TryRemoveLinkable(container);

        if (package.IsEmpty())
        {
            package.Uninitialize();
            _packagesByAddress.Remove(address);
        }
    }

    public bool HasPackgesAtAddress(ComponentAddress address)
    {
        return _packagesByAddress.ContainsKey(address);
    }

    public void OffsetPositions(ComponentAddress address, int deltaPos)
    {
        if (!_packagesByAddress.TryGetValue(address, out var package))
        {
            throw new Exception("Failed to find Package at provided address");
        }

        package.OffsetPositions(deltaPos);
    }
}