
using BoardPegs.Server;
using EccsLogicWorldAPI.Server;
using LICC;
using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BoardPegs.Logic.BoardPegHandling;

class LinkedRowWithLonelies : ILinkedRow
{
    private InputPeg _hiddenPeg;
    private int _count;
    private readonly List<InputPeg> _lonelyPegs = new(); //this could probably be an array

    public required int MaxLonelies { get; init; }

    // use UninitializeAndClear if it is possible for the instance to still be accessed afterwords, though it probably shouldn't be
    public void Uninitialize()
    {
        UninitializeHiddenPeg();
    }

    public void UninitializeAndClear()
    {
        UninitializeHiddenPeg();
        _count = 0;
    }

    public void AddPeg(IInputPeg peg)
    {
        if (MyServer.DEBUG) LConsole.WriteLine("is peg null? {0}", (InputPeg)peg == null);
        if (_count < MaxLonelies)
        {
            var index = _count;

            if (MyServer.DEBUG) LConsole.WriteLine("linking lonely peg with index {0}", _lonelyPegs.Count);

            if (MyServer.DEBUG) LConsole.WriteLine("test length before: {0}", _lonelyPegs.Count);
            _lonelyPegs.Add((InputPeg)peg);
            if (MyServer.DEBUG) LConsole.WriteLine("test length after: {0}", _lonelyPegs.Count);

            if (index > 0)
            {
                if (MyServer.DEBUG)
                {
                    LConsole.WriteLine("are the links null or equal?: {0}, {1}, {2}", 
                        _lonelyPegs[index].SecretLinks == null, 
                        _lonelyPegs[index - 1].SecretLinks == null, 
                        _lonelyPegs[index] == _lonelyPegs[index - 1]);
                }
                if (MyServer.DEBUG && _lonelyPegs[index].SecretLinks != null)
                {
                    LConsole.WriteLine("my secret links: {0}", _lonelyPegs[index].SecretLinks.Count);
                }
                if (MyServer.DEBUG && _lonelyPegs[index - 1].SecretLinks != null)
                {
                    LConsole.WriteLine("their secret links: {0}", _lonelyPegs[index - 1].SecretLinks.Count);
                }
                _lonelyPegs[index].AddSecretLinkWith(_lonelyPegs[index - 1]);
            }
        }
        else
        {
            if (_count == MaxLonelies)
            {
                InitializeHiddenPeg();

                foreach (var lonelyPeg in _lonelyPegs)
                {
                    lonelyPeg.RemoveAllSecretLinks();

                    lonelyPeg.AddSecretLinkWith(_hiddenPeg);
                }

                _lonelyPegs.Clear();
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

        if (MyServer.DEBUG) LConsole.WriteLine("is peg null? {0}", (InputPeg)peg == null);

        _count--;

        if (_count < MaxLonelies)
        {
            var index = _lonelyPegs.IndexOf((InputPeg)peg);

            if (MyServer.DEBUG) LConsole.WriteLine("unlinking lonely peg with index {0}", index);

            if (MyServer.DEBUG && ((InputPeg)peg).SecretLinks != null)
            {
                LConsole.WriteLine("my secret links before: {0}", ((InputPeg)peg).SecretLinks.Count);
            }

            if (index > 0)
            {
                _lonelyPegs[index].RemoveSecretLinkWith(_lonelyPegs[index - 1]);
            }

            if (index < _count)
            {
                _lonelyPegs[index].RemoveSecretLinkWith(_lonelyPegs[index + 1]);
            }

            if (index > 0 && index < _count)
            {
                _lonelyPegs[index - 1].AddSecretLinkWith(_lonelyPegs[index - 1]);
            }

            if (MyServer.DEBUG) LConsole.WriteLine("test length before: {0}", _lonelyPegs.Count);
            _lonelyPegs.Remove((InputPeg)peg);
            if (MyServer.DEBUG) LConsole.WriteLine("test length after: {0}", _lonelyPegs.Count);

            if (MyServer.DEBUG && ((InputPeg)peg).SecretLinks != null)
            {
                LConsole.WriteLine("my secret links after: {0}", ((InputPeg)peg).SecretLinks.Count);
            }
        }
        else
        {
            peg.RemoveSecretLinkWith(_hiddenPeg);

            if (_count == MaxLonelies)
            {
                var linkedPegs = _hiddenPeg.SecretLinks;

                _lonelyPegs.Add(linkedPegs[0]);

                for (int i = 1; i < MaxLonelies; i++)
                {
                    _lonelyPegs.Add(linkedPegs[i]);

                    _lonelyPegs[i].AddSecretLinkWith(_lonelyPegs[i - 1]);
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