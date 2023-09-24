namespace IS_Arch.BackEnd
{
    internal class StudentBuilder
    {
        public abstract class StudentBuild
        {
            public abstract void AddName(string name, Student CurrentStudent);
            public abstract void AddSurname(string surname, Student CurrentStudent);
            public abstract void AddGroup(string group, Student CurrentStudent);
            public abstract void AddID(uint id, Student CurrentStudent);
            public abstract void AddLearningStatus(bool learningstatus, Student CurrentStudent);
            public abstract Student GetResult(Student CurrentStudent); // Собрать
        }

        public class Director
        {
            StudentBuild currentStudentBuilder;
            public Director(StudentBuild builder)
            {
                this.currentStudentBuilder = builder;
            }
            public void AddWord(string name, string surname, string group, uint id, bool learningstatus, Student CurrentStudent)
            {
                currentStudentBuilder.AddName(name, CurrentStudent);
                currentStudentBuilder.AddSurname(surname, CurrentStudent);
                currentStudentBuilder.AddGroup(group, CurrentStudent);
                currentStudentBuilder.AddID(id, CurrentStudent);
                currentStudentBuilder.AddLearningStatus(learningstatus, CurrentStudent);
            }
        }
        public class ConcreteBuilder : StudentBuild
        {
            public override void AddName(string name, Student CurrentStudent)
            {
                CurrentStudent.Name = name;
            }
            public override void AddSurname(string surname, Student CurrentStudent)
            {
                CurrentStudent.Surname = surname;
            }
            public override void AddGroup(string group, Student CurrentStudent)
            {
                CurrentStudent.Name = group;
            }
            public override void AddID(uint id, Student CurrentStudent)
            {
                CurrentStudent.Student_id = id;
            }
            public override void AddLearningStatus(bool learningstatus, Student CurrentStudent)
            {
                CurrentStudent.LearningStatus = learningstatus;
            }

            public override Student GetResult(Student CurrentStudent)
            {
                return CurrentStudent;
            }
        }
    }
}