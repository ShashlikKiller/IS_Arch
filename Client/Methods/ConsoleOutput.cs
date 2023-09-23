using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Methods
{
    internal class ConsoleOutput
    {
        public static string MenuText()
        {
            string answer = "";
            answer += " Выберите то, что вы хотите сделать, и нажмите соответствующую кнопку :\n";
            answer += "   1. Вывод всех данных на экран\n";
            answer += "   2. Вывод карточки студента по идентификатору\n";
            answer += "   3. Сохранение всех записей в файл\n";
            answer += "   4. Удалить запись по ID студента\n";
            answer += "   5. Добавить новую запись\n";
            answer += "   Esc. Закрытие\n";
            return answer;
        }
    }
}
