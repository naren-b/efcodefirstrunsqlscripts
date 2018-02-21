namespace EFMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2ndMigration : DbMigrationBase
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Email", c => c.String(maxLength: 255));
            AddColumn("dbo.People", "PrimaryPhone", c => c.String(maxLength: 100));
            AddColumn("dbo.People", "SecondaryPhone", c => c.String(maxLength: 100));
            ExecuteSqlFilesWithinMigrationsFolder(@"2nd");
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "SecondaryPhone");
            DropColumn("dbo.People", "PrimaryPhone");
            DropColumn("dbo.People", "Email");
        }
    }
}
