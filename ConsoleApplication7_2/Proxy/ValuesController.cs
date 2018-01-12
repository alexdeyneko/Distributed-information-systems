using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApplication7
{
    public class ProxyController : ApiController,IAPI
    {
        
       
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
        
    }
}
