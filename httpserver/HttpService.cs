using System;
using System.IO;
using System.Net.Sockets;

namespace httpserver
{
    public class HttpService
    {
        private readonly TcpClient connectionSocket;
        private readonly string date = DateTime.Now.ToString();
        private const string CrLf = "\r\n";
        private const string HtmlType = "text/html";
        private const string RootCatalog = "c:\\webserver";
        private string _statusline = null;
        private string[] RequestArray = null;
        private EventLogger eventlogger;

        /// <summary>
        /// HTTP Service Constructor
        /// </summary>
        /// <param name="cntsocket">Den accepterede klient forbindelse</param>
        public HttpService(TcpClient cntsocket)
        {
            this.connectionSocket = cntsocket;
        }

        /// <summary>
        /// Modtager http request, behandler og returnerer et http response.
        /// Uddelegerer opgaver til private metoder.
        /// </summary>
        public void DoIt()
        {
            using (connectionSocket)
            {
                //Udskriver i konsol vindue at, forbindelse er oprettet
                Console.WriteLine("Der er oprettet forbindelse... \n");

                //Åben netværks-stream
                NetworkStream ns = connectionSocket.GetStream();
                
                //Læs request fra browser
                string therequest = OpenReader(ns);
                //Opretter event log som fortæller at en request er modtaget fra en client
                eventlogger.WriteLogEntry(1);

                //Sender http - response
                //Checking the request, handling nullReferenceExceptions to prevent server from crashing
                if (therequest == "close_but_no_cigar_exception-condition:119857")
                {
                    Console.WriteLine("Keep Alive");
                }
                else
                {
                    OpenWriter(ns, therequest);
                    RequestFile(RequestArray.GetValue(1).ToString(), ns);
                    //Opretter event log som fortæller at en response er sendt til en client
                    eventlogger.WriteLogEntry(2);

                    //Udskriv i konsol vindue
                    Console.WriteLine("Requested file: " + RequestArray.GetValue(1).ToString());
                    Console.WriteLine("--- Message sent: " + this.AssembleHttpResponse(therequest + CrLf + RootCatalog + RequestArray.GetValue(1)));
                }

                //Luk netværks-stream
                ns.Close();
            }
            connectionSocket.Close();
        }

        /// <summary>
        /// Opretter streamwriter og kalder metoden 'AssembleHttpResponse' som sammensætter http response.
        /// Herefter sender OpenWriter dette response.
        /// </summary>
        /// <param name="nsw">Instans af NetworkStream</param>
        /// <param name="request">Den sendte request fra klienten</param>
        private void OpenWriter(NetworkStream nsw, string request)
        {
            var sw = new StreamWriter(nsw);
            sw.AutoFlush = true;
            sw.Write(this.AssembleHttpResponse(request));
        }

        /// <summary>
        /// Opretter streamreader som læser http requests og returnerer denne.
        /// </summary>
        /// <param name="nsw">Instans af NetworkStream</param>
        /// <returns>Returnerer request i form af en string</returns>
        private string OpenReader(NetworkStream nsw)
        {
            var sr = new StreamReader(nsw);
            string getRequest = sr.ReadLine();
            if (getRequest != null)
            {
                return getRequest;
            }
            else
            {
                return "close_but_no_cigar_exception-condition:119857";
            }
        }

        /// <summary>
        /// Metode som checker om den forespurgte fil/side eksisterer.
        /// </summary>
        /// <param name="RequestArray">Property som indeholder den opdelte request</param>
        /// <returns>Returnerer specifik statusline alt efter om siden findes eller ikke findes</returns>
        private string DoesFileExist(string[] RequestArray)
        {
            string statuslineTrue = "HTTP/1.0 200 OK" + CrLf;
            string statuslineFalse = "HTTP/1.0 404 Not Found" + CrLf;

            bool existingFile = File.Exists(RootCatalog + RequestArray.GetValue(1));

                if (existingFile)
                {
                    Console.WriteLine("Does the file exist in RootDirectory: " + existingFile);
                    return statuslineTrue;
                }
                else
                {
                    Console.WriteLine("Does the file exist in RootDirectory: " + existingFile);
                    return statuslineFalse;
                }
        }

        /// <summary>
        /// RequestFile sender den requestede fil/side.
        /// </summary>
        /// <param name="filename">Filnavnet på den forespurgte fil/side</param>
        /// <param name="nsw">Instans af NetworkStream</param>
        private void RequestFile(string filename, NetworkStream nsw)
        {
            try
            {
                FileStream fs = File.OpenRead(RootCatalog + filename);
                fs.CopyTo(nsw);
            }
            catch (FileNotFoundException fnfex)
            {            
                Console.WriteLine("File not found");
            }
        }

        /// <summary>
        /// Metode som samler et http response, her vurderes også om filen/siden eksisterer i RootDirectory.
        /// </summary>
        /// <param name="getRequest">Requesten som er blevet sendt fra klienten.</param>
        /// <returns>HTTP response : bestående af statusline + headers + blankline</returns>
        private string AssembleHttpResponse(string getRequest)
        {
            
            RequestArray = getRequest.Split(' ');
            
            
            _statusline = this.DoesFileExist(RequestArray);

            string header1 = "Last-modified: " + date + CrLf;
            string header2 = "Content-type: " + HtmlType + CrLf;
            //string header3 = "Content-Length: " + ContentLengthInBytes().ToString() + CrLf;
            string blankline = CrLf;

            return _statusline + header1 + header2 + blankline;

        }

        public void logger(EventLogger log)
        {
            eventlogger = log;
        }

        //public int ContentLengthInBytes()
        //{
        //    byte[] bytearray = File.ReadAllBytes(RootCatalog + RequestArray.GetValue(1));

        //    return (int)bytearray.GetValue(0);
        //}

    }//End of class

}//End of namespace
