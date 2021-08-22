using Calculo_Biorritmo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculo_Biorritmo.Data
{
    class EmployeeEntity : DbContext
    {
        public EmployeeEntity() : base("BiorytmDb")
        {

        }

        public DbSet<employee> employees { get; set; }
        public DbSet<accident> accidents { get; set; }
        public DbSet<PendingSync> pendingSyncs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<employee>()
                .HasIndex(e => e.curp)
                .IsUnique();
        }
    }
}
