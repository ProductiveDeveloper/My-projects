using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Receiver.TCP.Receiver receiver = new Receiver.TCP.Receiver();
            Server.Domain.Domain domain = new Domain.Domain(receiver);
            domain.Run();
        }
    }
}
