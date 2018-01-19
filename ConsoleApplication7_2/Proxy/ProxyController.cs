using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ProxyNamespace;

namespace ConsoleApplication7
{
    public class ProxyController : ApiController,IAPI
    {
        private Sender sender = new Sender();
        
        public void Put(string id, [FromBody]string value)
        {

            int bucket = Proxy.BucketFunction(Convert.ToInt32(id));
            sender.baseAddress = Proxy.GetAddress(bucket);
            sender.Put(id,value);
            Proxy.kbt.AddPair(Convert.ToInt32(id), bucket);
            
            
        }

        public string Get(string id)
        {
            int bucket = Proxy.BucketFunction(Convert.ToInt32(id));
            sender.baseAddress = Proxy.GetAddress(bucket);
            return sender.Get(id);
        }

        public void Delete(string id)
        {
            int bucket = Proxy.BucketFunction(Convert.ToInt32(id));
            sender.baseAddress = Proxy.GetAddress(bucket);
            sender.Delete(id);
            Proxy.kbt.DeletePair(Convert.ToInt32(id), bucket);
            


        }

    }
}
