using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IknExercise7_Client
{
    class Client
    {
        public static void Main(string[] args)
        {
			var search = "u";
			ReadDir(search);
            
        }
		private const int port = 9000;
        const string IP_string_server = "10.0.0.2";
		const string IP_string_client = "10.0.0.1";

        public static void ReadDir(string charNumber)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Dgram, ProtocolType.Udp);
            IPAddress ip_address = IPAddress.Parse(IP_string_server);
            if (charNumber.Length > 1)
            {
                Console.WriteLine("argument too long");
                return;
            }
            byte[] sendbuf = Encoding.ASCII.GetBytes(charNumber);
			IPEndPoint server = new IPEndPoint(ip_address, port);
			socket.SendTo(sendbuf, server);
			Console.WriteLine("character send to server");

			/*UdpClient listener = new UdpClient(port);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(IP_string_server), port);

			byte[] bytes = listener.Receive(ref groupEP);
            string msg = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			Console.WriteLine(msg);*/
			byte[] bytes = new byte[1024];
			EndPoint recv = new IPEndPoint(IPAddress.Any, 0);
			socket.ReceiveFrom(bytes, ref recv);
			string msg = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
			msg = msg.TrimEnd('\0');
            Console.WriteLine(msg);
			Console.ReadLine();
            
    
    }
	}

   
}
