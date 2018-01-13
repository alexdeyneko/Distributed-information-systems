using Proxy;
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

        private void Route(int id)
        {
            int shard= Shard(Convert.ToInt32(id), ProxyStorage.nodePorts.Count);
            sender.baseAddress= "http://localhost:" + ProxyStorage.nodePorts[shard].ToString()+"/api/values/";

        }

        public void Put(string id, [FromBody]string value)
        {
            Route(Convert.ToInt32(id));
            sender.Put(id,value);
        }

        public string Get(string id)
        {
            Route(Convert.ToInt32(id));
            return sender.Get(id);
        }

        public void Delete(string id)
        {
            Route(Convert.ToInt32(id));
            sender.Delete(id);
        }
        
    }
}
