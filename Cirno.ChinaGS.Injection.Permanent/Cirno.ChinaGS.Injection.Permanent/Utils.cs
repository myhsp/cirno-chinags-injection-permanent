using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Unitive.Framework.Core;
using System.Windows.Controls;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class Utils
    {
        public static T GetConfigOrDefaultValue<T>(string dictname, string keyname, T defaultValue)
        {
            try
            {
                string conf = Program.AddonContext.DictionaryValue(dictname, keyname);
                if (!string.IsNullOrEmpty(conf))
                {
                    if (typeof(T) == typeof(bool))
                    {
                        return (T)(object)bool.Parse(conf);
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        return (T)(object)int.Parse(conf);
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        return (T)(object)double.Parse(conf);
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return (T)(object)conf;
                    }
                    else
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    return defaultValue;
                }
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetMachineCurrentStatus()
        {
            ///<summary>
            /// 获得机器当前状态
            ///<returns></returns>
            ///</summary>
            dynamic logicService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.SmartBoard.Logic",
                "GS.Terminal.SmartBoard.Logic.Core.Service");
            string status = logicService.GetState();

            return status;
        }

        public static string GetTerminalCode()
        {
            return Program.AddonContext.IntercativeData("TerminalCode");
        }

        public static string GetMachineWebPath()
        {
            /// <summary>
            /// 获得 webpath（不知道是什么）
            /// </summary>
            IAddonContext logicContext = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic").Context;

            bool success = false;
            string webPath = logicContext.GlobalSetting("WebPath", ref success);
            if (!success)
            {
                Program.AddonContext.Logger.Debug("[CirnoInjection] Method Utils.GetMachineWebPath can't fetch attribute WebPath through GlobalSetting - returned empty string.");
            }
            else
            {
                Program.AddonContext.Logger.Debug("[CirnoInjection] Method Utils.GetMachineWebPath is called and fetched attribute WebPath! Fuck GS!!");
            }

            return webPath;
        }

        public static string GetMachineMacAddr()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        public static string GetMachineID()
        {
            ///<summary>
            /// 获得当前机器 ID
            ///<returns></returns>
            ///</summary>
            IAddonContext logicContext = AddonRuntime.Instance.GetInstalledAddons()
                .FirstOrDefault((IAddon ss) => ss.SymbolicName == "GS.Terminal.SmartBoard.Logic").Context;

            bool success = false;
            string machineId = logicContext.GlobalSetting("tCode", ref success);
            if (!success)
            {
                Program.AddonContext.Logger.Debug("[CirnoInjection] Method Utils.GetMachineID can't fetch attribute tCode through GlobalSetting - returned empty string.");
            }
            else
            {
                Program.AddonContext.Logger.Debug("[CirnoInjection] Method Utils.GetMachineID is called and fetched attribute tCode! Fuck GS!!");
            }

            return machineId;
        }

        public static string AddGarnitureControl(UserControl control, double left, double top)
        {
            ///<summary>
            /// 添加用户控件
            ///<returns></returns>
            ///</summary>
            dynamic uiService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.MainShell",
                "GS.Terminal.MainShell.Services.UIService");

            string guid = uiService.AddGarnitureControl(control, top, left);
            return guid;
        }

        public static void CreateTimelineTask(DateTime StartTime, DateTime EndTime,
            int Lvl, bool AllowParallel,
            Action<string, string> OnStart, Action<string, string> OnPause,
            Action<string, string> OnRestart, Action<string, string> OnStop,
            Action<string, string> OnTaskStateChanged,
            Action<string, string> OnTaskCreated,
            string taskname)
        {
            ///<summary>
            /// 创建时间线任务
            ///<returns></returns>
            ///</summary>
            dynamic timelineService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateTimeLineTask(StartTime, EndTime, Lvl, AllowParallel, OnStart, OnPause, OnRestart, OnStop, OnTaskStateChanged, OnTaskCreated, taskname); ;
        }

        public static void CreateTimelineTask(DateTime StartTime, DateTime EndTime, int Lvl, Action<string, string> OnStart, Action<string, string> OnStop, string taskname)
        {
            ///<summary>
            /// 创建时间线任务
            ///<returns></returns>
            ///</summary>
            CreateTimelineTask(StartTime, EndTime, Lvl, true, OnStart, null, null, OnStop, null, null, taskname);
        }

        public static void CreateTask(DateTime ExecuteTime, Action<string> TaskAction, AsyncCallback Callback, string taskname)
        {
            ///<summary>
            /// 创建定时任务
            ///<returns></returns>
            ///</summary>
            dynamic timelineService = Program.AddonContext.GetFirstOrDefaultService("GS.Terminal.TimeLine", "GS.Terminal.TimeLine.Service");
            timelineService.CreateTask(ExecuteTime, TaskAction, Callback, taskname);
        }

        public static void ShowPopup(string msg)
        {
            dynamic uiAddon = Program.AddonContext.
                GetFirstOrDefaultService("GS.Terminal.MainShell", "GS.Terminal.MainShell.Services.UIService");

            uiAddon.ShowPrompt(msg, 3);
        }

        public static RemoteCommandGeneric ParseCommand(UdpTransmissionEventArgs e, ref CommandProtocolVersion protocolVersion)
        {
            try
            {
                if (e.Message.Contains("command_protocol_version"))
                {
                    try
                    {
                        RemoteCommandv2 commandv2 = JsonConvert.DeserializeObject<RemoteCommandv2>(e.Message);
                        RemoteCommandGeneric generic = new RemoteCommandGeneric
                        {
                            args = commandv2.args,
                            start_time = commandv2.start_time,
                            end_time = commandv2.end_time,
                            command_name = commandv2.command_name,
                            command_protocol_version = commandv2.command_protocol_version
                        };
                        protocolVersion = CommandProtocolVersion.Cirno_RCMD_v2;
                        return generic;
                    }
                    catch (Exception)
                    {
                        protocolVersion = CommandProtocolVersion.Unknown;
                        return null;
                    }
                }
                else
                {
                    RemoteCommandv1 commandv1 = JsonConvert.DeserializeObject<RemoteCommandv1>(e.Message);
                    protocolVersion = CommandProtocolVersion.Cirno_RCMD_v1;
                    return new RemoteCommandGeneric
                    {
                        args = commandv1.args.Split(';').ToList(),
                        start_time = commandv1.start_time,
                        end_time = commandv1.end_time,
                        command_name = commandv1.command_name,
                        command_protocol_version = "cirno-rcmd-v1"
                    };
                }
            }
            catch
            {
                protocolVersion = CommandProtocolVersion.Unknown;
                return null;
            }
        }
    }
}
