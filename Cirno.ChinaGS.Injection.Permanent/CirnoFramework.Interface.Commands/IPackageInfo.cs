using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirnoFramework.Interface.Commands
{
    public interface IPackageInfo
    {
        string PackageName { get; set; }
        string PackageVersion { get; set; }

        string GetPackageInfo();
    }
}
