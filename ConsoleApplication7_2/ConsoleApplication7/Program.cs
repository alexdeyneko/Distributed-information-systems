using Microsoft.Owin.Hosting;
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
           
            Node.nodePort =args[0];
            Node node= new Node(args.Skip(1).ToList());
            node.CreateDB();

            using (WebApp.Start<Startup>(url: "http://localhost:"+Node.nodePort+"/"))
            {
                Console.WriteLine("Node: port "+Node.nodePort);
                for (;;) { }
            }
            
        }
        
    }
}
