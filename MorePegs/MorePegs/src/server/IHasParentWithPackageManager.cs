using MorePegs.LogicCode.LinkableHandling;
using System.Collections.Generic;

namespace MorePegs.Server;

public interface IHasParentWithPackageManager
{
    public (List<PackageManager> x, List<PackageManager> y) GetActiveManagers();
}