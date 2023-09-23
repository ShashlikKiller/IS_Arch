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
using static IS_Arch.BackEnd.DataManipulation;
using CsvHelper.Configuration;
using CsvHelper;

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
                StartReceiving(udpSocket, Students);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            // server: end
            #endregion
        }

        private static async void StartReceiving(Socket udpSocket, List<Student> Students)
        {
            byte[] buffer = new byte[256];
            EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Server started successfully!");
            while (true)
            {
                int size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
                string data = Encoding.UTF8.GetString(buffer, 0, size);
                string server_answer = "";
                byte[] responseBuffer;

                if (TypeCheck.IntCheckBool(data))
                {
                    Console.WriteLine("Client choice: " + data); // Вывод выбора пользователя в меню.
                    switch (Convert.ToInt32(data))
                    {
                        case 1: // Вывод всех записей на экран
                            
                            server_answer = ConsoleOutputAll(Students);
                            break;
                        case 2: // Вывод записи по номеру
                            server_answer = "Введите ID";
                            responseBuffer = Encoding.UTF8.GetBytes(server_answer);
                            udpSocket.SendTo(responseBuffer, SocketFlags.None, senderEndPoint); // Отправляем клиенту сообщение
                            size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
                            data = Encoding.UTF8.GetString(buffer, 0, size);

                            if (TypeCheck.IntCheckBool(data))
                            {
                                server_answer = OutputByID(Students, Convert.ToInt32(data));
                            }
                            else
                            {
                                server_answer = "Неверный формат ID";
                            }
                            break;
                        case 3: // Запись данных в файл
                            
                            break;
                        case 4: // Удалить запись по номеру
                            
                            break;
                        case 5: // Добавление новой записи
                            
                            break;
                        default:
                            server_answer = "Incorrect input. Please press the button from 1 to 5.";
                            break;
                    }
                    responseBuffer = Encoding.UTF8.GetBytes(server_answer);
                    udpSocket.SendTo(responseBuffer, SocketFlags.None, senderEndPoint);
                }
                else
                {
                    responseBuffer = Encoding.UTF8.GetBytes("Invalid data. Try again.");
                    udpSocket.SendTo(responseBuffer, SocketFlags.None, senderEndPoint);
                    Console.WriteLine("Received invalid data");
                }
            }

            // server: end


            
            //bool activeapp = true; // Это можно убрать, оставив в while просто true.
            //while (activeapp)
            //{
             //   MenuCall(Students, path);
            //}
        }

        //public void StartMessageLoop(Socket UDPSocket)
        //{
        //    var e = SocketAs
        //    _ = Task.Run(async () =>
        //    {
        //        SocketReceiveFromResult res;
        //        while (true)
        //        {
        //            res = await UDPSocket.ReceiveAsync()
        //        }
        //    }
        //}

        public static void SaveRecords(List<Student> Students, string pathtofile)
        {
            using (var swriter = new StreamWriter(pathtofile))
            using (var csvwriter = new CsvWriter(swriter, CultureInfo.InvariantCulture))
            {
                csvwriter.WriteRecords(Students);
            }
            Console.WriteLine("\n Данные успешно сохранены. \n");
        }

        public static void MenuCall(List<Student> Students, string path)
        {
            MenuText();
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1: // Вывод всех записей на экран
                    Console.WriteLine("\n Список студентов:\n");
                    ConsoleOutputAll(Students);
                    break;
                case ConsoleKey.D2: // Вывод записи по номеру
                    //OutputByID(Students);
                    break;
                case ConsoleKey.D3: // Запись данных в файл
                    SaveRecords(Students, path);
                    break;
                case ConsoleKey.D4: // Удалить запись по индексу
                    DeleteRecord(Students);
                    break;
                case ConsoleKey.D5: // Добавление новой записи
                    AddNewRecord(Students);
                    break;
                case ConsoleKey.Escape: // Закрытие приложения
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine(" Нажмите конкретную кнопку от 1 до 5 или Esc для выхода из приложения.");
                    break;
            }
        }
    }
}
