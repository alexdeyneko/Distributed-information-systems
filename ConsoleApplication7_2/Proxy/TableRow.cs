using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public class TableRow
    {
        public string Port { get; set; }
        public int BeginBucket { get; set; }
        public int EndBucket;

        public TableRow(string port, int beginBucket, int endBucket)
        {
            Port = port;
            BeginBucket = beginBucket;
            EndBucket = endBucket;
        }
    }
}
