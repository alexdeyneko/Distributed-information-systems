using ConsoleApplication7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public class Resharder
    {
        KeyBucketTableService kbt = new KeyBucketTableService();
        BucketShardTableService bst = new BucketShardTableService();
        private Sender sender = new Sender();


        public void Analize()
        {
            Dictionary<int, string> BSTable = bst.GetNewTable();
            foreach (var row in BSTable)
            {
                if (row.Value != ProxyStorage.bucketShardTable[row.Key])
                {
                    Reshard(ProxyStorage.bucketShardTable[row.Key], row.Value, FindRowsFromBucket(row.Key));
                    bst.ChangeShard(row.Key,BSTable[row.Key]);
                    //не дописывает
                }
            }
        }

        public List<int> FindRowsFromBucket(int bucket)
        {
            return ProxyStorage.keyBucketTable[bucket];
        }

        public void Reshard(string oldAddress, string newAddress, List<int> keys)
        {
            
            foreach (var key in keys)
            {
                sender.baseAddress = "http://localhost:" + oldAddress + "/api/values/";
                string id = key.ToString();
                try
                {
                    var value = sender.Get(id);

                    sender.baseAddress = "http://localhost:" + newAddress + "/api/values/";
                    sender.Put(id, value);
                    sender.baseAddress = "http://localhost:" + oldAddress + "/api/values/";
                    sender.Delete(id);
                }
                catch
                {

                }
            }
        }
    }
}
