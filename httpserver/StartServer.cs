using System;
using System.Net.Sockets;

namespace httpserver
{
    public class StartServer
    {
        static void Main(string[] args)
        {
            //Informerer om at serveren er startet
            Console.WriteLine("--- HTTP server is started ---");

            //Opretter og fortæller hvilken socket/port som serveren skal lytte på
            TcpListener serversocket = new TcpListener(8087);
            serversocket.Start();

            while (true)
            {
                //Accepter Request
                TcpClient connectionSocket = serversocket.AcceptTcpClient();
                Console.WriteLine("--- New Connection Initiated ---");

                HttpService service = new HttpService(connectionSocket);

                service.DoIt();

            } //End of while-loop

            //Endnu ikke implementeret. Lukker for HTTP servicen.
            serversocket.Stop();

        }//End of Main


    }//End of Class

}//End of namespace
