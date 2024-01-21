using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Unitive.Framework.Core;
using CirnoFramework.Interface.Commands;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class Service
    {
        public static void SetHttpServerAddr(string addr)
        {
            Program.Config.RemoteHttpConfig.HttpServerAddr = addr;
        }

        public static List<string> GetInstalledPackageInfo()
        {
            List<string> result = new List<string>();
            foreach (IPackageInfo info in Program.Factory.packageInfos)
            {
                result.Add(info.GetPackageInfo());
            }
            return result;
        }

        public static string ExecuteCommand(string command, DateTime start, DateTime end, string[] args, ref int errorCode)
        {
            return Program.Factory.ExecuteCommand(command, start, end, args, ref errorCode);
        }
    }
}
