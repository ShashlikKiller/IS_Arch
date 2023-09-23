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
        public static void OutputByID(List<Student> Students)
        {
            uint searchID = TypeCheck.IntCheck(); // СДЕЛАТЬ ПРОВЕРКУ НА ИНТ
            for (int i = 0; i < Students.Count(); i++)
            {
                Student CurrentStudent = Students.ElementAt(i);
                if (CurrentStudent.Student_id == searchID)
                {
                    Console.WriteLine($" Student with ID = {searchID}");
                    ConsoleOutputSingle(CurrentStudent);
                    break;
                }
            }
        }
        public static void MenuText()
        {
            Console.Write(" Выберите то, что вы хотите сделать, и нажмите соответствующую кнопку :\n");
            Console.Write("   1. Вывод всех данных на экран\n");
            Console.Write("   2. Вывод карточки студента по идентификатору\n");
            Console.Write("   3. Сохранение всех записей в файл\n");
            Console.Write("   4. Удалить запись по ID студента\n");
            Console.Write("   5. Добавить новую запись\n");
            Console.Write("   Esc. Закрытие\n");
        }
        public static void ConsoleOutputAll(List<Student> Students)
        {
            foreach (Student student in Students)
            {
                ConsoleOutputSingle(student);
            }
        }

        public static void AddNewRecord(List<Student> Students)
        {
            Console.WriteLine(" Введите имя:");
            string varname = TypeCheck.StringCheck();
            Console.WriteLine(" Введите фамилию: ");
            string varsurname = TypeCheck.StringCheck();
            Console.WriteLine(" Введите название группы: ");
            string vargroup = TypeCheck.StringCheck();
            Console.WriteLine(" Введите ID студента: ");
            uint varstudent_id = TypeCheck.IntCheck(); // Сделать проверку на дурака
            Console.WriteLine(" Введите статус обучения (1 - Да, 2 - Нет): ");
            bool varlearningstatus = TypeCheck.BoolCheck();
            Student varstudent = new Student(varname, varsurname, vargroup, varstudent_id, varlearningstatus);
            Students.Add(varstudent);
        }

        public static void DeleteRecord(List<Student> Students)
        {
            Console.WriteLine(" Введите индекс записи, которую хотите удалить.");
            int delete_id = int.Parse(Console.ReadLine());
            foreach (Student item in Students)
            {
                if (item.Student_id == delete_id)
                {
                    Students.Remove(item);
                    break;
                }
            }
        }

        public static void ConsoleOutputSingle(Student student)
        {
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine($" ID: {student.Student_id}");
            Console.WriteLine($" Name: {student.Name} {student.Surname}");
            Console.WriteLine($" Group: {student.Group}, Still learning - {student.LearningStatus}");
            Console.WriteLine("-----------------------------------------------------------------");
        }
    }
}
