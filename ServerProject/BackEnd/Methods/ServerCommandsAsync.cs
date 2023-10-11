using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static IS_Arch.BackEnd.StudentBuilder;
using static IS_Arch.BackEnd.Methods.DataManipulation;
using System.Linq;
using IS_Arch.ServerProject.DataBase;

namespace IS_Arch.BackEnd.Methods
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
            await Task.Run(() => SendData(udpSocket, senderEndPoint, server_answer));
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
        public static async Task<string> case3(List<Student> newStudents, dbEntities db) // Запись данных в файл
        {
            await Task.Run(() =>
            {
                foreach (Student student in newStudents)
                {
                    db.Students.Add(student);
                }
            });
            return await Task.Run(() => $"Amount of saved records: {db.SaveChangesAsync().Result}\n");
        }


        public static async Task<string> case4(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students, dbEntities db) // Удалить запись по номеру
        {
            string server_answer;
            string data;
            server_answer = "Enter the ID";
            await SendDataAsync(udpSocket, senderEndPoint, server_answer); // Отправляем клиенту сообщение
            data = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
            if (TypeCheck.IntCheckBool(data))
            {
                return await Task.Run(() => DeleteRecord(Students, db, Convert.ToInt32(data)).ToString());
            }
            return "invalid ID format";
        }

        public static async Task<string> case5(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students, List<Student> newStudents, List<Group> Groups, List<LearningStatus> Statuses) // Добавить запись
        {
            Student varStudent = new Student();
            // Используем паттерн строитель для создания новой сущности
            ConcreteBuilder studentBuilder = new ConcreteBuilder();
            try
            {
                bool exit = false;
                await SendDataAsync(udpSocket, senderEndPoint, " Enter the name:");
                studentBuilder.AddName(ReceiveDataAsync(udpSocket, senderEndPoint).Result, varStudent); // TODO: посмотри что будент если senddata w/o await
                await SendDataAsync(udpSocket, senderEndPoint, " Enter the surname:");
                studentBuilder.AddSurname(ReceiveDataAsync(udpSocket, senderEndPoint).Result, varStudent);
                while (!exit)
                {
                    await SendDataAsync(udpSocket, senderEndPoint, " Enter the group ID:");
                    string group_id = await ReceiveDataAsync(udpSocket, senderEndPoint);
                    if (TypeCheck.IntCheckBool(group_id))
                    {
                        await Task.Run(() => studentBuilder.AddGroup(Convert.ToInt32(group_id), varStudent, Groups));
                        exit = true;
                    }
                    else await SendDataAsync(udpSocket, senderEndPoint, " Invalid group ID format. Try again.");
                }
                studentBuilder.AddID(varStudent, Students.Last()); // Берем id у последнего студента
                exit = false;
                while (!exit)
                {
                    await SendDataAsync(udpSocket, senderEndPoint, " Enter the student's learning status ID.");
                    string status_id = await ReceiveDataAsync(udpSocket, senderEndPoint);
                    if (TypeCheck.IntCheckBool(status_id))
                    {
                        await Task.Run(() => studentBuilder.AddStatus(Convert.ToInt32(status_id), varStudent, Statuses));
                        exit = true;
                    }
                    else await SendDataAsync(udpSocket, senderEndPoint, "Invalid ID format. Try again."); // Тут ошибка из-за парса постоянно 
                }
                Students.Add(studentBuilder.GetResult(varStudent));
                newStudents.Add(studentBuilder.GetResult(varStudent));
                return "Success. Student added.\n";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}\n";
            }
        }
    }
}
