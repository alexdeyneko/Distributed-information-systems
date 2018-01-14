using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public class KeyBucketTableService
    {
        private string configFileName = "key-bucket.txt";

        
        public void AddPair(int key,int bucket)
        {
            if(ProxyStorage.keyBucketTable.ContainsKey(bucket))
            {
                ProxyStorage.keyBucketTable[bucket].Add(key);
            }
            else
            {
                ProxyStorage.keyBucketTable.Add(bucket, new List<int>() { key });
            }
            WriteTable();
        }

        public void DeletePair(int key,int bucket)
        {
            ProxyStorage.keyBucketTable[bucket].Remove(key);
            if(ProxyStorage.keyBucketTable[bucket].Count.Equals(0))
            {
                ProxyStorage.keyBucketTable.Remove(bucket);
            }
            WriteTable();
        }

        public void LoadCurrentTable()
        {
            if (!File.Exists(configFileName))
            {
                File.Create(configFileName).Dispose();
            }
            ReadTable();
        }

        private void WriteTable()
        {
            using (StreamWriter writer = new StreamWriter(configFileName, false))
            {
                foreach (var row in ProxyStorage.keyBucketTable)
                {
                    //номер бакета
                    writer.Write(row.Key);
                    //ключи
                    string keys ="";
                    foreach(var item in row.Value)
                    {
                        keys += " "+ item.ToString();
                    }
                    writer.WriteLine(keys);

                }
            }
        }

        private void ReadTable()
        {
            foreach (var item in File.ReadLines(configFileName).ToList())
            {
                var words = item.Split(' ');
                int bucket = Convert.ToInt32(words[0]);
                words.ToList().RemoveAt(0);
                foreach(var key in words)
                {
                    AddPair(bucket, Convert.ToInt32(key));
                }

            }
        }
    }
}
