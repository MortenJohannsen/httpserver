using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace httpserverTest
{
    [TestClass]
    public class HttpServerTest
    {
        private const string CrLf = "\r\n";
        //private StreamWriter sw;
        //private StreamReader sr;
        //private NetworkStream ns;

        //private StartServer start;

        //[TestInitialize()]
        //public void HttpServerInit()
        //{

        //    //start = new StartServer();


        //    TcpClient clientsocket = new TcpClient("localhost", 8087);
        //    ns = clientsocket.GetStream();
        //    sw = new StreamWriter(ns);
        //    sr = new StreamReader(ns);
        //    sw.AutoFlush = true;

        //}

        [TestMethod]
        public void TestGet1()
        {
            String line = GetFirstLine("GET /mobajo.html HTTP/1.0");
            Assert.AreEqual("HTTP/1.0 200 OK", line);
        }

        [TestMethod]
        public void TestGet2()
        {
            String line = GetFirstLine("GET /fileDoesNotExist.txt HTTP/1.0");
            Assert.AreEqual("HTTP/1.0 404 Not Found", line);
        }


        //[TestMethod]
        //public void TestGetIllegalRequest()
        //{
        //    String line = GetFirstLine("GET /file.txt HTTP 1.0");
        //    Assert.AreEqual("HTTP/1.0 400 Illegal request", line);
        //}

        //[TestMethod]
        //public void TestGetIllegalMethodName()
        //{
        //    String line = GetFirstLine("PLET /file.txt HTTP/1.2");
        //    Assert.AreEqual("HTTP/1.0 400 Illegal request", line);
        //}

        //[TestMethod]
        //public void TestGetIllegalProtocol()
        //{
        //    String line = GetFirstLine("GET /file.txt HTTP/1.1");
        //    Assert.AreEqual("HTTP/1.0 400 Illegal protocol", line);
        //}

        //[TestMethod]
        //public void TestMethodNotImplemented()
        //{
        //    String line = GetFirstLine("POST /file.txt HTTP/1.0");
        //    Assert.AreEqual("HTTP/1.0 200 xxx", line);
        //}

        /// <summary>
        ///     Private helper method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static String GetFirstLine(String request)
        {
            var client = new TcpClient("localhost", 8087);
            NetworkStream networkStream = client.GetStream();

            var toServer = new StreamWriter(networkStream, Encoding.UTF8);
            toServer.Write(request + CrLf);
            toServer.Write(CrLf);
            toServer.Flush();

            var fromServer = new StreamReader(networkStream);
            String firstline = fromServer.ReadLine();
            toServer.Close();
            fromServer.Close();
            client.Close();
            return firstline;
        }
    }
}