using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Arch.BackEnd
{
    internal class DataManipulation
    {
        // Разработанное приложение должно обеспечивать следующие возможности:
        //   1. Вывод всех записей на экран +
        //   2. Вывод записи по номеру +
        //   3. Запись данных в файл +
        //   4. Удаление записи (записей) из файла +
        //   5. Добавление записи в файл +
        public static string OutputByID(List<Student> Students, int searchID)
        {
            string answer = "";
            for (int i = 0; i < Students.Count(); i++)
            {
                Student CurrentStudent = Students.ElementAt(i);
                if (CurrentStudent.Student_id == searchID)
                {
                    answer += $" Student with ID = {searchID} \n";
                    answer += ConsoleOutputSingle(CurrentStudent);
                    return answer;
                }
            }
            return "No students with this ID were found.";
        }
        
        public static string ConsoleOutputAll(List<Student> Students)
        {
            string answer = "";
            foreach (Student student in Students)
            {
                answer += ConsoleOutputSingle(student);
            }
            return answer;
        }

        public static string DeleteRecord(List<Student> Students, int delete_id)
        {
            foreach (Student item in Students)
            {
                if (item.Student_id == delete_id)
                {
                    Students.Remove(item);
                    return "The deletion was successful.\n";
                }
            }
            return "Something went wrong.\n";
        }

        public static string ConsoleOutputSingle(Student student)
        {
            string answer = "";
            answer += $"\n Student ID: {student.Student_id} \n";
            answer += $" Name: {student.Name} {student.Surname}\n";
            answer += $" Group: {student.Group}, Still learning - {student.LearningStatus}\n";
            return answer;
        }
    }
}
