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
        private string configFileName = "bucket-keys.txt";

        
        public void AddPair(int key,int bucket)
        {
            if(ProxyStorage.keyBucketTable.ContainsKey(bucket))
            {
                if (!ProxyStorage.keyBucketTable[bucket].Contains(key))
                {
                    ProxyStorage.keyBucketTable[bucket].Add(key);
                }
                
            }
            else
            {
                ProxyStorage.keyBucketTable.Add(bucket, new List<int>() { key });
            }
            
        }

        public void DeletePair(int key,int bucket)
        {
            ProxyStorage.keyBucketTable[bucket].Remove(key);
            /*
            if (ProxyStorage.keyBucketTable[bucket].Count.Equals(0))
            {
                ProxyStorage.keyBucketTable.Remove(bucket);
            }
            */
        }

        public void LoadCurrentTable()
        {
            if (!File.Exists(configFileName))
            {
                File.Create(configFileName).Dispose();
            }
            if (new FileInfo(configFileName).Length == 0)
            {
                PrepareTable();
            }
            else
            {
                ReadTable();
            }
            
        }

        public void PrepareTable()
        {
            using (StreamWriter writer = new StreamWriter(configFileName, false))
            {
                for (int i=0;i< ProxyStorage.bucketCount;i++)
                {
                    ProxyStorage.keyBucketTable.Add(i,new List<int>());
                    writer.WriteLine(i);
                }
            }
        }
        public void WriteTable()
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
                for(int i=1;i<words.Length;i++)
                {
                    AddPair(Convert.ToInt32(words[i]),bucket);
                }
                
            }
        }
    }
}
