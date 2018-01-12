using ConsoleApplication7;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>(url: "http://localhost:9004/"))
            {
                for (; ; ) { }
            }
        }
    }
}
