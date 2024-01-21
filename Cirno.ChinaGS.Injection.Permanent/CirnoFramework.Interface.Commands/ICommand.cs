using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Unitive.Framework.Core;

namespace CirnoFramework.Interface.Commands
{
    public interface ICommand
    {
        string Execute(IAddonContext context, DateTime start, DateTime stop, string[] args);
    }
}
