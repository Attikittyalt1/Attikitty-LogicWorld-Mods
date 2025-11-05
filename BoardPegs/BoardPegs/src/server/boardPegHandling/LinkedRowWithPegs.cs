using JimmysUnityUtilities;
using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using System;
using System.Collections.Generic;

namespace BoardPegs.Logic.BoardPegHandling;

class LinkedRowWithPegs : ILinkedRow
{
    private readonly List<InputPeg> _pegs = [];

    public void Uninitialize()
    {
        
    }

    public void UninitializeAndClear()
    {
        for (int i = 1; i < _pegs.Count; i++)
        {
            _pegs[i].RemoveSecretLinkWith(_pegs[i - 1]);
        }

        _pegs.Clear();
    }

    public void AddPeg(IInputPeg peg)
    {
        var index = _pegs.Count;

        if (index - 1 >= 0)
        {
            peg.AddSecretLinkWith(_pegs[index - 1]);
        }

        _pegs.Add((InputPeg)peg);
    }

    public void RemovePeg(IInputPeg peg)
    {
        if (_pegs.IsEmpty())
        {
            throw new Exception("Tried to remove peg from HiddenPegData that is already empty");
        }

        var index = _pegs.IndexOf((InputPeg)peg);

        if (index - 1 >= 0)
        {
            peg.RemoveSecretLinkWith(_pegs[index - 1]);
        }

        if (index + 1 < _pegs.Count)
        {
            peg.RemoveSecretLinkWith(_pegs[index + 1]);
        }

        if (index - 1 >= 0 && index + 1 < _pegs.Count)
        {
            _pegs[index - 1].AddSecretLinkWith(_pegs[index + 1]);
        }

        _pegs.Remove((InputPeg)peg);
    }

    public bool IsInitialized()
    {
        return true;
    }

    public bool IsEmpty()
    {
        return _pegs.IsEmpty();
    }
}