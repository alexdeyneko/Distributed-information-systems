using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using ConsoleApplication7;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.Owin.Hosting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace UnitTestProject1
{
    public class TestDataGenerator
    {
        public Dictionary<string, string> GenerateTestData()
        {
            Dictionary<string, string> testData=new Dictionary<string, string>();
            int count = 400;
            for (int i = 0; i < count; i++)
            {
                testData.Add(i.ToString(), (i*10).ToString());
            }
            return testData;
        }
        
    }

    [TestClass]
    public class NodeTests : Sender
    {
        static private string port = "9000";
        private string path = StringGenerator.GenerateDBFilePath(port);
        public Dictionary<string, string> testData;

        public NodeTests()
        {
            baseAddress = StringGenerator.GenerateNodeAddress(port);
            testData = new TestDataGenerator().GenerateTestData();

        }
        [TestMethod]
        public void OpenConnection()
        {
            Process.Start("Node.exe",port);

        }
        [TestMethod]
        public void ClearDB()
        {
            File.WriteAllText(path, string.Empty);
            Assert.AreEqual(new FileInfo(path).Length, 0);
        }

        

        [TestMethod]
        public void PutValues()
        {
            int oldSize = File.ReadAllLines(path).Length;
           
            foreach (var item in testData)
            {
                Put(item.Key, item.Value);
            }
            int newSize = File.ReadAllLines(path).Length;

            Assert.AreEqual(newSize, oldSize + testData.Count);

        }

        [TestMethod]
        public void GetValues()
        {

            foreach (var item in testData)
            {
                string content = Get(item.Key);
                Assert.AreNotEqual(content, "BadRequest");
                Assert.AreEqual(content, item.Value);
            }

        }

        [TestMethod]
        public void UpdateValues()
        {
            int oldSize = File.ReadAllLines(path).Length;
            
            foreach (var item in testData)
            {
                Put(item.Key, item.Value);
            }

            GetValues();

        }
        [TestMethod]
        public void RemoveValues()
        {
            foreach (var item in testData)
            {
                Delete(item.Key);
            }
            Assert.AreEqual(File.ReadAllLines(path).Length, 0);
        }

        [TestMethod]
        public void CheckRemoved()
        {
            foreach (var item in testData)
            {
                var value = Get(item.Key);
                Assert.AreEqual(Get(item.Key), "BadRequest");
            }
        }
    }


    [TestClass]
    public class ProxyTests : Sender
    {
        public Dictionary<string, string> testData;
        string proxyPort = "9004";
        string[] nodePorts = { "9000", "9001" 
                ,"9002","9003" 
        };

        public ProxyTests()
        {
            baseAddress = StringGenerator.GenerateProxyAddress(proxyPort);
            testData = new TestDataGenerator().GenerateTestData();

        }

        [TestMethod]
        public void StartNodesAndProxy()
        {
            string proxyArgs = proxyPort;

            for (int i = 0; i < nodePorts.Length; i++)
            {
                proxyArgs += " " + nodePorts[i];
                Process.Start("Node.exe", nodePorts[i]);
            }

            Process.Start("Proxy.exe", proxyArgs);

        }
        [TestMethod]
        public void ClearFiles()
        {
            List<string> files = new List<string>() {
                "bucket-keys.txt",
                "bucket-shard.txt" };
            
            foreach (var node in nodePorts)
                files.Add(StringGenerator.GenerateDBFilePath(node));
            foreach (var file in files)
                File.WriteAllText(file, string.Empty);
        }

        [TestMethod]
        public void KillProcesses()
        {
            foreach (var process in Process.GetProcesses())
            {
                if(process.ProcessName.Contains("Node")
                    || process.ProcessName.Contains("Proxy"))
                process.Kill();
            }
            
        }

        [TestMethod]
        public void PutValues()
        {
            foreach (var item in testData)
                Put(item.Key, item.Value);  
        }

        [TestMethod]
        public void CheckShardingResults()
        {
            for(int i=1;i<nodePorts.Length;i++)
            {
                Assert.AreEqual(GetFileSize(nodePorts[i]), GetFileSize(nodePorts[i - 1]));
            }
        }
        public int GetFileSize(string nodeNumber)
        {
            string path =StringGenerator.GenerateDBFilePath((nodeNumber).ToString());
            return File.ReadAllLines(path).Length;
        }

    }

    [TestClass]
    public class ReplicationTests
    {
        string masterPort = "9000";
        string []slavePorts = { "9001","9002"};

        [TestMethod]
        public void StartMasterAndSlaves()
        {
            Process.Start("Node.exe", masterPort+" "+String.Join(" ",slavePorts));
            Process.Start("Node.exe", slavePorts[0]);
            Process.Start("Node.exe", slavePorts[1]);

        }

        [TestMethod]
        public void CheckReplicationResults()
        {
            string masterText = File.ReadAllText(StringGenerator.GenerateDBFilePath((masterPort).ToString()));

            foreach (var slave in slavePorts)
            {
                Assert.AreEqual
                (
                   File.ReadAllText(StringGenerator.GenerateDBFilePath((slave).ToString())),
                   masterText
                );
            }
        }
    }
}
