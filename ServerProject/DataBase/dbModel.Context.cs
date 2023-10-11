namespace IS_Arch.ServerProject.DataBase
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbEntities : DbContext
    {
        public dbEntities()
            : base("name=dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<LearningStatus> LearningStatuses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
    }
}
