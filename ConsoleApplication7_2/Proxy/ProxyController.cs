using Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApplication7
{
    public class ProxyController : ApiController,IAPI
    {
        
       
        private Sender sender = new Sender();

        //private int Shard(int key, int n)
        private int BucketFunction(int key)
        {
            return key%ProxyStorage.bucketCount;
        }

        private void Route(int id)
        {
            int bucket= BucketFunction(Convert.ToInt32(id));

            sender.baseAddress= "http://localhost:" + GetShard(bucket)+"/api/values/";

        }

        private string GetShard(int bucket)
        {
            string port = "";
            foreach (var row in ProxyStorage.tableRows)
            {
                if(bucket<=row.EndBucket && bucket>=row.BeginBucket)
                {
                    port = row.Port;
                }
            }
            return port;
        }

        public void Put(string id, [FromBody]string value)
        {
            Route(Convert.ToInt32(id));
            sender.Put(id,value);
        }

        public string Get(string id)
        {
            Route(Convert.ToInt32(id));
            return sender.Get(id);
        }

        public void Delete(string id)
        {
            Route(Convert.ToInt32(id));
            sender.Delete(id);
        }
        
    }
}
