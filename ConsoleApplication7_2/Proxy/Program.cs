using ConsoleApplication7;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            CheckTable();
            using (WebApp.Start<Startup>(url: "http://localhost:" + ProxyStorage.port + "/"))
            {
                Console.WriteLine("Proxy: port " + ProxyStorage.port);
                for (; ; ) { }
            }
        }

        static void CheckTable()
        {
            string configFileName = "config.txt";
            
            if (!File.Exists(configFileName))
            {
                File.Create(configFileName).Dispose();
                //WriteTable(configFileName);
            }
            WriteTable(configFileName);

            ReadTable(configFileName);
            
        }

        static void WriteTable(string configFileName)
        {
            using (StreamWriter writer = new StreamWriter(configFileName, false))
            {
                for (int i=0;i<ProxyStorage.nodePorts.Count;i++)
                {
                    //порт ноды
                    writer.Write(ProxyStorage.nodePorts[i]+" ");
                    //начальный bucket
                    writer.Write(ProxyStorage.bucketCount / ProxyStorage.nodePorts.Count * i+" ");
                    //последний bucket
                    writer.WriteLine(ProxyStorage.bucketCount / ProxyStorage.nodePorts.Count * (i+1)-1);

                }
            }
        }

        static void ReadTable(string configFileName)
        {
            foreach (var item in File.ReadLines(configFileName).ToList())
            {
                var words = item.Split(' ');
                string port = words[0];
                int begin = Convert.ToInt32(words[1]);
                int end = Convert.ToInt32(words[2]);

                ProxyStorage.tableRows.Add(new TableRow(port,begin,end));
            }
        }
    }
}
