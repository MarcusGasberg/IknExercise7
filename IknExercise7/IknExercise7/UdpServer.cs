using System;_
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace IknExercise7
{
    public class UdpServer
    {
		private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public void StartServer(string ipAddress,int port)
		{
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(ipAddress),port);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
			_socket.Bind(groupEP);

            try
			{
				Console.WriteLine("Server started");
				while(true)
				{
					byte[] bytes = new byte[1024]; 
					_socket.Receive(bytes);
					string msg = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine(msg);
					if (msg.Length > 1)
						continue;
					msg = msg.ToLower();
                    switch(msg[0])
					{
						case 'l':
							SendFileToSocket("/proc/uptime");
							continue;
						case 'u':
							SendFileToSocket("/proc/loadavg");
							continue;
                           
						default:
							Debugger.Break();
							break;
					}
				}
			}
            catch(SocketException e)
			{
				Console.WriteLine(e);
			}
            finally
			{
				_socket.Close();
			}
		}

		private void SendFileToSocket(string filePath)
		{
			string text = File.ReadAllText(filePath);
			Byte[] sendBuffer = Encoding.UTF8.GetBytes(text);
			_socket.Send(sendBuffer);
		}
	}
}
