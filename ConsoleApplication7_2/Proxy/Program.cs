using ConsoleApplication7;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyStorage.port = args[0];
            for (int i = 1; i < args.Length; i++)
                ProxyStorage.nodePorts.Add(args[i]);

            new BucketShardTableService().LoadCurrentTable();
            new KeyBucketTableService().LoadCurrentTable();
           
            using (WebApp.Start<Startup>(url: "http://localhost:" + ProxyStorage.port + "/"))
            {
                new Resharder().Analize();
                Console.WriteLine("Proxy: port " + ProxyStorage.port);
                for (; ; ) { }
            }
        }
    }
}