
namespace BoardPegs.Logic.BoardPegHandling;

public interface IRowPackage
{
    public bool IsEmpty();
    public void Uninitialize();
    public void UninitializeAndClear();
    public bool HasLinkable(Linkable linkable);
    public void AddLinkable(Linkable linkable);
    public void RemoveLinkable(Linkable linkable);

    public bool TryAddLinkable(Linkable linkable)
    {
        if (!HasLinkable(linkable))
        {
            AddLinkable(linkable);
        }

        return false;
    }

    public bool TryRemoveLinkable(Linkable linkable)
    {
        if (HasLinkable(linkable))
        {
            RemoveLinkable(linkable);
        }

        return false;
    }
}