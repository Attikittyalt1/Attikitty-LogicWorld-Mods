using JimmysUnityUtilities;
using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using System;
using System.Collections.Generic;

namespace MorePegs.LogicCode.LinkableHandling;

class LinkedRow
{
    private readonly List<ILinkable> _linkables = [];

    public void Uninitialize()
    {
        
    }

    public void UninitializeAndClear()
    {
        for (int i = 1; i < _linkables.Count; i++)
        {
            _linkables[i].Unlink(_linkables[i - 1]);
        }

        _linkables.Clear();
    }

    public void AddLinkable(ILinkable linkable)
    {
        var index = _linkables.Count;

        if (index - 1 >= 0)
        {
            linkable.Link(_linkables[index - 1]);
        }

        _linkables.Add(linkable);
    }

    public void RemoveLinkable(ILinkable linkable)
    {
        if (_linkables.IsEmpty())
        {
            throw new Exception("Tried to remove peg from HiddenPegData that is already empty");
        }

        var index = _linkables.IndexOf(linkable);

        if (index - 1 >= 0)
        {
            linkable.Unlink(_linkables[index - 1]);
        }

        if (index + 1 < _linkables.Count)
        {
            linkable.Unlink(_linkables[index + 1]);
        }

        if (index - 1 >= 0 && index + 1 < _linkables.Count)
        {
            _linkables[index - 1].Link(_linkables[index + 1]);
        }

        _linkables.Remove(linkable);
    }

    public bool IsInitialized()
    {
        return true;
    }

    public bool IsEmpty()
    {
        return _linkables.IsEmpty();
    }
}