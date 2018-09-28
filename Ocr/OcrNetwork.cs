using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ocr
{
    class OcrNetwork
    {
        private static int port = 7053;
        private static WebServer webServer = new WebServer();
        private static ProxyServer proxyServer = new ProxyServer();
        private static Thread updateThread = null;

        private const string GetEnginesCommand = "get_engines";
        private const string RunOcrCommand = "run_engine";

        public static Dictionary<string, Dictionary<OcrEngineType, List<string>>> RemoteTypes = new Dictionary<string, Dictionary<OcrEngineType, List<string>>>();

        public static void Update()
        {
            StopUpdate();
            updateThread = new Thread(delegate()
            {
                UpdateInternal();
            });
            updateThread.Start();
        }

        public static void Close()
        {
            webServer.Stop();
            proxyServer.Stop();
        }

        private static void StopUpdate()
        {
            try
            {
                if (updateThread != null)
                {
                    updateThread.Abort();                    
                }
            }
            catch
            {
            }
            updateThread = null;
        }

        private static void UpdateInternal()
        {
            webServer.Stop();
            proxyServer.Stop();

            if (Program.Settings.ServerInfo.Proxy)
            {
                proxyServer.Start();
            }
            else if (Program.Settings.ServerInfo.Server)
            {
                webServer.Start();
            }
            UpdateRemoteEngines();
            Program.UpdateSettings();
        }

        private static void UpdateRemoteEngines()
        {
            try
            {
                RemoteOcr.ClearRemote();
                RemoteTypes.Clear();

                UpdateRemoteEngines(Program.Settings.ServerInfo.IP1);
                UpdateRemoteEngines(Program.Settings.ServerInfo.IP2);
            }
            catch
            {
            }
        }

        private static void UpdateRemoteEngines(string ip)
        {
            if(string.IsNullOrWhiteSpace(ip))
                return;

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string url = "http://" + ip + ":" + port + "/" + GetEnginesCommand;

                    webClient.Proxy = null;
                    webClient.Encoding = Encoding.UTF8;
                    string response = webClient.DownloadString(url);

                    string[] splitted = response.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string value in splitted)
                    {
                        try
                        {
                            string[] splitted2 = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            string typeStr = splitted2[0];
                            List<string> languages = new List<string>();
                            for (int i = 1; i < splitted2.Length; i++)
                            {
                                languages.Add(splitted2[i]);
                            }

                            OcrEngineType type = (OcrEngineType)Enum.Parse(typeof(OcrEngineType), typeStr);
                            if (!OcrEngine.Create(type).IsInstalled)
                            {
                                if (!RemoteTypes.ContainsKey(ip))
                                    RemoteTypes[ip] = new Dictionary<OcrEngineType, List<string>>();

                                RemoteTypes[ip][type] = languages.ToList();
                                RemoteOcr.SetRemote(type);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static OcrResult Load(OcrImage image, string language, OcrEngineType targetType)
        {
            try
            {
                string ip = null;
                foreach (KeyValuePair<string, Dictionary<OcrEngineType, List<string>>> remoteType in RemoteTypes)
                {
                    if (remoteType.Value.ContainsKey(targetType))
                    {
                        ip = remoteType.Key;
                        break;
                    }
                }
                if (string.IsNullOrWhiteSpace(ip))
                {
                    return OcrResult.Create(OcrResultType.NotInstalled);
                }

                using (WebClient webClient = new WebClient())
                {
                    string url = "http://" + ip + ":" + port + "/" + RunOcrCommand;

                    webClient.Proxy = null;
                    webClient.Encoding = Encoding.UTF8;
                    string response = webClient.UploadString(url, string.Format("type={0}&language={1}&data={2}",
                        targetType.ToString(), language, HttpUtility.UrlEncode(Convert.ToBase64String(image.ToBinary()))));

                    return OcrResult.FromBinary(Convert.FromBase64String(HttpUtility.UrlDecode(response)));
                }
            }
            catch(Exception e)
            {
                return OcrResult.Create(OcrResultType.Exception, e.ToString());
            }
        }

        class ProxyServer
        {
            private Thread thread;
            private TcpListener listener;
            private List<ProxySession> sessions = new List<ProxySession>();

            public void Start()
            {
                Stop();

                string ip = Program.Settings.ServerInfo.IP1;
                if (string.IsNullOrEmpty(ip))
                {
                    return;
                }

                thread = new Thread(delegate()
                    {
                        listener = new TcpListener(IPAddress.Any, port);
                        listener.Start();
                        while (listener != null)
                        {                            
                            TcpClient clientToProxy = null;
                            TcpClient proxyToServer = null;

                            try
                            {
                                clientToProxy = listener.AcceptTcpClient();
                                proxyToServer = new TcpClient();
                                proxyToServer.Connect(ip, port);

                                ProxySession session = new ProxySession(this, clientToProxy, proxyToServer);
                                AddSession(session);
                                session.BeginRecieve();
                            }
                            catch
                            {
                                if (clientToProxy != null)
                                {
                                    try
                                    {
                                        clientToProxy.Close();
                                        clientToProxy = null;
                                    }
                                    catch { }
                                }

                                if (proxyToServer != null)
                                {
                                    try
                                    {
                                        proxyToServer.Close();
                                        proxyToServer = null;
                                    }
                                    catch { }
                                }
                            }
                        }
                    });
                thread.Start();
            }

            public void Stop()
            {
                if (listener != null)
                {
                    try
                    {
                        listener.Stop();
                    }
                    catch
                    {
                    }
                    listener = null;
                }

                if (thread != null)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch
                    {
                        thread = null;
                    }
                }                

                lock (sessions)
                {
                    while (sessions.Count > 0)
                    {
                        ProxySession session = sessions[0];
                        session.Close();
                        sessions.Remove(session);
                    }

                    sessions.Clear();
                }
            }

            private void RemoveSession(ProxySession session)
            {
                lock (sessions)
                {
                    session.Close();
                    sessions.Remove(session);
                }
            }

            private void AddSession(ProxySession session)
            {
                lock (sessions)
                {
                    if (!sessions.Contains(session))
                    {
                        sessions.Add(session);
                    }
                }
            }

            class ProxySession
            {
                private ProxyServer proxyServer;
                private TcpClient clientToProxy;
                private TcpClient proxyToServer;
                private byte[] clientToProxyBuffer = new byte[1024];
                private byte[] proxyToServerBuffer = new byte[1024];

                public ProxySession(ProxyServer proxyServer, TcpClient clientToProxy, TcpClient proxyToServer)
                {
                    this.proxyServer = proxyServer;
                    this.clientToProxy = clientToProxy;
                    this.proxyToServer = proxyToServer;                    
                }

                public void BeginRecieve()
                {
                    try
                    {
                        BeginReceive(clientToProxy.Client);
                        BeginReceive(proxyToServer.Client);
                    }
                    catch
                    {
                        Close();
                    }
                }

                private void BeginReceive(Socket socket)
                {
                    try
                    {
                        byte[] buffer = GetBuffer(socket);
                        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, 
                            new AsyncCallback(OnReceive), socket);
                    }
                    catch
                    {
                        Close();
                    }
                }

                private void OnReceive(IAsyncResult ar)
                {
                    try
                    {
                        Socket socket = ar.AsyncState as Socket;
                        byte[] buffer = GetRecievedData(ar);

                        if (buffer.Length > 0)
                        {
                            RelayData(socket, buffer);
                            BeginReceive(socket);
                        }
                        else
                        {
                            Close();
                        }
                    }
                    catch
                    {
                        Close();
                    }
                }

                private byte[] GetRecievedData(IAsyncResult ar)
                {
                    Socket socket = ar.AsyncState as Socket;

                    int nBytesRec = 0;
                    if (socket != null)
                    {
                        try { nBytesRec = socket.EndReceive(ar); }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                    }
                    byte[] byReturn = new byte[nBytesRec];
                    Array.Copy(GetBuffer(socket), byReturn, nBytesRec);
                    return byReturn;
                }

                private byte[] GetBuffer(Socket socket)
                {
                    return socket == clientToProxy.Client ? clientToProxyBuffer : proxyToServerBuffer;
                }

                private void RelayData(Socket socket, byte[] buffer)
                {
                    if (socket == clientToProxy.Client)
                    {
                        proxyToServer.Client.Send(buffer);
                    }
                    else
                    {
                        clientToProxy.Client.Send(buffer);
                    }
                }

                public void Close()
                {
                    if (clientToProxy != null || proxyToServer != null)
                    {
                        try
                        {
                            clientToProxy.Close();                            
                        }
                        catch { }

                        try
                        {
                            proxyToServer.Close();                            
                        }
                        catch { }

                        clientToProxy = null;
                        proxyToServer = null;

                        proxyServer.RemoveSession(this);
                    }
                }
            }
        }

        class WebServer
        {
            private Thread thread;
            private HttpListener listener;

            public void Start()
            {
                Stop();

                thread = new Thread(delegate()
                {
                    listener = new HttpListener();
                    listener.Prefixes.Add("http://*:" + port + "/");
                    listener.Start();
                    while (listener != null)
                    {
                        try
                        {
                            HttpListenerContext context = listener.GetContext();
                            Process(context);
                        }
                        catch
                        {
                        }
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }

            public void Stop()
            {
                if (listener != null)
                {
                    try
                    {
                        listener.Stop();
                    }                    
                    catch
                    {
                    }
                    listener = null;
                }

                if (thread != null)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch
                    {
                    }
                    thread = null;
                }                
            }

            private void Process(HttpListenerContext context)
            {
                try
                {
                    string url = context.Request.Url.OriginalString;

                    string response = string.Empty;

                    if (url.Contains("favicon.ico"))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.OutputStream.Close();
                        return;
                    }

                    if (url.Contains(GetEnginesCommand))
                    {
                        if (Program.Engines != null)
                        {
                            Dictionary<OcrEngineType, OcrEngine> engines = new Dictionary<OcrEngineType, OcrEngine>(Program.Engines);
                            foreach (OcrEngine engine in engines.Values)
                            {
                                if (engine.IsInstalled)
                                {
                                    string languages = string.Empty;
                                    foreach (string language in engine.SupportedLanguages.Keys)
                                    {
                                        languages += language + ",";
                                    }
                                    languages = languages.TrimEnd(',');
                                    response += engine.Name + "," + languages + "|";
                                }
                            }
                            response = response.TrimEnd('|', ',');
                        }
                    }
                    else if (url.Contains(RunOcrCommand))
                    {
                        Dictionary<string, string> postData = GetPostData(context);
                        OcrEngineType type = (OcrEngineType)Enum.Parse(typeof(OcrEngineType), postData["type"]);
                        string language = postData["language"];
                        byte[] data = Convert.FromBase64String(postData["data"]);
                        OcrImage image = OcrImage.FromBinary(data);
                        OcrResult result = null;

                        try
                        {
                            result = OcrEngine.Create(type).Load(image, language);
                        }
                        catch(Exception e)
                        {
                            result = OcrResult.Create(OcrResultType.Exception, e.ToString());
                        }

                        try
                        {
                            response = HttpUtility.UrlEncode(Convert.ToBase64String(result.ToBinary()));
                        }
                        catch
                        {
                        }
                    }

                    byte[] resposeBuffer = Encoding.UTF8.GetBytes(response.ToString());

                    context.Response.ContentType = "text/html";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.ContentLength64 = resposeBuffer.Length;
                    context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    context.Response.AddHeader("Last-Modified", DateTime.Now.ToString("r"));
                    context.Response.OutputStream.Write(resposeBuffer, 0, resposeBuffer.Length);
                    context.Response.OutputStream.Flush();
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                catch
                {
                }

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.OutputStream.Close();
            }

            private Dictionary<string, string> GetPostData(HttpListenerContext context)
            {
                var request = context.Request;
                string text;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    text = reader.ReadToEnd();
                }

                Dictionary<string, string> postParams = new Dictionary<string, string>();
                string[] rawParams = text.Split('&');
                foreach (string param in rawParams)
                {
                    string[] kvPair = param.Split('=');
                    string key = kvPair[0];
                    string value = HttpUtility.UrlDecode(kvPair[1]);
                    postParams.Add(key, value);
                }

                return postParams;
            }
        }
    }
}
