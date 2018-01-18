using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    public class StringGenerator
    {
        
        static public string GenerateProxyAddress(string port)
        {
            return "http://localhost:" + port + "/api/proxi/";
        }

        static public string GenerateNodeAddress(string port)
        {
            return "http://localhost:" + port + "/api/values/";
        }

        static public string GenerateDBFilePath(string port)
        {
            return @"nodes\" + port + ".txt";
        }


    }
}
