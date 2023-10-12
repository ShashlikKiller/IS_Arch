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
        #region Receive/Send data methods
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
        #endregion

        #region Async server's commands
        public static async Task<string> OutputAllAsync(List<Student> Students) // Вывод всех записей
        {
            return await Task.Run(() => ConsoleOutputAll(Students));
        }

        public static async Task<string> OutputByIDAsync(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students) // Вывод 1 записи по ID
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
            return IDError;
        }
        public static async Task<string> RecordsSaveAsync(List<Student> newStudents, dbEntities db) // Запись данных в файл
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


        public static async Task<string> DeleteRecordByIDAsync(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students, dbEntities db) // Удалить запись по номеру
        {
            string server_answer;
            string data;
            server_answer = "Enter the ID:\n";
            await SendDataAsync(udpSocket, senderEndPoint, server_answer); // Отправляем клиенту сообщение
            data = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
            if (TypeCheck.IntCheckBool(data))
            {
                return await Task.Run(() => DeleteRecord(Students, db, Convert.ToInt32(data)).ToString());
            }
            return IDError;
        }

        public static async Task<string> AddNewRecordAsync(Socket udpSocket, EndPoint senderEndPoint, List<Student> Students, List<Student> newStudents, List<Group> Groups, List<LearningStatus> Statuses) // Добавить запись
        {
            Student varStudent = new Student();
            // Используем паттерн строитель для создания новой сущности
            ConcreteBuilder studentBuilder = new ConcreteBuilder();
            try
            {
                bool exit = false;
                await SendDataAsync(udpSocket, senderEndPoint, " Enter the name:\n");
                studentBuilder.AddName(ReceiveDataAsync(udpSocket, senderEndPoint).Result, varStudent); // TODO: посмотри что будент если senddata w/o await
                await SendDataAsync(udpSocket, senderEndPoint, " Enter the surname:\n");
                studentBuilder.AddSurname(ReceiveDataAsync(udpSocket, senderEndPoint).Result, varStudent);
                MinAndMaxValues MinAndMaxGroupValues = new MinAndMaxValues();
                MinAndMaxGroupValues.StatusIDGetMaxAndMin(Statuses);
                while (!exit)
                {
                    await SendDataAsync(udpSocket, senderEndPoint, " Enter the group ID:\n");
                    string group_id = await ReceiveDataAsync(udpSocket, senderEndPoint);
                    if (TypeCheck.IntCheckBool(group_id))
                    {
                        int group_id_int = Convert.ToInt32(group_id);
                        if (group_id_int > MinAndMaxGroupValues.Min && group_id_int < MinAndMaxGroupValues.Max)
                        {
                            await Task.Run(() => studentBuilder.AddGroup(Convert.ToInt32(group_id), varStudent, Groups));
                            exit = true;
                        }
                        else SendDataAsync(udpSocket, senderEndPoint, OUTofbounce);
                    }
                    else SendDataAsync(udpSocket, senderEndPoint, IDError);
                }
                studentBuilder.AddID(varStudent, Students.Last()); // Берем id у последнего студента (это все равно не имеет смысла, т.к. при сохранении SQL делает здесь автоинкримент)
                exit = false;
                MinAndMaxValues MinAndMaxStatusValues = new MinAndMaxValues();
                MinAndMaxStatusValues.StatusIDGetMaxAndMin(Statuses);
                while (!exit)
                {
                    await SendDataAsync(udpSocket, senderEndPoint, " Enter the student's learning status ID.\n");
                    string status_id = await ReceiveDataAsync(udpSocket, senderEndPoint);
                    if (TypeCheck.IntCheckBool(status_id))
                    {
                        int status_id_int = Convert.ToInt32(status_id);
                        if (status_id_int > MinAndMaxStatusValues.Min && status_id_int < MinAndMaxStatusValues.Max)
                        {
                            await Task.Run(() => studentBuilder.AddStatus(Convert.ToInt32(status_id), varStudent, Statuses));
                            exit = true;
                        }
                        else SendDataAsync(udpSocket, senderEndPoint, OUTofbounce); // no await
                    }
                    else SendDataAsync(udpSocket, senderEndPoint, IDError); // no await
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
        #endregion
        private static readonly string IDError = "\nInvalid ID format. Try again.\n";
        private static readonly string OUTofbounce = "\nThis ID is out of bounce. Please, select the id from the min allowed to the max.\n";
    }
}
