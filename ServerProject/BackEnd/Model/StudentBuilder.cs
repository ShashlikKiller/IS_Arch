
using IS_Arch.ServerProject.DataBase;
using System.Collections.Generic;
using System.Linq;

namespace IS_Arch.BackEnd
{
    internal class StudentBuilder
    {
        public abstract class StudentBuild
        {
            public abstract void AddName(string name, Student CurrentStudent);
            public abstract void AddSurname(string surname, Student CurrentStudent);
            public abstract void AddGroup(int group, Student CurrentStudent);
            public abstract void AddID(Student CurrentStudent, Student prevStudent);
            public abstract void AddStatus(int status, Student CurrentStudent);
            public abstract Student GetResult(Student CurrentStudent);
        }

        public class Director
        {
            readonly StudentBuild currentStudentBuilder;
            public Director(StudentBuild builder)
            {
                this.currentStudentBuilder = builder;
            }
            public void AddStudent(string name, string surname, int group, int status, Student CurrentStudent, Student prevStudent)
            {
                currentStudentBuilder.AddName(name, CurrentStudent);
                currentStudentBuilder.AddSurname(surname, CurrentStudent);
                currentStudentBuilder.AddGroup(group, CurrentStudent);
                currentStudentBuilder.AddID(CurrentStudent, prevStudent);
                currentStudentBuilder.AddStatus(status, CurrentStudent);
            }
        }
        public class ConcreteBuilder : StudentBuild
        {
            public override void AddName(string name, Student CurrentStudent)
            {
                CurrentStudent.name = name;
            }
            public override void AddSurname(string surname, Student CurrentStudent)
            {
                CurrentStudent.surname = surname;
            }
            public override void AddGroup(int group, Student CurrentStudent)
            {
                CurrentStudent.group_id = group;    
            }
            public override void AddID(Student CurrentStudent, Student prevStudent)
            {
                CurrentStudent.id = prevStudent.id + 1;
            }
            public override void AddStatus(int status, Student CurrentStudent)
            {
                CurrentStudent.learningstatus_id = status;
            }

            public override Student GetResult(Student CurrentStudent)
            {
                return CurrentStudent;
            }
        }
    }
}