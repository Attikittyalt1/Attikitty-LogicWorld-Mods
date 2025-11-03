using EccsLogicWorldAPI.Server;
using JimmysUnityUtilities;
using LICC;
using LogicAPI.Data;
using LogicWorld.Server.Circuitry;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BoardPegs.Logic;

public class BoardPegTracker<T>
    where T : IComparable<T>, IEquatable<T>
{
    const bool DEBUG = true;

    class BoardPegPackage
    {
        class HiddenPegData
        {
            private InputPeg hiddenPeg;
            private int count;

            private void InitializeHiddenPeg()
            {
                if (hiddenPeg != null)
                {
                    throw new Exception("Tried to initialize hidden peg that already is initialized");
                }

                if (DEBUG) LConsole.WriteLine("borrowed peg");
                hiddenPeg = VirtualInputPegPool.borrowPeg();
            }

            private void UninitializeHiddenPeg()
            {
                if (hiddenPeg == null)
                {
                    throw new Exception("Tried to uninitialize hidden peg that is not initialized");
                }


                if (DEBUG) LConsole.WriteLine("returned peg");
                hiddenPeg.RemoveAllSecretLinks();
                VirtualInputPegPool.returnPeg(hiddenPeg);
                hiddenPeg = null;
            }

            private void LinkPeg(IBoardPeg<T> peg)
            {
                if (hiddenPeg == null)
                {
                    throw new Exception("Tried to link board peg to hidden peg that does not exist");
                }

                peg.LinkPeg(hiddenPeg);
            }

            private void UnlinkPeg(IBoardPeg<T> peg)
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

            public void AddPeg(IBoardPeg<T> peg)
            {
                if (DEBUG) LConsole.WriteLine("linking peg with current count: {0}", count);

                if (count == 0)
                {
                    InitializeHiddenPeg();
                }


                LinkPeg(peg);

                count++;
            }

            public void RemovePeg(IBoardPeg<T> peg)
            {
                if (count == 0)
                {
                    throw new Exception("Tried to remove peg from HiddenPegData that is already empty");
                }

                if (DEBUG) LConsole.WriteLine("unlinking peg with current count: {0}", count);

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

        struct PegPositionData
        {
            public Vector2Int position;
            public bool horizontal;
            public bool vertical;
        }

        private Dictionary<IBoardPeg<T>, PegPositionData> BoardPegPositions = [];

        private Dictionary<int, HiddenPegData> HorizontalHiddenPegs = [];

        private Dictionary<int, HiddenPegData> VerticalHiddenPegs = [];

        public bool IsEmpty() => BoardPegPositions.IsEmpty();

        // use UninitializeAndClear if it is possible for the instance to still be accessed afterwords, though it probably shouldn't be
        public void Uninitialize()
        {
            if (!IsEmpty())
            {
                throw new Exception("Tried to uninitialize BoardPegPackage that is not empty");
            }

            foreach (var value in HorizontalHiddenPegs)
            {
                var hiddenPegData = value.Value;
                if (hiddenPegData.IsInitialized())
                {
                    hiddenPegData.Uninitialize();
                }
            }

            foreach (var value in VerticalHiddenPegs)
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
            foreach (var value in HorizontalHiddenPegs)
            {
                var hiddenPegData = value.Value;
                if (hiddenPegData.IsInitialized())
                {
                    hiddenPegData.UninitializeAndClear();
                }
            }

            foreach (var value in VerticalHiddenPegs)
            {
                var hiddenPegData = value.Value;
                if (hiddenPegData.IsInitialized())
                {
                    hiddenPegData.UninitializeAndClear();
                }
            }

            BoardPegPositions.Clear();
            HorizontalHiddenPegs.Clear();
            VerticalHiddenPegs.Clear();
        }

        public void AddPeg(IBoardPeg<T> boardPeg)
        {
            CheckForPositionChanges();

            if (BoardPegPositions.ContainsKey(boardPeg))
            {
                throw new Exception("Tried to add peg to BoardPegPackage that already contains it");
            }

            var data = GetBoardPegData(boardPeg);

            AddPegPosition(boardPeg, data);

            BoardPegPositions.Add(boardPeg, data);
        }

        public void RemovePeg(IBoardPeg<T> boardPeg)
        {
            CheckForPositionChanges();

            if (!BoardPegPositions.TryGetValue(boardPeg, out var data))
            {
                throw new Exception("Tried to remove peg from BoardPegPackage that does not contain it");
            }

            RemovePegPosition(boardPeg, data);

            BoardPegPositions.Remove(boardPeg);
        }

        private void CheckForPositionChanges()
        {
            if (!HasPositionChanged())
            {
                return;
            }

            foreach (var pair in BoardPegPositions)
            {

                var boardPeg = pair.Key;
                var data = pair.Value;

                RemovePegPosition(boardPeg, data);

                var newData = GetBoardPegData(boardPeg);

                AddPegPosition(boardPeg, newData);

                BoardPegPositions[boardPeg] = newData;
            }
        }

        private bool HasPositionChanged()
        {
            if (BoardPegPositions.Count == 0)
            {
                return false;
            }

            var firstPair = BoardPegPositions.First();
            return firstPair.Key.GetLinkingPosition() != firstPair.Value.position;
        }

        private void AddPegPosition(IBoardPeg<T> boardPeg, PegPositionData data)
        {
            if (DEBUG) LConsole.WriteLine("started to add peg at position: {0}, {1}", data.position.x, data.position.y);

            if (data.horizontal)
            {
                if (!HorizontalHiddenPegs.TryGetValue(data.position.x, out var value))
                {
                    value = new HiddenPegData();
                    HorizontalHiddenPegs.Add(data.position.x, value);
                }

                value.AddPeg(boardPeg);
            }

            if (data.vertical)
            {

                if (!VerticalHiddenPegs.TryGetValue(data.position.y, out var value))
                {
                    value = new HiddenPegData();
                    VerticalHiddenPegs.Add(data.position.y, value);
                }

                value.AddPeg(boardPeg);
            }
        }

        private void RemovePegPosition(IBoardPeg<T> boardPeg, PegPositionData data)
        {
            if (DEBUG) LConsole.WriteLine("started to remove peg at position: {0}, {1}", data.position.x, data.position.y);

            if (data.horizontal)
            {
                if (!HorizontalHiddenPegs.TryGetValue(data.position.x, out var value))
                {
                    throw new Exception("HiddenPegData could not be found in BoardPegPackage at horizontal position " + data.position.x);
                }

                value.RemovePeg(boardPeg);

                if (value.IsEmpty())
                {
                    HorizontalHiddenPegs.Remove(data.position.x);
                }
            }

            if (data.vertical)
            {
                if (!VerticalHiddenPegs.TryGetValue(data.position.y, out var value))
                {
                    throw new Exception("HiddenPegData could not be found in BoardPegPackage at vertical position " + data.position.y);
                }

                value.RemovePeg(boardPeg);

                if (value.IsEmpty())
                {
                    VerticalHiddenPegs.Remove(data.position.y);
                }
            }
        }

        private PegPositionData GetBoardPegData(IBoardPeg<T> boardPeg)
        {
            return new PegPositionData
            {
                position = boardPeg.GetLinkingPosition(),
                horizontal = boardPeg.ShouldBeLinkedHorizontally(),
                vertical = boardPeg.ShouldBeLinkedVertically()
            };
        }

    }

    private readonly Dictionary<T, BoardPegPackage> BoardPegPackageByAddress = [];

    public void StartTrackingBoardPeg(IBoardPeg<T> boardPeg)
    {
        if (boardPeg.IsTracked)
        {
            throw new Exception("Tried to start tracking BoardPeg that's already being tracked");
        }

        var address = boardPeg.GenerateTrackerAddress();

        if (!BoardPegPackageByAddress.TryGetValue(address, out var package))
        {
            package = new BoardPegPackage();
            BoardPegPackageByAddress.Add(address, package);
        }

        package.AddPeg(boardPeg);
        boardPeg.AssignedTrackerKey = address;
        boardPeg.IsTracked = true;
    }

    public void StopTrackingBoardPeg(IBoardPeg<T> boardPeg)
    {
        if (!boardPeg.IsTracked)
        {
            throw new Exception("Tried to stop tracking BoardPeg that wasn't being tracked");
        }

        var address = boardPeg.AssignedTrackerKey;
        var package = BoardPegPackageByAddress[address];

        package.RemovePeg(boardPeg);
        boardPeg.AssignedTrackerKey = default;
        boardPeg.IsTracked = false;

        if (package.IsEmpty())
        {
            package.Uninitialize();
            BoardPegPackageByAddress.Remove(address);
        }
    }
}