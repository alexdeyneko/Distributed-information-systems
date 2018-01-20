using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    public class Node
    {
        public static Dictionary<string, string> dictionary;
        public static string dbFileName;

        private string nodePort;
        static List<string> slavePorts;
        static Sender sender = new Sender();


        public Node(string port,List<string> slaves)

        {
            nodePort = port;
            slavePorts = slaves;
            dbFileName = @"nodes\"  + nodePort + ".txt";
            dictionary = new Dictionary<string, string>();
           
        }

       
        public void Start()
        {
            using (WebApp.Start<Startup>(url: "http://localhost:" + nodePort + "/"))
            {
                Console.WriteLine("Node: port " + nodePort);
                for (; ; ) { }
            }
        }
        public void CreateDB()
        {

            if (!File.Exists(dbFileName))
            {
                File.Create(dbFileName);
            }
            else
            {
                ReadData();
            }
        }

        private void ReadData()
        {

            foreach (var item in File.ReadLines(dbFileName).ToList())
            {
                var words = item.Split(new char[] { ' ' }, 2);

                string key = words[0];
                string value = words[1];

                dictionary.Add(key, value);
            }
        }

        static public void PutIntoReplica(string key, string value)
        {
            foreach(var port in slavePorts)
            {
                sender.baseAddress = StringGenerator.GenerateNodeAddress(port);
                sender.Put(key, value);
            }
        }

        static public void DeleteFromReplica(string key)
        {
            foreach (var port in slavePorts)
            {
                sender.baseAddress = StringGenerator.GenerateNodeAddress(port);
                sender.Delete(key);
            }
        }
    }
}
