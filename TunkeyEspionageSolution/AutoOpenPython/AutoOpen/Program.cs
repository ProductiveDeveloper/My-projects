using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AutoOpen
{
    class Program
    {
        static void Main(string[] args)
        {
            Storage.InMemory.Storage storage = new Storage.InMemory.Storage("Paths");
            Domain.Domain domain = new Domain.Domain(storage);
            domain.Run();
        }
    }
}
