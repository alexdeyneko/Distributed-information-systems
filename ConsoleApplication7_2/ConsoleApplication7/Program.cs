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

        static void Main()
        {
            
            using (WebApp.Start<Startup>(url: "http://localhost:9004/"))
            {
                for (;;) { }
            }
            
        }
    }
}
