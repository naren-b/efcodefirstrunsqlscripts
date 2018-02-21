namespace EFMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Web;

    public class DbMigrationBase : DbMigration
    {
        #region <Fields & Constants>

        protected const string MIGRATION_SCRIPTS_BASE_DIRECTORY = @"Migrations";
        protected readonly DbContext context;

        #endregion

        #region Constructors
        public DbMigrationBase()
        {
        }

        public DbMigrationBase(DbContext context)
        {
            this.context = context;
        }
        #endregion

        #region Properties
        public string MigrationsRootFolderPath
        {
            get
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                if (HttpContext.Current != null)
                    baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                return Path.Combine(baseDirectory, MIGRATION_SCRIPTS_BASE_DIRECTORY);

            }
        }
        #endregion

        #region  Override Methods

        public override void Up()
        {
        }
        #endregion

        #region <Methods>
        internal void ExecuteSqlFilesWithinMigrationsFolder(string migrationsFolderName)
        {
            if (migrationsFolderName.StartsWith(@"\"))
                migrationsFolderName = migrationsFolderName.Substring(1);

            var fullFolderPath = Path.Combine(MigrationsRootFolderPath, migrationsFolderName);
            RecursivelyExecuteSqlFilesInFolder(fullFolderPath);
        }

        private void RecursivelyExecuteSqlFilesInFolder(string fullFolderPath)
        {
            if (!Directory.Exists(fullFolderPath))
            {
                return;
            }

            var files = Directory.EnumerateFiles(fullFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                 .Where(f => f.ToLowerInvariant().EndsWith(".sql")).OrderBy(f => f);

            foreach (var file in files)
            {
                ExecuteSqlScriptInFile(file);
            }

            Directory.EnumerateDirectories(fullFolderPath).OrderBy(d => d).ToList().ForEach(directory =>
                {
                    RecursivelyExecuteSqlFilesInFolder(directory);
                }
            );
        }

        private void ExecuteSqlScriptInFile(string filePath)
        {
            var query = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(query))
                return;
            ExecuteSqlScript(query);
        }

        private void ExecuteSqlScript(string sqlScript)
        {
            //Split the scripts by GO statement and execute the scripts. This will be helpful scenarios like scripts with huge seed data. To avoid connection issues, instead split the single file into multiple files
            //Just add a go statement in every 300 inserts
            sqlScript = sqlScript.Replace("\r\n\tgo", "\r\nGO");
            sqlScript = sqlScript.Replace("\r\n\tGo", "\r\nGO");
            sqlScript = sqlScript.Replace("\r\n\tgO", "\r\nGO");
            sqlScript = sqlScript.Replace("\r\n\tGO", "\r\nGO");

            sqlScript = sqlScript.Replace("go\t\r\n", "GO\r\n");
            sqlScript = sqlScript.Replace("Go\t\r\n", "GO\r\n");
            sqlScript = sqlScript.Replace("gO\t\r\n", "GO\r\n");
            sqlScript = sqlScript.Replace("GO\t\r\n", "GO\r\n");

            sqlScript = sqlScript.Replace("\r\ngo\r\n", "\r\nGO\r\n");
            sqlScript = sqlScript.Replace("\r\nGo\r\n", "\r\nGO\r\n");
            sqlScript = sqlScript.Replace("\r\ngO\r\n", "\r\nGO\r\n");

            string[] sql = sqlScript.Split(new[] { "\r\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var sqlCommand in sql)
            {
                var sqlToRun = sqlCommand;
                if (!string.IsNullOrWhiteSpace(sqlToRun))
                {
                    if (sqlToRun.ToLowerInvariant().EndsWith("go"))
                        sqlToRun = sqlToRun.Substring(0, sqlCommand.Length - 2);

                    if (sqlToRun.ToLowerInvariant().StartsWith("go"))
                        sqlToRun = sqlToRun.Substring(2);

                    if (context != null)
                    {
                        context.Database.ExecuteSqlCommand(sqlToRun);
                    }
                    else
                    {
                        Sql(sqlToRun);
                    }
                }
            }
        }

        #endregion
    }
}
