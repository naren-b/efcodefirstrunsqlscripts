namespace EFMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigrationBase
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        MiddleName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        DateOfBirth = c.DateTime(nullable: false),
                        IsActive = c.Boolean(),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            ExecuteSqlFilesWithinMigrationsFolder("Initial");
        }
        
        public override void Down()
        {
            DropTable("dbo.People");
        }
    }
}
