
namespace AttikittySelectionTools.Client;

public abstract class Command
{
    public abstract Command Inverse { get; }

    public abstract void Trigger();

    public abstract bool Equals(Command other);

}