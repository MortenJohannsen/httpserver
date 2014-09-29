using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    class StartServer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- HTTP server is started ---");

            TcpListener serversocket = new TcpListener(8080);
            serversocket.Start();

            while (true)
            {
                //Accept Request
                TcpClient connectionSocket = serversocket.AcceptTcpClient();
                Console.WriteLine("--- New Connection Initiated ---");

                HTTPService service = new HTTPService(connectionSocket);


            }

        }
    }
}
