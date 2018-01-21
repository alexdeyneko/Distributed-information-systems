using ConsoleApplication7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyNamespace
{
    public class Resharder
    {

        private Sender sender = new Sender();

        public void Analize(KeyBucketTableService kbt, BucketShardTableService bst)
        {
            Dictionary<int, string> BSTable = bst.GetNewTable();
            foreach (var row in BSTable)
            {


                if (row.Value != bst.GetTable()[row.Key])
                {
                    Reshard(bst.GetTable()[row.Key], row.Value, FindRowsFromBucket(row.Key, kbt.GetTable()));
                    bst.ChangeShard(row.Key, BSTable[row.Key]);

                }
            }
        }

        private List<int> FindRowsFromBucket(int bucket, Dictionary<int, List<int>> table)
        {
            return table[bucket];
        }

        private void Reshard(string oldPort, string newPort, List<int> keys)
        {

            foreach (var key in keys)
            {
                sender.baseAddress = StringGenerator.GenerateNodeAddress(oldPort);
                string id = key.ToString();

                var value = sender.Get(id);

                sender.baseAddress = StringGenerator.GenerateNodeAddress(newPort);
                sender.Put(id, value);
                sender.baseAddress = StringGenerator.GenerateNodeAddress(oldPort);
                sender.Delete(id);
            }

        }
    }
}

