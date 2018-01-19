using ConsoleApplication7;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            int bucketCount = 100;

            Proxy proxy = new Proxy(args[0], bucketCount, args.Skip(1).ToList());
            proxy.Start();
        }
    }
}