using LogicAPI.Server.Components;

namespace BoardPegs.Logic.BoardPegHandling;

public interface ILinkedRow
{
    public bool IsEmpty();

    public bool IsInitialized();

    // use UninitializeAndClear if it is possible for the instance to still be accessed afterwords, though it probably shouldn't be
    public void Uninitialize();

    public void UninitializeAndClear();

    public void AddPeg(IInputPeg peg);

    public void RemovePeg(IInputPeg peg);
}