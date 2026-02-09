using FlexiServer.Core;
using FlexiServer.Core.Frame;
using FlexiServer.Core.Tick;
using FlexiServer.Transport.Interface;
using System.Net;
using System.Net.Sockets;

namespace FlexiServer.Transport.Udp
{
    public class UdpTransport(TickManager tickManager, FrameManager frameManager) : ITransport
    {
        private readonly TickManager tickManager = tickManager;
        private readonly FrameManager frameManager = frameManager;
        private Socket socket;
        private CancellationTokenSource? cts;
        private TickHandle? tickHandle;
        private int port;
        public void SetPort(int _port) => port = _port;
        public void OnConnectionStateChanged(Action<ClientConnectData, EPlayerConnectionState> onConnectionStateChanged)
        {

        }

        public void OnMessageReceived(Action<ClientConnectData, string, string> receivedCall)
        {

        }

        public void SendMessage(string clientId, string message)
        {

        }

        public void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            cts = new CancellationTokenSource();
        }

        public void Stop()
        {
            cts?.Cancel();
        }
        private void StartReceiveLoop()
        {
            byte[] buffer = new byte[2048];
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

            while (cts != null && cts.Token.IsCancellationRequested)
            {
                int len = socket.ReceiveFrom(buffer, ref remote);
                var client = (IPEndPoint)remote;
            }
        }
    }
}
