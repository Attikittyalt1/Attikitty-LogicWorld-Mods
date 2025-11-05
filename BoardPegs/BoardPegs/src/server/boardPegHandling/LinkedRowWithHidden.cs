using BoardPegs.Server;
using EccsLogicWorldAPI.Server;
using LICC;
using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using System;

namespace BoardPegs.Logic.BoardPegHandling;

class LinkedRowWithHidden : ILinkedRow
{
    private InputPeg _hiddenPeg;
    private int _count;

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
        if (_count == 0)
        {
            InitializeHiddenPeg();
        }


        peg.AddSecretLinkWith(_hiddenPeg);

        _count++;
    }

    public void RemovePeg(IInputPeg peg)
    {
        if (_count == 0)
        {
            throw new Exception("Tried to remove peg from HiddenPegData that is already empty");
        }

        peg.RemoveSecretLinkWith(_hiddenPeg);

        if (_count == 1)
        {
            UninitializeHiddenPeg();
        }

        _count--;
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