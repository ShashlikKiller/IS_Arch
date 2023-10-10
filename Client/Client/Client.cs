using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    internal class Client
    {
        static void Main(string[] args)
        {
            #region client initialization
            Console.WriteLine("This is client.");
            const string ip = "127.0.0.1"; // this is client's ip and port
            const int port = 8082;
            EndPoint udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // client's endpoint
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081); // server's endpoint
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(udpEndPoint);
            Console.WriteLine("Client started successfully!");
            Console.WriteLine(MenuText());
            #endregion
            #region working with server
            while (true)
            {
                var buffer = new byte[4096]; // Инициализация буфера, размера сообщения и данных
                int size;
                var data = new StringBuilder();
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                string message = Console.ReadLine();
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
            #endregion
        }

        public static string MenuText()
        {
            string answer = "";
            answer 
            += " Выберите то, что вы хотите сделать, и нажмите соответствующую кнопку :\n"
            + "   1. Вывод всех данных на экран\n"
            + "   2. Вывод карточки студента по идентификатору\n"
            + "   3. Сохранение всех записей в файл\n"
            + "   4. Удалить запись по ID студента\n"
            + "   5. Добавить новую запись\n"
            + "   Esc. Закрытие\n";
            return answer;
        }
    }
}
