using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyNamespace
{
    public class BucketShardTableService
    {
        private string configFileName = "bucket-shard.txt";
        private Dictionary<int, string> table;

        public BucketShardTableService()
        {
            table = new Dictionary<int, string>();
        }

        public Dictionary<int, string>  GetTable()
        {
            return table;
        }
        public void LoadCurrentTable() //создает таблицу либо загружает готовую (при старте)
        {
            
            if (!File.Exists(configFileName))
            {
                File.Create(configFileName).Dispose();
                
            }
            if (new FileInfo(configFileName).Length==0)
            {
                table= GetNewTable();
                WriteTable(table);
               
            }
            else
            {
                table=GetOldTable();
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

            for (int i = 0; i < Proxy.bucketCount; i++)
            {
                dictionary.Add(i, Proxy.nodePorts[i / (Proxy.bucketCount / Proxy.nodePorts.Count)]);
            }
            return dictionary;

        }
        
        public void ChangeShard(int bucket,string shard)
        {
            table[bucket] = shard;
            WriteTable(table);
        }



    }
}
