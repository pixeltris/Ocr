using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ocr
{
    class ClipSyncServer
    {
        private const int port = 22983;

        private Thread thread;
        private TcpListener listener;
        private List<Session> sessions = new List<Session>();

        public event TextChangedHandler TextChanged;

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    if (TextChanged != null)
                        TextChanged(value);
                }                
            }
        }

        public ClipSyncServer()
        {
        }

        public void Start()
        {
            Stop();

            thread = new Thread(delegate()
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                while (true)
                {
                    TcpClient client = null;

                    try
                    {
                        client = listener.AcceptTcpClient();
                        Session session = new Session(this, client);
                        AddSession(session);
                        session.BeginRecieve();
                    }
                    catch
                    {
                        if (client != null)
                        {
                            try
                            {
                                client.Close();
                                client = null;
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
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }

            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }

            lock (sessions)
            {
                while (sessions.Count > 0)
                {
                    Session session = sessions[0];
                    session.Close();
                    sessions.Remove(session);
                }

                sessions.Clear();
            }
        }

        private void RemoveSession(Session session)
        {
            lock (sessions)
            {
                session.Close();
                sessions.Remove(session);
            }
        }

        private void AddSession(Session session)
        {
            lock (sessions)
            {
                if (!sessions.Contains(session))
                {
                    sessions.Add(session);
                }
            }
        }

        class Session
        {
            private ClipSyncServer server;
            private TcpClient client;
            private byte[] buffer = new byte[1024];
            private List<byte> data = new List<byte>();

            public Session(ClipSyncServer server, TcpClient client)
            {
                this.server = server;
                this.client = client;
            }

            public void BeginRecieve()
            {
                try
                {
                    BeginReceive(client.Client);
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
                        data.AddRange(buffer);
                        HandlePacket();
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
                return buffer;
            }

            public void Close()
            {
                if (client != null)
                {
                    try
                    {
                        client.Close();
                    }
                    catch { }

                    client = null;

                    server.RemoveSession(this);
                }
            }

            private void HandlePacket()
            {
                try
                {
                    bool available = true;
                    while (available)
                    {
                        available = false;
                        byte[] bytes = data.ToArray();
                        if (bytes.Length >= 2)
                        {
                            int dataLength = IPAddress.HostToNetworkOrder(BitConverter.ToInt16(bytes, 0));
                            if (data.Count >= dataLength + 2)
                            {
                                available = true;
                                byte[] packet = new byte[dataLength];
                                Buffer.BlockCopy(bytes, 2, packet, 0, packet.Length);
                                data.RemoveRange(0, dataLength + 2);

                                string text = Encoding.UTF8.GetString(packet);
                                if (IsCommand(text))
                                {
                                    string command = GetCommand(text);
                                    if (command != null)
                                    {
                                        HandleCommand(command);
                                    }
                                }
                                else if (IsMessage(text))
                                {
                                    string message = GetMessage(text);
                                    if (message != null)
                                    {
                                        HandleMessage(message);
                                    }
                                }
                                else
                                {
                                    // Unknown message type
                                    Close();
                                }
                            }
                        }

                        if (bytes.Length > short.MaxValue * 10)
                        {
                            // Large message, close the session
                            Close();
                        }
                    }
                }
                catch
                {
                }
            }

            private void HandleMessage(string message)
            {
                server.Text = message; 
            }

            private void HandleCommand(string command)
            {
                switch (command)
                {
                    case "quit":
                        Close();
                        break;
                    case "Can you add me to your clients?":
                        SendCommand("I can add you to my clients.");
                        break;
                }
            }

            public void SendMessage(string message)
            {
                Send(message, MessageHead, MessageTail);
            }

            public void SendCommand(string command)
            {
                Send(command, CommandHead, CommandTail);
            }

            private void Send(string text, string head, string tail)
            {
                string fullMessage = head + text + tail;
                byte[] bytes = Encoding.UTF8.GetBytes(fullMessage);
                try
                {
                    client.Client.Send(bytes);
                }
                catch
                {
                    Close();
                }
            }

            private bool IsMessage(string text)
            {
                return text.StartsWith(MessageHead);
            }

            private bool IsCommand(string text)
            {
                return text.StartsWith(CommandHead);
            }

            private string GetMessage(string text)
            {
                return GetText(text, MessageHead, MessageTail);
            }

            private string GetCommand(string text)
            {
                return GetText(text, CommandHead, CommandTail);
            }

            private string GetText(string text, string head, string tail)
            {
                int headIndex = text.IndexOf(head);
                int tailIndex = text.IndexOf(tail);
                if (headIndex == 0 && tailIndex > 0)
                {
                    return text.Substring(headIndex + head.Length, tailIndex - (headIndex + head.Length));
                }
                return null;
            }

            const string CommandHead = "$$898|@*[";
            const string CommandTail = "$$898|@*]";

            const string MessageHead = "$$898|@'(";
            const string MessageTail = "$$898|@')";
        }                
    }

    public delegate void TextChangedHandler(string text);
}
