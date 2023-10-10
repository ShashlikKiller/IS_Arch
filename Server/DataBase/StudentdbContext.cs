using IS_Arch.BackEnd;
using IS_Arch.DataBase.Models;
using System.Data.Entity;
using System.Text.RegularExpressions;
using Group = IS_Arch.DataBase.Models.Group;

namespace IS_Arch.DataBase
{
    public class StudentdbContext : DbContext
    {
        public StudentdbContext() : base("dbConnectionString")
        { 
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<LearningStatus> LearningStatuses { get; set; } 
    }
}
