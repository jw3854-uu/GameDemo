using System.Net;
using System.Net.Sockets;

namespace FlexiServer.Transport.Udp
{
    public static class UdpEndpoints
    {
        public static void StartUdpListen(this WebApplication app,int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            CancellationTokenSource cts = new CancellationTokenSource();

            UdpTransport?transport = app.Services.GetService<UdpTransport>();
            if (transport == null) { socket.Dispose(); return; }
            transport.SetSocket(socket,cts);
            
            byte[] buffer = new byte[2048];
            EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

            while (cts != null && cts.Token.IsCancellationRequested && socket != null)
            {
                int len = socket.ReceiveFrom(buffer, ref remote);
                var clientRemote = (IPEndPoint)remote;
                transport.ReceiveFromRemote(buffer, len, clientRemote);
            }
        }
        
    }
}
