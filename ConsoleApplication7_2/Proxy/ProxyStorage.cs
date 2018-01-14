using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public static class ProxyStorage
    {
        public static string port;
        public static List<string>nodePorts=new List<string>();
        public static int bucketCount=100;
        public static Dictionary<int,string> bucketShardTable=new Dictionary<int, string>();
        public static Dictionary<int, List<int>> keyBucketTable 
            = new Dictionary<int, List<int>>();

    }
}
