using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyNamespace
{
    public class KeyBucketTableService
    {
        private string configFileName = "bucket-keys.txt";

        private Dictionary<int, List<int>> table;
        
        public KeyBucketTableService()
        {
            table = new Dictionary<int, List<int>>();
        }

        public Dictionary<int, List<int>> GetTable()
        {
            return table;
        }

        public void AddPair(int key,int bucket)
        {
            if(table.ContainsKey(bucket))
            {
                if (!table[bucket].Contains(key))
                {
                    table[bucket].Add(key);
                }
                
            }
            else
            {
                table.Add(bucket, new List<int>() { key });
            }
            WriteTable();
            
        }

        public void DeletePair(int key,int bucket)
        {
            table[bucket].Remove(key);
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
                foreach (var row in table)
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
