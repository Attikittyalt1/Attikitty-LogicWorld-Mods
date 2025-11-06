using BoardPegs.Server;
using LICC;
using LogicAPI.Data;
using System;
using System.Collections.Generic;

namespace BoardPegs.Logic.BoardPegHandling;

public class PackageManager2D : IPackageManager<Linkable2D>
{
    private readonly Dictionary<ComponentAddress, (IRowPackage horizontal, IRowPackage vertical)> PackagesByAddress = [];

    public void StartTrackingBoardPeg(Linkable2D linkable, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var pair))
        {
            pair.horizontal = new RowPackage();
            pair.vertical = new RowPackage();
            PackagesByAddress.Add(address, pair);
        }

        if (linkable.ShouldBeLinkedHorizontally())
        {
            if (MyServer.DEBUG) LConsole.WriteLine("linking horizontally");
            pair.horizontal.AddLinkable(linkable.ToHorizontalLinkable());
        }

        if (linkable.ShouldBeLinkedVertically())
        {
            if (MyServer.DEBUG) LConsole.WriteLine("linking vertically");
            pair.vertical.AddLinkable(linkable.ToVerticalLinkable());
        }
    }

    public void StopTrackingBoardPeg(Linkable2D linkable, ComponentAddress address)
    {
        if (!PackagesByAddress.TryGetValue(address, out var pair))
        {
            throw new Exception("Failed to find BoardPegLinkPackages at provided address");
        }

        if (MyServer.DEBUG && pair.horizontal.HasLinkable(linkable.ToHorizontalLinkable()))
        {
            LConsole.WriteLine("unlinking horizontally");
        }

        pair.horizontal.TryRemoveLinkable(linkable.ToHorizontalLinkable());

        if (MyServer.DEBUG && pair.vertical.HasLinkable(linkable.ToVerticalLinkable()))
        {
            LConsole.WriteLine("unlinking vertically");
        }

        pair.vertical.TryRemoveLinkable(linkable.ToVerticalLinkable());

        if (pair.horizontal.IsEmpty() && pair.vertical.IsEmpty())
        {
            pair.horizontal.Uninitialize();
            pair.vertical.Uninitialize();
            PackagesByAddress.Remove(address);
        }
    }
}