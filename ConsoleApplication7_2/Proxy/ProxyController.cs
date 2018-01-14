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
        private KeyBucketTableService kbt=new KeyBucketTableService();
        static int currentBucket;
        //private int Shard(int key, int n)
        private int BucketFunction(int key)
        {
            return key%ProxyStorage.bucketCount;
        }

        private void Route(int id)
        {
            currentBucket= BucketFunction(id);
            //kbt.AddPair(id,bucket);
            sender.baseAddress= "http://localhost:" + GetShard(currentBucket)+"/api/values/";

        }

        private string GetShard(int bucket)
        {
            
            return ProxyStorage.bucketShardTable[bucket];
        }

        public void Put(string id, [FromBody]string value)
        {
            Route(Convert.ToInt32(id));
            sender.Put(id,value);
            kbt.AddPair(Convert.ToInt32(id),currentBucket);
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
            kbt.AddPair(Convert.ToInt32(id), currentBucket);

        }

    }
}
