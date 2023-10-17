using IS_Arch.ServerProject.DataBase;
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
                    += $"Student with ID = {searchID}\n"
                    + ConsoleOutputSingle(CurrentStudent);
                    return answer;
                }
            }
            return "No students with this ID were found.\n";
        }

        public static string GetName(Student student)
        {
            return student.name;
        }

        public static string GetSurname(Student student)
        {
            return student.surname;
        }

        public static string GetFullName(Student student)
        {
            return GetName(student) + GetSurname(student);
        }

        public static string GetStudentForList(Student student)
        {
            return GetUID(student) + GetSurname(student);
        }
        
        public static string GetUID(Student student)
        {
            return student.id.ToString();
        }

        public static string GetLearningStatus(Student student)
        {
            return student.LearningStatus.status;
        }

        public static string GetGroup(Student student)
        {
            return student.Group.name;
        }

        public static string DeleteRecord(List<Student> Students, dbEntities db, int delete_id)
        {
            foreach (Student item in Students)
            {
                if (item.id == delete_id)
                {
                    Students.Remove(item);
                    db.Students.Remove(item);
                    return "The deletion was successful.\n";
                }
            }
            return "Something went wrong.\n";
        }

        public static string ConsoleOutputSingle(Student student)
        {
            string answer = "";
            answer
            += $"\nStudent ID: {student.id} \n"
            + $"Name: {student.name} {student.surname}\n"
            + $"Group: {student.Group.name}, Learning status: {student.LearningStatus.status}\n";
            return answer;
        }

    }
}
