
namespace IS_Arch.ServerProject.DataBase
{
    using System.Collections.Generic;
    
    public partial class LearningStatus
    {
        public LearningStatus()
        {
            this.Student = new HashSet<Student>();
        }
    
        public int id { get; set; }
        public string status { get; set; }
    
        public virtual ICollection<Student> Student { get; set; }

        public LearningStatus(int id, string status)
        {
            this.id = id;
            this.status = status;
        }
    }
}
