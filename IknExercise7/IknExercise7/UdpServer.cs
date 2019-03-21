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
		private const int _port = 11000;
		private const string iPString = "10.0.0.1";

        public static void StartServer()
		{
			UdpClient listener = new UdpClient(_port);
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(iPString),_port);

            try
			{
				Console.WriteLine("Server started");
				while(true)
				{
					byte[] bytes = listener.Receive(ref groupEP);
					string msg = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
					if (msg.Length > 1)
						continue;
					msg = msg.ToLower();
                    switch(msg[0])
					{
						case 'l':
							SendFileToListener(listener, "/proc/uptime");
							continue;
						case 'u':
							SendFileToListener(listener, "/proc/loadavg");
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
				listener.Close();
			}
		}

		private static void SendFileToListener(UdpClient listener,string filePath)
		{
			string text = File.ReadAllText(filePath);
			Byte[] sendBuffer = Encoding.UTF8.GetBytes(text);
			listener.Send(sendBuffer, sendBuffer.Length);
		}
	}
}
