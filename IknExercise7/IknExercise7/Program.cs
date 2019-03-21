using System;

namespace IknExercise7
{
    class MainClass
    {
        public static void Main(string[] args)
        {
			UdpServer server = new UdpServer();
            server.StartServer("10.0.0.1",10000);
        }
    }
}
