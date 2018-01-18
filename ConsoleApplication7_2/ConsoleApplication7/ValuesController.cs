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

        private void WriteData()
        {
            using (StreamWriter writer = new StreamWriter(Node.dbFileName, false))
            {
                foreach (var item in Node.dictionary)
                {
                    writer.Write(item.Key + " ");
                    writer.WriteLine(item.Value);
                }
            }
        }
        

        // GET api/values/5 
        public string Get(string id)
        {
            
            return Node.dictionary[id];
        }


        // PUT api/values/5 
        public void Put(string id, [FromBody]string value)
        {
  
            if (!Node.dictionary.ContainsKey(id))
            {
                Node.dictionary.Add(id, value); 
            }
            else
            {
                Node.dictionary[id] = value;
            }
            WriteData();
            Node.PutIntoReplica(id, value);
            
        }
        // DELETE api/values/5 
        public void Delete(string id)
        {
            //readAll();
            if (Node.dictionary.ContainsKey(id))
            {
                Node.dictionary.Remove(id);
            }
            WriteData();
            Node.DeleteFromReplica(id);
        }

       
    }
}
