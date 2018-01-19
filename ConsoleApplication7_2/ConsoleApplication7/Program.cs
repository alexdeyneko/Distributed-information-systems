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
           
            Node node= new Node(args[0],args.Skip(1).ToList());
            node.CreateDB();
            node.Start();
            
        }
        
    }
}
