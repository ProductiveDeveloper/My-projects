using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.InMemory;

namespace AutoOpen.Domain
{
    public class Domain
    {
        readonly Storage.InMemory.Storage Storage;

        public Domain(Storage.InMemory.Storage storage)
        {
            Storage = storage;
        }

        public void Run()
        {
            List<string> paths = Storage.Paths;
            if (paths != null)
                foreach (var item in this.Storage.Paths)
                {
                    OpenPath($@"{item}");
                }
        }

        private void OpenPath(string path)
        {
            var startInfo = new ProcessStartInfo("cmd", $@"/c cd C:\ && {path} && EXIT");
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
        }
    }
}
