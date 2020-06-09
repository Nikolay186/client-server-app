using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            try
            {
                StartConnection("localhost", 8880);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static void StartConnection(string hostName, int port)
        {
            byte[] recieve = new byte[1024];

            IPHostEntry host = Dns.GetHostEntry(hostName);
            IPAddress ip = host.AddressList[0];
            IPEndPoint ipEnd = new IPEndPoint(ip, port);

            Socket transferSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            transferSocket.Connect(ipEnd);

            Console.WriteLine("Enter your message: ");
            string msg = Console.ReadLine();

            Console.WriteLine($"Connecting to port {transferSocket.RemoteEndPoint}");

            byte[] sendMsg = Encoding.UTF8.GetBytes(msg);
            int bytesSent = transferSocket.Send(sendMsg);
            int bytesRecieved = transferSocket.Receive(recieve);

            Console.WriteLine($"Server's reply: {Encoding.UTF8.GetString(recieve, 0, bytesRecieved)}");

            if (msg.IndexOf("shutdown") == -1)
            {
                StartConnection(hostName, port);
            }
            transferSocket.Shutdown(SocketShutdown.Both);
            transferSocket.Close();
        }
    }
}
