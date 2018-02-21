using EFModels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMigrations
{
    [DbConfigurationType(typeof(CustomDbConfiguration))]
    public partial class EFEntities : DbContext
    {
        public EFEntities()
            : base("name=EFEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Person> Person { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
