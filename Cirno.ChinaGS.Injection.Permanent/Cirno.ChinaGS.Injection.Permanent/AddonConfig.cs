using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS.Unitive.Framework.Core;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class AddonConfig
    {
        private bool _debugging;
        private HttpConfig _httpConfig;
        private int _remoteUdpPort;
        private int _localUdpPort;

        public bool Debugging
        {
            get
            {
                return this._debugging;
            }
        }

        public HttpConfig RemoteHttpConfig
        {
            get
            {
                return this._httpConfig;
            }
        }

        public int RemoteUdpPort
        {
            get
            {
                return this._remoteUdpPort;
            }
        }
        
        public int LocalUdpPort
        {
            get
            {
                return this._localUdpPort;
            }
        }

        public AddonConfig()
        {
            this._debugging = Utils.GetConfigOrDefaultValue("BaseConfig", "Debugging", false);
            this._remoteUdpPort = Utils.GetConfigOrDefaultValue("BaseConfig", "RemoteUdpPort", 19261);
            this._localUdpPort = Utils.GetConfigOrDefaultValue("BaseConfig", "LocalUdpPort", 19260);
            this._httpConfig = new HttpConfig();
        }

        public class HttpConfig
        {
            private string _httpServerAddr;
            private string _apiResultFeedback;
            private string _apiStaticFile;
            private int _timeout;

            public string HttpServerAddr
            {
                get
                {
                    return this._httpServerAddr;
                }
                set
                {
                    this._httpServerAddr = value;
                }
            }

            public int Timeout
            {
                get
                {
                    return this._timeout;
                }
            }

            public string ApiResultFeedback
            {
                get
                {
                    return this._apiResultFeedback;
                }
            }

            public string ApiStaticFile
            {
                get
                {
                    return this._apiStaticFile;
                }
            }

            public HttpConfig()
            {
                this._httpServerAddr = 
                    Utils.GetConfigOrDefaultValue("HttpConfig", "RemoteHttpServer", "http://127.0.0.1:5000");
                this._timeout = Utils.GetConfigOrDefaultValue("HttpConfig", "Timeout", 1000);
                this._apiStaticFile = Utils.GetConfigOrDefaultValue("HttpConfig", "Api-StaticFile", "static");
                this._apiResultFeedback = Utils.GetConfigOrDefaultValue("HttpConfig", "Api-ResultFeedback", "api/resultfeed");
            }
        }
    }
}
