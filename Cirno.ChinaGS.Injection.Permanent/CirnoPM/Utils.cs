using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Unitive.Framework.Core;
using System.Net;

namespace CirnoPM
{
    public class Utils
    {
        public static string DownloadString(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        public static void DownloadFile(string url, string savefilename)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFileAsync(new Uri(url), savefilename);
            }
        }

        public static string ListInstalledPackageInfo(IAddonContext context)
        {
            dynamic service = context.GetFirstOrDefaultService("Cirno.ChinaGS.Injection.Permanent", 
               "Cirno.ChinaGS.Injection.Permanent.Service");
            List<string> list = service.GetInstalledPackageInfo();
            return string.Join(";", list);
        }

        public static PackageInfo ParsePackageName(string package)
        {
            if (package.Contains("="))
            {
                return new PackageInfo
                {
                    PackageName = package.Split('=')[0].Trim(),
                    PackageVersion = package.Split('=')[1].Trim()
                };
            }
            else
            {
                return new PackageInfo
                {
                    PackageName = package.Trim(),
                    PackageVersion = "default"
                };
            }
        }

        public static string GetRemotePackageFilename(PackageInfo package)
        {
            if (package.PackageVersion.ToLower() == "default")
            {
                return package.PackageName + ".dll.cpm";
            }
            else
            {
                return package.PackageName + "_" + package.PackageVersion + ".dll.cpm";
            }
        }

        public static List<PackageInfo> ParseManifest(string manifest)
        {
            List<PackageInfo> result = new List<PackageInfo>();
            foreach (string i in manifest.Split(';'))
            {
                string item = i.Trim();
                if (item.Contains("="))
                {
                    result.Add(new PackageInfo
                    {
                        PackageName = item.Split('=')[0].Trim(),
                        PackageVersion = item.Split('=')[1].Trim()
                    });
                }
                else
                {
                    result.Add(new PackageInfo
                    {
                        PackageName = item.Trim(),
                        PackageVersion = "default"
                    });
                }
            }
            return result;
        }
    }
}
