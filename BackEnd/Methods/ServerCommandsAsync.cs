using NLog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static IS_Arch.BackEnd.DataManipulation;
using static IS_Arch.BackEnd.StudentBuilder;

namespace IS_Arch.BackEnd
{
    public class ServerCommandsAsync
    {
        public static string ReceiveData(Socket udpSocket, EndPoint senderEndPoint)
        {
            string data;
            int size;
            byte[] buffer = new byte[256];
            size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
            data = Encoding.UTF8.GetString(buffer, 0, size);
            return data;
        }

        public static async Task<string> ReceiveDataAsync(Socket udpSocket, EndPoint senderEndPoint)
        {
            return await Task.Run(() => ReceiveData(udpSocket, senderEndPoint));
        }

        public static void SendData(Socket udpSocket, EndPoint senderEndPoint, string server_answer)
        {
            byte[] responseBuffer;
            responseBuffer = Encoding.UTF8.GetBytes(server_answer);
            udpSocket.SendTo(responseBuffer, SocketFlags.None, senderEndPoint);
        }

        public static async Task SendDataAsync(Socket udpSocket, EndPoint senderEndPoint, string server_answer)
        {
            await Task.Run(() => SendData(udpSocket, senderEndPoint, server_answer)); // TODO: async
        }

        public static async Task<string> case1(List<Student> Students) // Вывод всех записей
        {
            return await Task.Run(() => ConsoleOutputAll(Students));
        }

        public static async Task<string> case2(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students) // Вывод 1 записи по ID
        {
            string server_answer;
            string data;
            server_answer = "Enter the ID";
            await SendDataAsync(udpSocket, senderEndPoint, server_answer); // Отправляем клиенту сообщение
            data = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
            if (TypeCheck.IntCheckBool(data))
            {
                return await Task.Run(() => OutputByID(Students, Convert.ToInt32(data)).ToString());
            }
            return "invalid ID format";
        }
        public static async Task<string> case3(List<Student> Students, string path) // Запись данных в файл
        {
            return await Task.Run(() => SaveRecords(Students, path));
        }

        public static async Task<string> case4(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students) // Удалить запись по номеру
        {
            string server_answer;
            string data;
            server_answer = "Enter the ID";
            await SendDataAsync(udpSocket, senderEndPoint, server_answer); // Отправляем клиенту сообщение
            data = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
            if (TypeCheck.IntCheckBool(data))
            {
                return await Task.Run(()=> DeleteRecord(Students, Convert.ToInt32(data)).ToString());
            }
            return "invalid ID format";
        }

        public static async Task<string> case5(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students) // Добавить запись
        {
            Student varStudent = new Student();
            // Используем паттерн строитель для создания новой сущности
            ConcreteBuilder studentBuilder = new ConcreteBuilder();
            try // Почему try такой большой?
            {
                await SendDataAsync(udpSocket, senderEndPoint, " Enter the name:");
                studentBuilder.AddName(ReceiveDataAsync(udpSocket, senderEndPoint).Result, varStudent); // TODO: посмотри что будент если senddata w/o await
                await SendDataAsync(udpSocket, senderEndPoint, " Enter the surname:");
                studentBuilder.AddSurname(ReceiveDataAsync(udpSocket, senderEndPoint).Result, varStudent);
                await SendDataAsync(udpSocket, senderEndPoint, " Enter the group:");
                studentBuilder.AddGroup(ReceiveDataAsync(udpSocket, senderEndPoint).Result, varStudent);
                bool exit = false;
                while (!exit) // Вход на ввод ID студента (проверка на int)
                {
                    await SendDataAsync(udpSocket, senderEndPoint, " Enter the ID:");
                    string newid = await ReceiveDataAsync(udpSocket, senderEndPoint);
                    if (TypeCheck.IntCheckBool(newid))
                    {
                        await Task.Run(() => studentBuilder.AddID(Convert.ToUInt32(newid), varStudent));
                        exit = true;
                    }
                    else await SendDataAsync(udpSocket, senderEndPoint, "Invalid ID format. Try again.");
                }
                exit = false;
                while (!exit) // Вход на ввод статуса обучения студента (проверка на bool)
                {
                    await SendDataAsync(udpSocket, senderEndPoint, " Enter the student's learning status. (1 = True, 2 = False):\n");
                    string newbool = await ReceiveDataAsync(udpSocket, senderEndPoint);
                    if (TypeCheck.BoolCheck(newbool))
                    {
                        await Task.Run(() => studentBuilder.AddLearningStatus(Convert.ToBoolean(ReceiveData(udpSocket, senderEndPoint)), varStudent));
                        exit = true;
                    }
                    else SendDataAsync(udpSocket, senderEndPoint, "Invalid BOOL format. Try again."); // Тут ошибка из-за парса постоянно 
                }
                Students.Add(studentBuilder.GetResult(varStudent));
                return "Success. Student added.\n";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message + "\n";
            }
        }
    }
}
