using JimmysUnityUtilities;
using LICC;
using LogicWorld.UI;
using System.Collections.Generic;

namespace AttikittySelectionTools.Client;

public class CommandManager
{
    public int MaxCount { get; init; }

    private enum CommandState
    {
        None,
        Undo,
        Redo
    }

    private readonly List<Command> _undostack = [];
    private readonly List<Command> _redostack = [];
    private CommandState _state = CommandState.None;


    public void AddCommand(Command command)
    {
        if (_state != CommandState.None)
        {
            return;
        }

        if (command == null)
        {
            return;
        }

        if (_undostack.IsNotEmpty() && _undostack[^1].Equals(command))
        {
            return;
        }

        if (MaxCount > 0 && _undostack.Count >= MaxCount)
        {
            _undostack.RemoveAt(0);
        }

        _undostack.Add(command);

        _redostack.Clear();

        if (MyClient.DEBUG) PrintStacks();
    }

    public bool CanUndo() => _undostack.Count > 0;
    public bool CanRedo() => _redostack.Count > 0;

    public void Undo()
    {
        var command = _undostack[^1];

        _undostack.RemoveAt(_undostack.Count - 1);
        _redostack.Add(command.Inverse);

        var previous_state = _state;
        _state = CommandState.Undo;

        command.Trigger();

        _state = previous_state;

        if (MyClient.DEBUG) PrintStacks();
    }

    public void Redo()
    {
        var command = _redostack[^1];

        _redostack.RemoveAt(_redostack.Count - 1);
        _undostack.Add(command.Inverse);

        var previous_state = _state;
        _state = CommandState.Undo;

        command.Trigger();

        _state = previous_state;

        if (MyClient.DEBUG) PrintStacks();
    }

    public bool IsEmpty()
    {
        return _undostack.Count == 0 && _redostack.Count == 0;
    }

    public void Clear()
    {
        _undostack.Clear();
        _redostack.Clear();
    }

    public void PrintStacks()
    {
        LConsole.WriteLine("Undo stack: ");
        foreach (var command in _undostack)
        {
            LConsole.WriteLine(command.ToString());
        }

        LConsole.WriteLine("Redo stack: ");
        foreach (var command in _redostack)
        {
            LConsole.WriteLine(command.ToString());
        }

        var selection = MultiSelector.GetCurrentSelection();
        int count = 0;
        if (selection != null) count = selection.Count;
        LConsole.WriteLine("Prev Selection Count: " + count);

        LConsole.WriteLine();
    }
}
