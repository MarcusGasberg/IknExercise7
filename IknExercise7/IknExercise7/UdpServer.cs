using System;
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
					EndPoint recvEP = new IPEndPoint(IPAddress.Any, 0);
					_socket.ReceiveFrom(bytes,ref recvEP);
					string msg = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
					msg = msg.TrimEnd('\0');
					Console.WriteLine(msg);                 
					msg = msg.ToLower();
                    switch(msg[0])
					{
						case 'l':
							SendFileToSocket("/proc/uptime",recvEP);
							continue;
						case 'u':
							SendFileToSocket("/proc/loadavg",recvEP);
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

		private void SendFileToSocket(string filePath, EndPoint endpoint)
		{
			string text = File.ReadAllText(filePath);
			Byte[] sendBuffer = Encoding.UTF8.GetBytes(text);
			_socket.SendTo(sendBuffer,endpoint);
		}
	}
}
