using JimmysUnityUtilities;
using LICC;
using MorePegs.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using UnityEngine;

namespace MorePegs.LogicCode.LinkableHandling;

class RowPackage
{
    private Dictionary<LinkableContainer, int> LinkablePositions = [];

    private Dictionary<int, LinkedRow> LinkedRows = [];

    public bool IsEmpty() => LinkablePositions.IsEmpty();

    public bool HasLinkable(LinkableContainer container)
    {
        return LinkablePositions.ContainsKey(container);
    }

    // use UninitializeAndClear if it is possible for the instance to still be accessed afterwords, though it probably shouldn't be
    public void Uninitialize()
    {
        if (!IsEmpty())
        {
            throw new Exception("Tried to uninitialize package that is not empty");
        }

        foreach (var linkedRow in LinkedRows.Values)
        {
            if (linkedRow.IsInitialized())
            {
                linkedRow.Uninitialize();
            }
        }
    }

    public void UninitializeAndClear()
    {
        foreach (var linkedRow in LinkedRows.Values)
        {
            if (linkedRow.IsInitialized())
            {
                linkedRow.UninitializeAndClear();
            }
        }

        LinkablePositions.Clear();
        LinkedRows.Clear();
    }

    public void AddLinkable(LinkableContainer container)
    {
        if (LinkablePositions.ContainsKey(container))
        {
            throw new Exception("Tried to add linkable to package that already contains it");
        }

        var position = container.GetLinkingPosition();

        LinkAtPosition(container, position);

        LinkablePositions.Add(container, position);
    }

    public void RemoveLinkable(LinkableContainer container)
    {
        if (!LinkablePositions.TryGetValue(container, out var position))
        {
            throw new Exception("Tried to remove linkable from package that does not contain it");
        }

        UnlinkAtPosition(container, position);

        LinkablePositions.Remove(container);
    }

    public void UpdatePositions()
    {
        int positionChange = GetPositionChangeFromBoard();
        if (positionChange == 0) return;

        ChangePositionsInUnision(positionChange);
    }

    private int GetPositionChangeFromBoard()
    {
        var (container, oldPosition) = LinkablePositions.First();

        return container.GetLinkingPosition() - oldPosition;
    }

    private void ChangePositionsInUnision(int deltaPos)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("changing positions with delta: " + deltaPos);

        // i should really use an immutable approach. maybe just a foreach loop would better but this just looks so nice
        LinkablePositions = LinkablePositions.ToDictionary(
            entry => entry.Key,
            entry => entry.Value + deltaPos
        );

        // same thing as before
        LinkedRows = LinkedRows.ToDictionary(
            entry => entry.Key + deltaPos,
            entry => entry.Value
        );
    }

    private void LinkAtPosition(LinkableContainer container, int position)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("started to link at position: {0}", position);

        if (!LinkedRows.TryGetValue(position, out var linkedRow))
        {
            linkedRow = new LinkedRow()
            {
                //MaxLonelies = 2
            };
            LinkedRows.Add(position, linkedRow);
        }

        linkedRow.AddLinkable(container.Linkable);
    }

    private void UnlinkAtPosition(LinkableContainer container, int position)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("started to unlink at position: {0}", position);

        if (!LinkedRows.TryGetValue(position, out var linkedRow))
        {
            throw new Exception("Linked Row could not be found in package at position " + position);
        }

        linkedRow.RemoveLinkable(container.Linkable);

        if (linkedRow.IsEmpty())
        {
            LinkedRows.Remove(position);
        }
    }

    public bool TryAddLinkable(LinkableContainer container)
    {
        if (!HasLinkable(container))
        {
            AddLinkable(container);
        }

        return false;
    }

    public bool TryRemoveLinkable(LinkableContainer container)
    {
        if (HasLinkable(container))
        {
            RemoveLinkable(container);
        }

        return false;
    }
}
