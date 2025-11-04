using EccsLogicWorldAPI.Server;
using JimmysUnityUtilities;
using LICC;
using LogicWorld.Server.Circuitry;
using System;
using System.Collections.Generic;
using System.Linq;
using BoardPegs.Server;

namespace BoardPegs.Logic.BoardPegLink;

class Package : IPackage
{
    class HiddenPeg
    {
        private InputPeg hiddenPeg;
        private int count;

        private void InitializeHiddenPeg()
        {
            if (hiddenPeg != null)
            {
                throw new Exception("Tried to initialize hidden peg that already is initialized");
            }

            if (MyServer.DEBUG) LConsole.WriteLine("borrowed peg");
            hiddenPeg = VirtualInputPegPool.borrowPeg();
        }

        private void UninitializeHiddenPeg()
        {
            if (hiddenPeg == null)
            {
                throw new Exception("Tried to uninitialize hidden peg that is not initialized");
            }


            if (MyServer.DEBUG) LConsole.WriteLine("returned peg");
            hiddenPeg.RemoveAllSecretLinks();
            VirtualInputPegPool.returnPeg(hiddenPeg);
            hiddenPeg = null;
        }

        private void LinkPeg(Link peg)
        {
            if (hiddenPeg == null)
            {
                throw new Exception("Tried to link board peg to hidden peg that does not exist");
            }

            peg.LinkPeg(hiddenPeg);
        }

        private void UnlinkPeg(Link peg)
        {
            if (hiddenPeg == null)
            {
                throw new Exception("Tried to unlink board peg from hidden peg that does not exist");
            }

            peg.UnlinkPeg(hiddenPeg);
        }

        // use UninitializeAndClear if it is possible for the instance to still be accessed afterwords, though it probably shouldn't be
        public void Uninitialize()
        {
            UninitializeHiddenPeg();
        }

        public void UninitializeAndClear()
        {
            UninitializeHiddenPeg();
            count = 0;
        }

        public void AddLink(Link peg)
        {
            if (MyServer.DEBUG) LConsole.WriteLine("linking peg with current count: {0}", count);

            if (count == 0)
            {
                InitializeHiddenPeg();
            }


            LinkPeg(peg);

            count++;
        }

        public void RemoveLink(Link peg)
        {
            if (count == 0)
            {
                throw new Exception("Tried to remove peg from HiddenPegData that is already empty");
            }

            if (MyServer.DEBUG) LConsole.WriteLine("unlinking peg with current count: {0}", count);

            UnlinkPeg(peg);

            if (count == 1)
            {
                UninitializeHiddenPeg();
            }

            count--;
        }

        public bool IsInitialized()
        {
            return hiddenPeg != null;
        }

        public bool IsEmpty()
        {
            return count == 0;
        }
    }

    private Dictionary<Link, int> LinkPositions = [];

    private Dictionary<int, HiddenPeg> HiddenPegs = [];

    public bool IsEmpty() => LinkPositions.IsEmpty();

    public bool HasLink(Link boardPegLink)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("checking if link at position {0} has link: {1}", boardPegLink.GetLinkingPosition(), LinkPositions.ContainsKey(boardPegLink));
        return LinkPositions.ContainsKey(boardPegLink);
    }

    // use UninitializeAndClear if it is possible for the instance to still be accessed afterwords, though it probably shouldn't be
    public void Uninitialize()
    {
        if (!IsEmpty())
        {
            throw new Exception("Tried to uninitialize package that is not empty");
        }

        foreach (var value in HiddenPegs)
        {
            var hiddenPegData = value.Value;
            if (hiddenPegData.IsInitialized())
            {
                hiddenPegData.Uninitialize();
            }
        }
    }

    public void UninitializeAndClear()
    {
        foreach (var value in HiddenPegs)
        {
            var hiddenPeg = value.Value;
            if (hiddenPeg.IsInitialized())
            {
                hiddenPeg.UninitializeAndClear();
            }
        }

        LinkPositions.Clear();
        HiddenPegs.Clear();
    }

    public void AddLink(Link boardPegLink)
    {
        CheckForPositionChanges();

        if (LinkPositions.ContainsKey(boardPegLink))
        {
            throw new Exception("Tried to add link to package that already contains it");
        }

        var position = boardPegLink.GetLinkingPosition();

        LinkAtPosition(boardPegLink, position);

        LinkPositions.Add(boardPegLink, position);
    }

    public void RemoveLink(Link boardPegLink)
    {
        CheckForPositionChanges();

        if (!LinkPositions.TryGetValue(boardPegLink, out var position))
        {
            throw new Exception("Tried to remove link from package that does not contain it");
        }

        UnlinkAtPosition(boardPegLink, position);

        LinkPositions.Remove(boardPegLink);
    }

    private void CheckForPositionChanges()
    {
        if (!HasPositionChanged())
        {
            return;
        }

        foreach (var pair in LinkPositions)
        {

            var boardPegLink = pair.Key;
            var position = pair.Value;

            UnlinkAtPosition(boardPegLink, position);

            var newPosition = boardPegLink.GetLinkingPosition();

            LinkAtPosition(boardPegLink, newPosition);

            LinkPositions[boardPegLink] = newPosition;
        }
    }

    private bool HasPositionChanged()
    {
        if (LinkPositions.Count == 0)
        {
            return false;
        }

        var firstPair = LinkPositions.First();
        return firstPair.Key.GetLinkingPosition() != firstPair.Value;
    }

    private void LinkAtPosition(Link boardPegLink, int position)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("started to link at position: {0}", position);

        if (!HiddenPegs.TryGetValue(position, out var hiddenPeg))
        {
            hiddenPeg = new HiddenPeg();
            HiddenPegs.Add(position, hiddenPeg);
        }

        hiddenPeg.AddLink(boardPegLink);
    }

    private void UnlinkAtPosition(Link boardPegLink, int position)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("started to unlink at position: {0}", position);

        if (!HiddenPegs.TryGetValue(position, out var value))
        {
            throw new Exception("Hidden peg could not be found in package at position " + position);
        }

        value.RemoveLink(boardPegLink);

        if (value.IsEmpty())
        {
            HiddenPegs.Remove(position);
        }
    }
}
