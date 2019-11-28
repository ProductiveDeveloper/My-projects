using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sender.TCP
{
    public class Sender
    {
        public delegate Task<string> OnMessage(string message);
        public event OnMessage OnMessageHandler;

        public string Ip { get; set; }
        public int Port { get; set; } = 8080;
        public string MyIp { get; set; }

        public Sender(string Ip)
        {
            this.Ip = Ip;
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            MyIp = addr[0].ToString();
        }
        
        private string Answer { get; set; } = "null";
        public void Run()
        {
            int error = 0;
            while (true)
            {
                try
                {
                    Thread.Sleep(5000);
                    var tcpEndPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);

                    var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    var data = Encoding.UTF8.GetBytes(MyIp);

                    tcpSocket.Connect(tcpEndPoint);

                    if (Answer == "null")
                        tcpSocket.Send(data);
                    else
                    {
                        tcpSocket.Send(Encoding.UTF8.GetBytes(Answer));
                        Answer = "null";
                    }

                    var buffer = new byte[256];
                    var size = 0;
                    var answer = new StringBuilder();

                    do
                    {
                        size = tcpSocket.Receive(buffer);
                        answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    while (tcpSocket.Available > 0);

                    Answer = OnMessageHandler(answer.ToString()).Result;

                    tcpSocket.Shutdown(SocketShutdown.Both);
                    tcpSocket.Close();                    
                }
                catch
                {
                    if (error == 10)
                        break;
                    error++;
                }
            }
        }
    }
}
