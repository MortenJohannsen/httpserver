using System;
using System.IO;
using System.Net.Sockets;

namespace httpserver
{
    public class HttpService
    {
        private TcpClient connectionSocket;
        private string date = DateTime.Now.ToString();
        private const string CrLf = "\r\n";
        private const string HtmlType = "text/html";
        private static readonly string RootCatalog = "c:\\webserver";
        private string _statusline = null;
        private string[] RequestArray = null;

        public HttpService(TcpClient cntsocket)
        {
            this.connectionSocket = cntsocket;
        }

        public void DoIt()
        {
            using (connectionSocket)
            {
                
                


                Console.WriteLine("Der er oprettet forbindelse...");
                NetworkStream ns = connectionSocket.GetStream();
                var sw = new StreamWriter(ns);
                var sr = new StreamReader(ns);
                sw.AutoFlush = true;

                string getRequest = sr.ReadLine();
                Console.Write(getRequest);

                

                sw.Write(this.AssembleHttpResponse(getRequest));

                RequestFile(RequestArray.GetValue(1).ToString(), ns);

                Console.Write(RequestArray.GetValue(1).ToString());

                Console.WriteLine("--- Message sent" + this.AssembleHttpResponse(getRequest + RootCatalog + RequestArray.GetValue(1)));

                sw.Close();
                sr.Close();
            }
            connectionSocket.Close();
        }

        private string DoesFileExist(string[] RequestArray)
        {
            string statuslineTrue = "HTTP/1.0 200 OK" + CrLf;
            string statuslineFalse = "HTTP/1.0 404 Not Found" + CrLf;

            bool existingFile = File.Exists(RootCatalog + RequestArray.GetValue(1));

                if (existingFile)
                {
                    Console.WriteLine(existingFile);
                    return statuslineTrue;
                }
                else
                {
                    Console.WriteLine(existingFile);
                    return statuslineFalse;
                }
        }


        private void RequestFile(string array, NetworkStream nsw)
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

        private string AssembleHttpResponse(string getRequest)
        {
            RequestArray = getRequest.Split(' ');
            
            _statusline = this.DoesFileExist(RequestArray);

            string header1 = "Last-modified: " + date + CrLf;
            string header2 = "Content-type: " + HtmlType + CrLf;
            string blankline = CrLf;

            return _statusline + header1 + header2 + blankline;

        }
    }
}
