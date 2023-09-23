using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is client.");
            const string ip = "127.0.0.1"; // this is client's ip and port
            const int port = 8082;
            EndPoint udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // client's endpoint
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081); // server's endpoint
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(udpEndPoint);
            Console.WriteLine("Client started successfully!");

            while (true)
            {
                var buffer = new byte[4096]; // Инициализация буфера, размера сообщения и данных
                var size = 0;
                var data = new StringBuilder();

                var message = Console.ReadLine();
                udpSocket.SendTo(Encoding.UTF8.GetBytes(message), serverEndPoint);

                do
                {
                    size = udpSocket.ReceiveFrom(buffer, ref serverEndPoint);
                    data.Append(Encoding.UTF8.GetString(buffer));
                }
                while (udpSocket.Available > 0);

                Console.WriteLine(data);
            }
            // TODO: Закрытие сокета
        }
    }
}
