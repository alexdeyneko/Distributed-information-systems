﻿using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

namespace ConsoleApplication7
{
    class Program
    {

        static void Main(String[] args)
        {
           
            Storage.port =args[0];
            
            CreateDB();
            
            using (WebApp.Start<Startup>(url: "http://localhost:"+Storage.port+"/"))
            {
                Console.WriteLine("Node: port "+Storage.port);
                for (;;) { }
            }
            
        }

        static private void CreateDB()
        {
            Storage.filePath= @"nodes\" + Storage.port + ".txt";

            if (!File.Exists(Storage.filePath))
            {
                File.Create(Storage.filePath);
            }
        }
    }
}
