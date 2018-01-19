using ConsoleApplication7;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyNamespace
{
    public class Proxy
    {
        private string proxyPort;
        public static List<string> nodePorts;
        public static int bucketCount;

        public static KeyBucketTableService kbt;
        private static BucketShardTableService bst;
       
        public Proxy(string port, int bCount, List<string>nPorts)
        {
            proxyPort = port;
            bucketCount = bCount;
            nodePorts = nPorts;
            bst = new BucketShardTableService();
            bst.LoadCurrentTable();
            kbt = new KeyBucketTableService();
            kbt.LoadCurrentTable();
        }

        public void Start()
        {
            using (WebApp.Start<Startup>(url: "http://localhost:" + proxyPort + "/"))
            {
                new Resharder().Analize(kbt,bst);
                Console.WriteLine("Proxy: port " + proxyPort);
                for (; ; ) { }
            }
        }

        public static int BucketFunction(int key)
        {
            return key % bucketCount;
        }

        private static string GetShard(int bucket)
        {
             return bst.GetTable()[bucket];
        }

        public static string GetAddress(int bucket)
        {
            return StringGenerator.GenerateNodeAddress(GetShard(bucket));
        }




    }
}
