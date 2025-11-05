using BoardPegs.Server;
using EccsLogicWorldAPI.Server;
using LICC;
using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using System;
using System.Collections.Generic;

namespace BoardPegs.Logic.BoardPegHandling;

class LinkedRowWithLonelies : ILinkedRow
{
    private InputPeg _hiddenPeg;
    private int _count;
    private readonly List<InputPeg> _lonelyPegs = [];

    public required int MaxLonelies { get; init; }

    // use UninitializeAndClear if it is possible for the instance to still be accessed afterwords, though it probably shouldn't be
    public void Uninitialize()
    {
        UninitializeHiddenPeg();
    }

    public void UninitializeAndClear()
    {
        UninitializeHiddenPeg();

        for (int i = 1; i < _lonelyPegs.Count; i++)
        {
            _lonelyPegs[i].RemoveSecretLinkWith(_lonelyPegs[i - 1]);
        }

        _lonelyPegs.Clear();
        _count = 0;
    }

    public void AddPeg(IInputPeg peg)
    {
        if (_count < MaxLonelies)
        {
            var index = _count;

            _lonelyPegs.Add((InputPeg)peg);

            if (index > 0)
            {
                _lonelyPegs[index].AddSecretLinkWith(_lonelyPegs[index - 1]);
            }
        }
        else
        {
            if (_count == MaxLonelies)
            {
                InitializeHiddenPeg();

                if (MaxLonelies > 0)
                {
                    _lonelyPegs[0].AddSecretLinkWith(_hiddenPeg);

                    for (int i = 1; i < MaxLonelies; i++)
                    {
                        _lonelyPegs[i].RemoveSecretLinkWith(_lonelyPegs[i - 1]);

                        _lonelyPegs[i].AddSecretLinkWith(_hiddenPeg);
                    }

                    _lonelyPegs.Clear();
                }
            }

            peg.AddSecretLinkWith(_hiddenPeg);
        }

        _count++;
    }

    public void RemovePeg(IInputPeg peg)
    {
        if (_count == 0)
        {
            throw new Exception("Tried to remove peg from HiddenPegData that is already empty");
        }

        _count--;

        if (_count < MaxLonelies)
        {
            var index = _lonelyPegs.IndexOf((InputPeg)peg);

            if (index - 1 >= 0)
            {
                _lonelyPegs[index].RemoveSecretLinkWith(_lonelyPegs[index - 1]);
            }

            if (index + 1 < _lonelyPegs.Count)
            {
                _lonelyPegs[index].RemoveSecretLinkWith(_lonelyPegs[index + 1]);
            }

            if (index - 1 >= 0 && index + 1 < _lonelyPegs.Count)
            {
                _lonelyPegs[index - 1].AddSecretLinkWith(_lonelyPegs[index + 1]);
            }
            _lonelyPegs.Remove((InputPeg)peg);
        }
        else
        {
            peg.RemoveSecretLinkWith(_hiddenPeg);

            if (_count == MaxLonelies)
            {
                if (MaxLonelies > 0)
                {
                    var linkedPegs = _hiddenPeg.SecretLinks;

                    _lonelyPegs.Add(linkedPegs[0]);

                    for (int i = 1; i < MaxLonelies; i++)
                    {
                        _lonelyPegs.Add(linkedPegs[i]);

                        _lonelyPegs[i].AddSecretLinkWith(_lonelyPegs[i - 1]);
                    }
                }

                UninitializeHiddenPeg();
            }
        }
    }

    public bool IsInitialized()
    {
        return _hiddenPeg != null;
    }

    public bool IsEmpty()
    {
        return _count == 0;
    }

    private void InitializeHiddenPeg()
    {
        if (_hiddenPeg != null)
        {
            throw new Exception("Tried to initialize hidden peg that already is initialized");
        }

        if (MyServer.DEBUG) LConsole.WriteLine("borrowed peg");
        _hiddenPeg = VirtualInputPegPool.borrowPeg();
    }

    private void UninitializeHiddenPeg()
    {
        if (_hiddenPeg == null)
        {
            throw new Exception("Tried to uninitialize hidden peg that is not initialized");
        }


        if (MyServer.DEBUG) LConsole.WriteLine("returned peg");
        _hiddenPeg.RemoveAllSecretLinks();
        VirtualInputPegPool.returnPeg(_hiddenPeg);
        _hiddenPeg = null;
    }
}