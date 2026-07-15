namespace MorePegs.LogicCode.LinkableHandling;

public interface ILinkable
{
    public void Link(ILinkable linkable);
    public void Unlink(ILinkable linkable);
}