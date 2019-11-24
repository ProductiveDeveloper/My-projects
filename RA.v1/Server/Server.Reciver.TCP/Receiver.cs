using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.Receiver.TCP
{
    public class Receiver
    {
        public delegate string OnMessage(string message);
        public event OnMessage OnMessageHandler;

        public string ip;
        public int port = 8080;

        public void Start()
        {
            if (ip == null)
            {
                String strHostName = string.Empty;
                strHostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                Console.WriteLine(addr[0].ToString());
                ip = addr[0].ToString();
            }

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(5);

            while (true)
            {
                var listener = tcpSocket.Accept();
                var buffer = new byte[256];
                var size = 0;
                var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer);
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (listener.Available > 0);

                listener.Send(Encoding.UTF8.GetBytes(OnMessageHandler(data.ToString())));

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
    }
}
