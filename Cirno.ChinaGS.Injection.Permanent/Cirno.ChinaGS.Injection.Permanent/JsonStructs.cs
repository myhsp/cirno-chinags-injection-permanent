using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public interface IRemoteCommandGeneric
    {
        string command_protocol_version { get; set; }
        string command_name { get; set; }
        string start_time { get; set; }
        string end_time { get; set; }
        List<string> args { get; set; }
    }
    
    public class RemoteCommandGeneric : IRemoteCommandGeneric
    {
        public string command_protocol_version { get; set; }
        public string command_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public List<string> args { get; set; }
    }

    public class RemoteCommandv1
    {
        public string command_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string args { get; set; }
    }

    public class RemoteCommandv2 : IRemoteCommandGeneric
    {
        public string command_protocol_version { get; set; }
        public string command_name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public List<string> args { get; set; }
    }

    public interface IResult
    {
        string machine_id { get; set; }
        string machine_ip { get; set; }
        string msg { get; set; }
        int errcode { get; set; }
        string time { get; set; }
    }

    public class Result : IResult
    {
        public string machine_id { get; set; }
        public string machine_ip { get; set; }
        public string msg { get; set; }
        public int errcode { get; set; }
        public string time { get; set; }
    }

    public enum CommandProtocolVersion
    {
        Unknown,
        Cirno_RCMD_v1,
        Cirno_RCMD_v2
    }
}
