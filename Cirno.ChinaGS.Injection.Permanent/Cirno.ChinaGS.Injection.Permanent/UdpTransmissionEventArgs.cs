using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using GS.Unitive.Framework.Core;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class UdpTransmissionEventArgs : EventArgs
    {
        public IAddonContext Context { get; set; }
        public byte[] Data { get; set; }
        public string Message { get; set; }
        public EndPoint RemoteEP { get; set; }
    }
}
