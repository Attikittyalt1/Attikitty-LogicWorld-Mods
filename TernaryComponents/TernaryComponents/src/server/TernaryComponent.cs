using LogicAPI.Server.Components;
using SkysCondensedCablingLib.Server;

namespace TernaryComponents.Server;

public abstract class TernaryComponent : LogicComponent, IHasSuperPegs
{
    public virtual int InputSuperSize(int index) => 2;
    public virtual int OutputSuperSize(int index) => 2;
}