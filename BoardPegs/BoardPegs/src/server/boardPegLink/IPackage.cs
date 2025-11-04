
namespace BoardPegs.Logic.BoardPegLink;

public interface IPackage
{
    public bool IsEmpty();
    public void Uninitialize();
    public void UninitializeAndClear();
    public bool HasLink(Link link);
    public void AddLink(Link link);
    public void RemoveLink(Link link);

    public bool TryAddLink(Link link)
    {
        if (!HasLink(link))
        {
            AddLink(link);
        }

        return false;
    }

    public bool TryRemoveLink(Link link)
    {
        if (HasLink(link))
        {
            RemoveLink(link);
        }

        return false;
    }
}