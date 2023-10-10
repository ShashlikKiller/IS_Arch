using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Arch.DataBase.Models
{
    public class LearningStatus
    {
        private int id;
        private string status;
        public int Id { get { return id; } }
        public string Status { get { return status; } }
        public LearningStatus() { }
        public LearningStatus(int id, string status)
        {
            this.id = id;
            this.status = status;
        }
    }
}
