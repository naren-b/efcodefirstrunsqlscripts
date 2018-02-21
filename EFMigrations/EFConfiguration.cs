using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMigrations
{
    public sealed class EFConfiguration : DbMigrationsConfiguration<EFEntities>
    {
        public EFConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(EFEntities context)
        {
            new DbMigrationBase(context).ExecuteSqlFilesWithinMigrationsFolder(@"2nd");

            context.SaveChanges();
        }

    }
}
