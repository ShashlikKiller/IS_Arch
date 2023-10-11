
using IS_Arch.ServerProject.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Group = IS_Arch.ServerProject.DataBase.Group;

namespace IS_Arch.BackEnd
{
    internal class StudentBuilder
    {
        public abstract class StudentBuild
        {
            public abstract void AddName(string name, Student CurrentStudent);
            public abstract void AddSurname(string surname, Student CurrentStudent);
            public abstract void AddGroup(int group, Student CurrentStudent, List<Group> Groups);
            public abstract void AddID(Student CurrentStudent, Student prevStudent);
            public abstract void AddStatus(int status, Student CurrentStudent, List<LearningStatus> Statuses);
            public abstract Student GetResult(Student CurrentStudent);
        }

        public class Director
        {
            readonly StudentBuild currentStudentBuilder;
            public Director(StudentBuild builder)
            {
                this.currentStudentBuilder = builder;
            }
            public void AddStudent(string name, string surname, int group, int status, Student CurrentStudent,
                Student prevStudent, List<Group> Groups, List<LearningStatus> Statuses)
            {
                currentStudentBuilder.AddName(name, CurrentStudent);
                currentStudentBuilder.AddSurname(surname, CurrentStudent);
                currentStudentBuilder.AddGroup(group, CurrentStudent, Groups);
                currentStudentBuilder.AddID(CurrentStudent, prevStudent);
                currentStudentBuilder.AddStatus(status, CurrentStudent, Statuses);
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
            public override void AddGroup(int group, Student CurrentStudent, List<Group> Groups)
            {
                CurrentStudent.group_id = group;
                CurrentStudent.Group = Groups.FirstOrDefault(g => g.id == group);
            }
            public override void AddID(Student CurrentStudent, Student prevStudent)
            {
                CurrentStudent.id = prevStudent.id + 1;
            }
            public override void AddStatus(int status, Student CurrentStudent, List<LearningStatus> Statuses)
            {
                CurrentStudent.learningstatus_id = status;
                CurrentStudent.LearningStatus = Statuses.FirstOrDefault(statuses => statuses.id == status);
            }

            public override Student GetResult(Student CurrentStudent)
            {
                return CurrentStudent;
            }
        }
    }
}