using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace httpserver
{
    public class StartServer
    {
        static void Main(string[] args)
        {
            //Informerer om at serveren er startet
            Console.WriteLine("--- HTTP server is started --- \n \n");

            //Opretter og fortæller hvilken socket/port som serveren skal lytte på
            TcpListener serversocket = new TcpListener(65080);
            serversocket.Start();

            while (true)
            {

                  //Accepter Request
                  TcpClient connectionSocket = serversocket.AcceptTcpClient();
                  EndPoint ep = connectionSocket.Client.RemoteEndPoint;
                  string IP = ep.ToString();
                  Console.WriteLine("--- New Connection Initiated --- From host IP: " + IP);
                    
                  HttpService service = new HttpService(connectionSocket);

                  //Opretter en taskfactory som håndterer opgaverne
                  TaskFactory tf = new TaskFactory();
                  tf.StartNew(service.DoIt);

            } //End of while-loop

            //Endnu ikke implementeret. Lukker for HTTP servicen.
            serversocket.Stop();

        }//End of Main


    }//End of Class

}//End of namespace
