using IS_Arch.ServerProject.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IS_Arch.BackEnd.Methods
{
    public class DataManipulation
    {
        // Разработанное приложение должно обеспечивать следующие возможности:
        //   1. Вывод всех записей на экран 
        //   2. Вывод записи по номеру 
        //   3. Запись данных в файл 
        //   4. Удаление записи (записей) из файла 
        //   5. Добавление записи в файл 
        public static string OutputByID(List<Student> Students, int searchID)
        {
            string answer = "";
            for (int i = 0; i < Students.Count(); i++)
            {
                Student CurrentStudent = Students.ElementAt(i);
                if (CurrentStudent.id == searchID)
                {
                    answer 
                    += $" Student with ID = {searchID}\n"
                    + ConsoleOutputSingle(CurrentStudent);
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
                if (item.id == delete_id)
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
            answer
            += $"\n Student ID: {student.id} \n"
            + $" Name: {student.name} {student.surname}\n"
            + $" Group: {student.Group.name}, Still learning - {student.LearningStatus.status}\n";
            return answer;
        }

    }
}
