using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    public class HTTPService
    {
        private TcpClient connectionSocket;
        private string date = DateTime.Now.ToString();
        private const string CRLF = "\r\n";
        private const string htmlType = "text/html";
        private static readonly string RootCatalog = "c:\\webserver";

        public HTTPService(TcpClient cntsocket)
        {
            this.connectionSocket = cntsocket;
        }

        public void doIt()
        {
            using (connectionSocket)
            {
                Console.WriteLine("Der er oprettet forbindelse...");
                NetworkStream ns = connectionSocket.GetStream();
                StreamWriter sw = new StreamWriter(ns);
                StreamReader sr = new StreamReader(ns);
                sw.AutoFlush = true;

                string getRequest = sr.ReadLine();
                Console.WriteLine(getRequest);

                string[] requestArray = new string[3];
                requestArray = getRequest.Split(' ');
                RequestFile(requestArray.GetValue(1).ToString(), ns);

                Console.WriteLine(requestArray.GetValue(1).ToString());

                string statusline = "HTTP/1.0 200 ok" + CRLF;
                string header1 = "Last-modified: " + date + CRLF;
                string header2 = "Content-type: " + htmlType + CRLF;
                string blankline = CRLF;
                string body = RootCatalog + requestArray.GetValue(1) + CRLF;


                //sw.WriteLine(statusline + header1 + header2 + blankline + body);


                Console.WriteLine("--- Message sent {0}, {1}, {2}, {3}, {4}:", statusline, header1, header2, blankline, body);
                
                sw.Close();
                sr.Close(); 
            }
            
            connectionSocket.Close();

        }

        public void RequestFile(string array, NetworkStream nsw)
        {
            try
            {
                FileStream fs = File.OpenRead(RootCatalog + array);
                fs.CopyTo(nsw);
            }
            catch (FileNotFoundException fnfex)
            {
                
                Console.WriteLine("File not found");
            }
            

        }


    }
}
