using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Storage.InMemory
{
    public class Storage
    {
        public List<string> Paths;
        private string Path;

        public Storage(string path)
        {
            Path = path;
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + $@"\{path}"))
                System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + $@"\{path}").Close();
            Paths = GetsPaths();
            if (Paths.Count == 0)
                Paths = null;
        }

        private List<string> GetsPaths()
        {
            List<string> result = new List<string>();
            using (StreamReader stream = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + $@"\{Path}" , Encoding.Default , false))
            {
                string text;
                while ((text = stream.ReadLine()) != null)
                {
                    result.Add(text);
                }
            }
            return result;
        }
    }
}
