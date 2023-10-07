using IS_Arch.BackEnd;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NLog;
using static IS_Arch.BackEnd.DataManipulation;
using CsvHelper.Configuration;
using CsvHelper;
using static IS_Arch.BackEnd.StudentBuilder;
using static IS_Arch.BackEnd.ServerCommandsAsync;

namespace IS_Arch
{
    internal class Server
    // 1. Клиент производить только отображение данных.
    // To Do: Следующие операции производятся на стороне сервера:
    //   Передача запрошенных данных из файла:
    //   Передача всех данных
    //   Передача записи по номеру
    //   Запись новых данных от клиента в файл
    //   Удаление записи из файла по её номеру
    // 2. Логирование операций на стороне сервера с помощью библиотеки NLog
    // 3. Сервер должен многопоточным.
    // Многопоточность реализовать с помощью асинхронных методов класса UDPClient
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            Console.WriteLine("This is server.");
            #region database
            // database: start
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldUseConstructorParameters = type => false // Передача библиотеке разрешение на
                                                               // присваивание полей без конструктора
            };
            string path; // переменная пути до csv файла
            try
            {
                path = Environment.CurrentDirectory + "\\database.csv"; // Создание пути для .csv файла
            }
            catch
            {
                Console.WriteLine(" Файл по данному адресу отсутствует. Введите прямой адрес:");
                path = Console.ReadLine();
            }
            List<Student> Students;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                IEnumerable<Student> Records = csv.GetRecords<Student>(); // Базовый интерфейс для всех
                if (Records != null)                                      // неуниверсальных коллекций
                {
                    Students = Records.ToList();
                }
                else Students = new List<Student>();
            }
            #endregion
            #region server initialization
            // server: start
            const string ip = "127.0.0.1";
            const int port = 8081; // У КЛИЕНТА И СЕРВЕРА РАЗНЫЕ ПОРТЫ! СВЯЗЬ ЧЕРЕЗ СЕРВЕР И КЛИЕНТ ЭНДПОИНТ

            try
            {
                var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                udpSocket.Bind(udpEndPoint);
                StartReceiving(udpSocket, Students, path, logger);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            // server: end
            #endregion
        }

        private static async Task StartReceiving(Socket udpSocket, List<Student> Students, string path, Logger logger)
        {
            string server_answer; // Переменная ответа сервера клиенту
            string data; // Данные сообщения от клиента
            IPAddress clientIP = IPAddress.Parse("127.0.0.1"); // this is client's ip and port
            const int clientPort = 8082;
            EndPoint senderEndPoint = new IPEndPoint(clientIP, clientPort); // Эндпоинт клиента(отправителя сообщений)
            logger.Info("Server started at" + DateTime.Now);
            Console.WriteLine("Server started successfully!");
            while (true)
            {
                server_answer = "";
                data = ReceiveData(udpSocket, senderEndPoint);

                if (TypeCheck.IntCheckBool(data))
                {
                    Console.WriteLine("Client choice: " + data); // Вывод выбора пользователя в меню.
                    logger.Info("Client choice:" + data);
                    switch (Convert.ToInt32(data))
                    {
                        case 1: // Вывод всех записей на экран 
                             server_answer = case1(Students).Result;
                            break;
                        case 2: // Вывод записи по номеру 
                             server_answer = case2(udpSocket, senderEndPoint, Students).Result;
                            break;
                        case 3: // Запись данных в файл 
                             server_answer = case3(Students, path).Result;
                            break;
                        case 4: // Удалить запись по номеру
                             server_answer = case4(udpSocket, senderEndPoint, Students).Result;
                            break;
                        case 5: // Добавление новой записи
                             server_answer = case5(udpSocket, senderEndPoint, Students).Result;
                            break;
                        default:
                            server_answer = "Incorrect input. Please press the button from 1 to 5.";
                            logger.Error("Incorrect user input");
                            break;
                    }
                    SendDataAsync(udpSocket, senderEndPoint, server_answer + BackToMenu);
                }
                else
                {
                    SendDataAsync(udpSocket, senderEndPoint, "Invalid data. Try again.");
                    logger.Error("Incorrect user input");
                    Console.WriteLine("Received invalid data");
                }
            }
        }

        private static string BackToMenu = "Вы возвращены в меню. Выберите пункт от 1 до 5.\n";
    }
}
