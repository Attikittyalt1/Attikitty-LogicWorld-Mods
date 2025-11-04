using BoardPegs.Server;
using LICC;
using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace BoardPegs.Logic.BoardPegLink;

public class PackageManager2D : IPackageManager<LinkWrapper2D>
{
    private readonly Dictionary<ComponentAddress, (IPackage horizontal, IPackage vertical)> PackagesByAddress = [];

    public void StartTrackingBoardPeg(LinkWrapper2D link, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var pair))
        {
            pair.horizontal = new Package();
            pair.vertical = new Package();
            PackagesByAddress.Add(address, pair);
        }

        if (link.ShouldBeLinkedHorizontally())
        {
            if (MyServer.DEBUG) LConsole.WriteLine("linking horizontally");
            pair.horizontal.AddLink(link.ToHorizontalLink());
        }

        if (link.ShouldBeLinkedVertically())
        {
            if (MyServer.DEBUG) LConsole.WriteLine("linking vertically");
            pair.vertical.AddLink(link.ToVerticalLink());
        }
    }

    public void StopTrackingBoardPeg(LinkWrapper2D link, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var pair))
        {
            throw new Exception("Failed to find BoardPegLinkPackages at provided address");
        }

        if (pair.horizontal.TryRemoveLink(link.ToHorizontalLink()) && MyServer.DEBUG)
        {
            LConsole.WriteLine("unlinked horizontally");
        }

        if (pair.vertical.TryRemoveLink(link.ToVerticalLink()) && MyServer.DEBUG)
        {
            LConsole.WriteLine("unlinked vertically");
        }

        if (pair.horizontal.IsEmpty() && pair.vertical.IsEmpty())
        {
            pair.horizontal.Uninitialize();
            pair.vertical.Uninitialize();
            PackagesByAddress.Remove(address);
        }
    }
}