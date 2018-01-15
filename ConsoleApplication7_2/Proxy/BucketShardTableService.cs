using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public class BucketShardTableService
    {
        private string configFileName = "bucket-shard.txt";
        public void LoadCurrentTable() //создает таблицу либо загружает готовую (при старте)
        {
            
            if (!File.Exists(configFileName))
            {
                File.Create(configFileName).Dispose();
                
            }
            if (new FileInfo(configFileName).Length == 0)
            {
                ProxyStorage.bucketShardTable = GetNewTable();
                WriteTable(ProxyStorage.bucketShardTable);
               
            }
            else
            {
                ProxyStorage.bucketShardTable=GetOldTable();
            }
           
        }
        
        
        private void WriteTable(Dictionary<int, string>table) //выгружает таблицу в файл
        {
            using (StreamWriter writer = new StreamWriter(configFileName, false))
            {
                foreach (var row in table)
                {
                    //WriteRow(row.Key, row.Value);
                    //номер бакета
                    writer.Write(row.Key + " ");
                    //номер порта
                    writer.WriteLine(row.Value);
                }
            }
           
        }
        
        private Dictionary<int, string> GetOldTable()     //загружает таблицу из файла
        {
            Dictionary<int, string> table = new Dictionary<int, string>();
            foreach (var item in File.ReadLines(configFileName).ToList())
            {
                var words = item.Split(' ');
                int bucket = Convert.ToInt32(words[0]);
                string port = words[1];
                table.Add(bucket,port);
            }
            return table;
        }

        public Dictionary<int, string> GetNewTable()       //генерирует новую таблицу, исходя из кол-ва нод
        {

            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            for (int i = 0; i < ProxyStorage.bucketCount; i++)
            {
                dictionary.Add(i, ProxyStorage.nodePorts[i / (ProxyStorage.bucketCount / ProxyStorage.nodePorts.Count)]);
            }
            return dictionary;

        }

        public void ChangeShard(int bucket,string shard)
        {
            ProxyStorage.bucketShardTable[bucket] = shard;
            WriteTable(ProxyStorage.bucketShardTable);
        }



    }
}
