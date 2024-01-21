using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using GS.Unitive.Framework.Core;

namespace CirnoBuiltins
{
    public class Utils
    {
        public static void ShadowLantern(IAddonContext context, string msg, DateTime start, DateTime end)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").ShadowLantern(msg, start, end);
        }

        public static void ShadowLanternEx(IAddonContext context, string msg, double min)
        {
            ShadowLantern(context, msg, DateTime.Now, DateTime.Now.AddMinutes(min));
        }

        public static void ShadowLanternLTP(IAddonContext context, string msg, DateTime start, DateTime end)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").ShadowLanternLTP(msg, start, end);
        }

        public static void ResetWebPath(IAddonContext context, string webpath)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").ResetWebPath(webpath);
        }

        public static void ShowPrompt(IAddonContext context, string msg)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").ShowPrompt(msg);
        }

        public static string GetScreenCapture(IAddonContext context, string filename)
        {
            return context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").GetScreenCapture(filename);
        }

        public static Guid AddPosterTemplate(IAddonContext context, string imageUri)
        {
            return context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").AddPosterTemplate(imageUri);
        }

        public static void RemovePosterTemplate(IAddonContext context, Guid guid)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").RemovePosterTemplate(guid);
        }

        public static void AddMultiMediaVisualTemplate(IAddonContext context, string media_json_filename)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").AddMultiMediaVisualTemplate(media_json_filename);
        }

        public static void RemoveVisualTemplate(IAddonContext context, string template_name, bool ignore_first = true)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").RemoveVisualTemplate(template_name, ignore_first);
        }

        public static string GetCachePath(IAddonContext context)
        {
            return context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").GetCachePath();
        }

        public static void ClearAllPosterTemplate(IAddonContext context)
        {
            context.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic", "GS.Terminal.SmartBoard.Logic.Core.InjectionEntranceService").ClearAllPosterTemplate();
        }

        public static void WriteJson(IAddonContext context, string filename, string json_b64)
        {
            string cache = GetCachePath(context);
            if (!filename.EndsWith(".json"))
            {
                filename = filename + ".json";
            }
            if (!Directory.Exists(Path.Combine(cache, "BlockCache")))
            {
                Directory.CreateDirectory(Path.Combine(cache, "BlockCache"));
            }
            byte[] byteB64 = Convert.FromBase64String(json_b64);
            string content = Encoding.UTF8.GetString(byteB64);

            File.WriteAllText(Path.Combine(cache, "BlockCache", filename), content, Encoding.UTF8);
        }

        public static void DownloadFile(IAddonContext context, string url, string savename)
        {
            string cache = GetCachePath(context);
            string savepath = cache;

            if (savename.EndsWith(".png") || savename.EndsWith(".jpg") || savename.EndsWith(".bmp"))
            {
                savepath = Path.Combine(cache, "image", savename);
            }
            else if (savename.EndsWith(".flv") || savename.EndsWith(".mp4"))
            {
                savepath = Path.Combine(cache, "video", savename);
            }
            else
            {
                savepath = Path.Combine(savepath, "utils");
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                savepath = Path.Combine(savepath, savename);
            }

            using (WebClient web = new WebClient())
            {
                try
                {
                    web.DownloadFileAsync(new Uri(url), savepath);
                }
                catch (Exception)
                {
                }
            }

        }
    }
}
