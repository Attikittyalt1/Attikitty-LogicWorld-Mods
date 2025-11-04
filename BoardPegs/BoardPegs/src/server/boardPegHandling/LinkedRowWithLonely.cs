
using BoardPegs.Server;
using EccsLogicWorldAPI.Server;
using LICC;
using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using System;
using System.Linq;

namespace BoardPegs.Logic.BoardPegHandling;

class LinkedRowWithLonely : ILinkedRow
{
    private InputPeg _hiddenPeg;
    private InputPeg _lonelyPeg;
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
        if (MyServer.DEBUG) LConsole.WriteLine("linking peg with current count: {0}", _count);

        if (_count == 0)
        {
            _lonelyPeg = (InputPeg) peg;
        } else
        {
            if (_count == 1)
            {
                InitializeHiddenPeg();

                Link(_lonelyPeg);
                _lonelyPeg = null;
            }

            Link(peg);
        }

        _count++;
    }

    public void RemovePeg(IInputPeg peg)
    {
        if (_count == 0)
        {
            throw new Exception("Tried to remove peg from HiddenPegData that is already empty");
        }

        if (MyServer.DEBUG) LConsole.WriteLine("unlinking peg with current count: {0}", _count);

        if (_count == 1)
        {
            _lonelyPeg = null;
        }
        else
        {
            Unlink(peg);

            if (_count == 2)
            {
                _lonelyPeg = _hiddenPeg.SecretLinks.First();
                Unlink(_lonelyPeg);

                UninitializeHiddenPeg();
            }
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

    private void Link(IInputPeg peg)
    {
        if (_hiddenPeg == null)
        {
            throw new Exception("Tried to link board peg to hidden peg that does not exist");
        }

        peg.AddSecretLinkWith(_hiddenPeg);
    }

    private void Unlink(IInputPeg peg)
    {
        if (_hiddenPeg == null)
        {
            throw new Exception("Tried to unlink board peg from hidden peg that does not exist");
        }

        peg.RemoveSecretLinkWith(_hiddenPeg);
    }
}