using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using GS.Unitive.Framework.Core;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class UdpTransmissionManager : IDisposable
    {
        private UdpClient client;
        private IPEndPoint localEP;
        private int port;
        private bool listening;

        public UdpTransmissionManager(int localPort)
        {
            this.port = localPort;
            this.localEP = new IPEndPoint(IPAddress.Any, localPort);
            this.client = new UdpClient(this.localEP);
        }

        public bool Listening
        {
            get
            {
                return this.listening;
            }
            set
            {
                this.listening = value;
            }
        }

        private void Listen()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (this.listening)
            {
                try
                {
                    byte[] buffer = this.client.Receive(ref remoteEP);
                    string ret = string.Empty;

                    if (buffer.Length > 0)
                    {
                        string msg = Encoding.UTF8.GetString(buffer);
                        this.UdpTransmissionReceived(new UdpTransmissionEventArgs()
                        {
                            Context = Program.AddonContext,
                            Data = buffer,
                            Message = msg,
                            RemoteEP = remoteEP
                        });
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        public void StartListenAsync()
        {
            new Thread(Listen)
            {
                IsBackground = true
            }.Start();
            this.listening = true;
        }

        public void Send(string msg, IPEndPoint remoteEP, int remotePort)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            remoteEP.Port = remotePort;
            this.client.Send(buffer, buffer.Length, remoteEP);
        }

        protected virtual void UdpTransmissionReceived(UdpTransmissionEventArgs e)
        {
            UdpTransmissionReceivedEventHandler handler = this.OnUdpTransmissionReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Dispose()
        {
            this.listening = false;
            this.client.Close();
        }

        public event UdpTransmissionReceivedEventHandler OnUdpTransmissionReceived;
    }
    public delegate void UdpTransmissionReceivedEventHandler(object sender, UdpTransmissionEventArgs e);
}
