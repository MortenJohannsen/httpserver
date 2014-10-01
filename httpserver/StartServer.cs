using System;
using System.Net.Sockets;

namespace httpserver
{
    public class StartServer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- HTTP server is started ---");

            TcpListener serversocket = new TcpListener(8087);
            serversocket.Start();

            while (true)
            {
                //Accept Request
                TcpClient connectionSocket = serversocket.AcceptTcpClient();
                Console.WriteLine("--- New Connection Initiated ---");

                HttpService service = new HttpService(connectionSocket);

                service.DoIt();

            } //End of while-loop
            serversocket.Stop();
        }//End of Main
    }//End of Class
}//End of namespace
