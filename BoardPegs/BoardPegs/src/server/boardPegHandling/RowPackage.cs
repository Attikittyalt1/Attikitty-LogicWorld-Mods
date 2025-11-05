using JimmysUnityUtilities;
using LICC;
using System;
using System.Collections.Generic;
using System.Linq;
using BoardPegs.Server;

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

        foreach (var value in LinkedRows)
        {
            var linkedRow = value.Value;
            if (linkedRow.IsInitialized())
            {
                linkedRow.Uninitialize();
            }
        }
    }

    public void UninitializeAndClear()
    {
        foreach (var value in LinkedRows)
        {
            var linkedRow = value.Value;
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
        CheckForPositionChanges();

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
        CheckForPositionChanges();

        if (!LinkablePositions.TryGetValue(linkable, out var position))
        {
            throw new Exception("Tried to remove linkable from package that does not contain it");
        }

        UnlinkAtPosition(linkable, position);

        LinkablePositions.Remove(linkable);
    }

    private void CheckForPositionChanges()
    {
        if (!HasPositionChanged())
        {
            return;
        }

        foreach (var pair in LinkablePositions)
        {

            var linkable = pair.Key;
            var position = pair.Value;

            UnlinkAtPosition(linkable, position);

            var newPosition = linkable.GetLinkingPosition();

            LinkAtPosition(linkable, newPosition);

            LinkablePositions[linkable] = newPosition;
        }
    }

    private bool HasPositionChanged()
    {
        if (LinkablePositions.Count == 0)
        {
            return false;
        }

        var firstPair = LinkablePositions.First();
        return firstPair.Key.GetLinkingPosition() != firstPair.Value;
    }

    private void LinkAtPosition(Linkable linkable, int position)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("started to link at position: {0}", position);

        if (!LinkedRows.TryGetValue(position, out var linkedRow))
        {
            linkedRow = new LinkedRowWithPegs() 
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
