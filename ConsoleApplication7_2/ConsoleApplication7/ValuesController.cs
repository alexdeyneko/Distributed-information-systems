using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApplication7
{
    public class ValuesController : ApiController,IAPI
    {
        //private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        /*
        private void readAll()
        {
            foreach (var item in File.ReadLines(Storage.filePath).ToList())
            {
                string key = item.Split(' ')[0];
                string value = item.Split(' ')[1];
                dictionary.Add(key, value);
            }
        }
        */
        private void WriteData()
        {
            using (StreamWriter writer = new StreamWriter(Storage.filePath, false))
            {
                foreach (var item in Storage.dictionary)
                {
                    writer.Write(item.Key + " ");
                    writer.WriteLine(item.Value);
                }
            }
        }
        

        // GET api/values/5 
        public string Get(string id)
        {
            //readAll();
            //return dictionary[id];
            return Storage.dictionary[id];
        }


        // PUT api/values/5 
        public void Put(string id, [FromBody]string value)
        {

            //readAll();
            if (!Storage.dictionary.ContainsKey(id))
            {
                Storage.dictionary.Add(id, value); 
            }
            else
            {
                Storage.dictionary[id] = value;
            }
            WriteData();
            
        }
        // DELETE api/values/5 
        public void Delete(string id)
        {
            //readAll();
            if (Storage.dictionary.ContainsKey(id))
            {
                Storage.dictionary.Remove(id);
            }
            WriteData();
        }

       
    }
}
