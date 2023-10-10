namespace IS_Arch.BackEnd
{
        public class Student
        {
            private string name;
            private string surname;
            private string group;
            private uint student_id;
            private bool learningstatus;
            public Student(string name, string surname, string group, uint student_id, bool learningstatus = true)
            {
                this.name = name;
                this.surname = surname;
                this.group = group;
                this.student_id = student_id;
                this.learningstatus = learningstatus;
            }
            public Student()
            {

            }
            #region Getters And Setters
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                }
            }
            public string Surname
            {
                get
                {
                    return surname;
                }
                set
                {
                    surname = value;
                }
            }
            public string Group
            {
                get
                {
                    return group;
                }
                set
                {
                    group = value;
                }
            }
            public uint Student_id
            {
                get
                {
                    return student_id;
                }
                set
                {
                    student_id = value;
                }
            }
            public bool LearningStatus
            {
                get
                {
                    return learningstatus;
                }
                set
                {
                    learningstatus = value;
                }
            }
            #endregion
        }
    }