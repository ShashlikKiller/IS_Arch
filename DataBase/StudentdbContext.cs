using System.Data.Entity;

namespace IS_Arch.DataBase
{
    public class StudentdbContext : DbContext
    {
        protected StudentdbContext() : base("dbConnectionString")
        { 
        }
    }
}
