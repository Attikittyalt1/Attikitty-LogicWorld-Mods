using JimmysUnityUtilities;
using LICC;
using System;
using System.Collections.Generic;
using System.Linq;
using BoardPegs.Server;
using UnityEngine;

namespace BoardPegs.Logic.BoardPegHandling;

class RowPackage : IRowPackage
{
    private Dictionary<Linkable, int> LinkablePositions = [];

    private Dictionary<int, ILinkedRow> LinkedRows = [];

    public bool IsEmpty() => LinkablePositions.IsEmpty();

    public bool HasLinkable(Linkable linkable)
    {
        return LinkablePositions.ContainsKey(linkable);
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

    public void AddLinkable(Linkable linkable)
    {
        CheckForPositionChangeFromBoard();

        if (LinkablePositions.ContainsKey(linkable))
        {
            throw new Exception("Tried to add linkable to package that already contains it");
        }

        var position = linkable.GetLinkingPosition();

        LinkAtPosition(linkable, position);

        LinkablePositions.Add(linkable, position);
    }

    public void RemoveLinkable(Linkable linkable)
    {
        if (!LinkablePositions.TryGetValue(linkable, out var position))
        {
            throw new Exception("Tried to remove linkable from package that does not contain it");
        }

        UnlinkAtPosition(linkable, position);

        LinkablePositions.Remove(linkable);
    }

    private void CheckForPositionChangeFromBoard()
    {
        int positionChange = GetPositionChangeFromBoard();
        if (positionChange == 0) return;

        // i should really use an immutable approach. maybe just a foreach loop would better but this just looks so nice
        LinkablePositions = LinkablePositions.ToDictionary(
            entry => entry.Key,
            entry => entry.Value + positionChange
        );

        // same thing as before
        LinkedRows = LinkedRows.ToDictionary(
            entry => entry.Key + positionChange,
            entry => entry.Value
        );
    }

    private int GetPositionChangeFromBoard()
    {
        // this function makes the assumption that if a peg's global position has not changed, then any local position changes must be due to a board resizing

        foreach (var (linkable, oldPosition) in LinkablePositions)
        {
            var change = linkable.GetLinkingPosition() - oldPosition;

            if (!linkable.HasBeenMoved() && change != 0)
            {
                return change;
            }
        }

        return 0;
    }

    private void LinkAtPosition(Linkable linkable, int position)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("started to link at position: {0}", position);

        if (!LinkedRows.TryGetValue(position, out var linkedRow))
        {
            linkedRow = new LinkedRowWithManualLinking()
            {
                //MaxLonelies = 2
            };
            LinkedRows.Add(position, linkedRow);
        }

        linkedRow.AddPeg(linkable.LinkablePeg);
    }

    private void UnlinkAtPosition(Linkable linkable, int position)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("started to unlink at position: {0}", position);

        if (!LinkedRows.TryGetValue(position, out var linkedRow))
        {
            throw new Exception("Linked Row could not be found in package at position " + position);
        }

        linkedRow.RemovePeg(linkable.LinkablePeg);

        if (linkedRow.IsEmpty())
        {
            LinkedRows.Remove(position);
        }
    }
}
