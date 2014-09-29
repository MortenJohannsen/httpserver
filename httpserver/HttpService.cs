using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace httpserver
{
    public class HTTPService
    {
        private TcpClient connectionSocket;

        public HTTPService(TcpClient cntsocket)
        {
            this.connectionSocket = cntsocket;
        }

        public void doIt()
        {
            Console.WriteLine("Der er oprettet forbindelse...");
        }


    }
}
