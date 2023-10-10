using IS_Arch.BackEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Arch.DataBase.Models
{
    public class Group
    {
        private int id;
        private string name;
        public int Id { get { return id; } }
        public string Name { get { return name; } }

        public virtual ICollection<Student> StudentsInGroup
        { get; set; }

        public Group(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public Group()
        {
        }
    }
}
