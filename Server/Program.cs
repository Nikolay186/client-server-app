using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static int port = 8880;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            Console.WriteLine("The server is running");

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ip = host.AddressList[0];
            IPEndPoint ipEnd = new IPEndPoint(ip, port);

            Socket socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(ipEnd);
                socket.Listen(10);

                while(true)
                {
                    Console.WriteLine($"Listening port {ipEnd}");
                    Socket s = socket.Accept();

                    string response = null;

                    byte[] data = new byte[1024];
                    int dataLength = s.Receive(data);
                    response += Encoding.UTF8.GetString(data, 0, dataLength);
                    Console.WriteLine($"Retrieving response... \r\n {response}");

                    string reply = "Query size: " + response.Length.ToString();

                    byte[] replyMsg = Encoding.UTF8.GetBytes(reply);
                    s.Send(replyMsg);

                    if (response.IndexOf("shutdown") > -1)
                    {
                        Console.WriteLine("Connection closed.");
                        break;
                    }
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
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
    }
}
