﻿using System;
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
    public class UnitTest1 : Sender
    {
        static private string port = "9000";
        private string path = @"nodes\"+port+".txt";
        public Dictionary<string, string> testData;
        public UnitTest1()
        {
            baseAddress = "http://localhost:"+port+"/api/values/";
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
    public class UnitTest2 : Sender
    {
        public Dictionary<string, string> testData;
        string proxyPort = "9004";
        string[] nodePorts = { "9000", "9001", "9002", "9003" };

        public UnitTest2()
        {
            baseAddress = "http://localhost:" + proxyPort + "/api/proxy/";
            testData = new TestDataGenerator().GenerateTestData();

        }

        [TestMethod]
        public void StartTwoNodesAndProxy()
        {
            string proxyArgs = proxyPort;

            for (int i = 0; i < 2; i++)
            {
                proxyArgs += " " + nodePorts[i];
                Process.Start("Node.exe", nodePorts[i]);
            }

            Process.Start("Proxy.exe", proxyArgs);

        }
        [TestMethod]
        public void PutValues()
        {
            
            foreach (var item in testData)
            {
                Put(item.Key, item.Value);
            }
            
        }
        
    }
}
