using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApplication7
{
    public class ValuesController : ApiController
    {
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();
        
        private void readAll()
        {
            foreach (var item in File.ReadLines(Storage.filePath).ToList())
            {
                string key = item.Split(' ')[0];
                string value = item.Split(' ')[1];
                dictionary.Add(key, value);
            }
        }
        private void writeAll()
        {
            using (StreamWriter writer = new StreamWriter(Storage.filePath, false))
            {
                foreach (var item in dictionary)
                {
                    writer.Write(item.Key + " ");
                    writer.WriteLine(item.Value);
                }
            }
        }
        

        // GET api/values/5 
        public string Get(string id)
        {
            readAll();
            return dictionary[id];
        }


        // PUT api/values/5 
        public void Put(string id, [FromBody]string value)
        {

            readAll();
            if (!dictionary.ContainsKey(id))
            {
                dictionary.Add(id, value); 
            }
            else
            {
                dictionary[id] = value;
            }
            writeAll();
            
        }

        public void Delete(string id)
        {
            readAll();
            if (dictionary.ContainsKey(id))
            {
                dictionary.Remove(id);
            }
            writeAll();
        }

       
       /*
        private Sender sender = new Sender();
       
        private int Shard(int key, int n)
        {
            return key%n;
        }
        private string Route(int port)
        {
            return "http://localhost:" + (9000 + port).ToString()+"/";
        }

        public void Put(string id, [FromBody]string value)
        {
            sender.baseAddress = Route(Shard(Convert.ToInt32(id), 2));
            sender.Put(id,value);
        }

        public string Get(string id)
        {
            sender.baseAddress = Route(Shard(Convert.ToInt32(id), 2));
            return sender.Get(id);
        }

        public void Delete(string id)
        {
            sender.baseAddress = Route(Shard(Convert.ToInt32(id), 2));
            sender.Delete(id);
        }
        */
    }
}
