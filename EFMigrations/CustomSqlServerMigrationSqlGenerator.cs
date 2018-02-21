using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace EFMigrations
{
    public class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
	{
        private int dropConstraintCount = 0;
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetAuditColumnDefaults(addColumnOperation.Column, addColumnOperation.Table);

            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetAuditColumnDefaults(createTableOperation.Columns, createTableOperation.Name);

            base.Generate(createTableOperation);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation)
        {
            SetAuditColumnDefaults(alterColumnOperation.Column, alterColumnOperation.Table);
            base.Generate(alterColumnOperation);
        }

        protected override void Generate(AlterTableOperation alterTableOperation)
        {
            SetAuditColumnDefaults(alterTableOperation.Columns, alterTableOperation.Name);
            base.Generate(alterTableOperation);
        }

        private void SetAuditColumnDefaults(IEnumerable<ColumnModel> columns, string tableName)
        {
            foreach (var columnModel in columns)
            {
                SetAuditColumnDefaults(columnModel, tableName);
            }
        }

        private void HandleSqlDefaultValueAttribute(ColumnModel column, string tableName)
        {
            AnnotationValues values;
            if (column.Annotations.TryGetValue("SqlDefaultValue", out values))
            {
                if (values.NewValue == null)
                {
                    column.DefaultValueSql = null;
                    using (var writer = Writer())
                    {
                        // Drop Constraint
                        writer.WriteLine(GetSqlDropConstraintQuery(tableName, column.Name));
                        Statement(writer);
                    }
                }
                else
                {

                    column.DefaultValueSql = values.NewValue.ToString();
                }
            }
        }

        private void HandleDefaultValueAttribute(ColumnModel column, string tableName)
        {
            AnnotationValues values;
            if (column.Annotations.TryGetValue("DefaultValue", out values))
            {
                if (values.NewValue == null)
                {
                    column.DefaultValue = null;
                    using (var writer = Writer())
                    {
                        // Drop Constraint
                        writer.WriteLine(GetSqlDropConstraintQuery(tableName, column.Name));
                        Statement(writer);
                    }
                }
                else
                {
                    column.DefaultValue = values.NewValue;
                }
            }
        }
        private void SetAuditColumnDefaults(ColumnModel column, string tableName)
        {
            HandleSqlDefaultValueAttribute(column, tableName);
            HandleDefaultValueAttribute(column, tableName);

            List<string> dateTimeColumsns = new List<string> { "UpdatedAt", "CreatedAt" };
            List<string> booleanColumnsWithDefaultTrue = new List<string> {"IsActive"};
            List<string> booleanColumnsWithDefaultFalse = new List<string> { "IsDeleted" };
            List<string> guidColumns = new List<string> { };
          
            if (guidColumns.Any(col => string.Compare(column.Name, col, System.StringComparison.InvariantCultureIgnoreCase) == 0)
                && column.Type == PrimitiveTypeKind.Guid)
            {
                column.DefaultValueSql = "NEWID()";
            }

            if (dateTimeColumsns.Any(col => string.Compare(column.Name, col, System.StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                column.DefaultValueSql = "GETUTCDATE()";
            }

            if (booleanColumnsWithDefaultTrue.Any(col => string.Compare(column.Name, col, System.StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                column.DefaultValue = "1";
            }

            if (booleanColumnsWithDefaultFalse.Any(col => string.Compare(column.Name, col, System.StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                column.DefaultValue = "0";
            }
        }

        private string GetSqlDropConstraintQuery(string tableName, string columnName)
        {
            var str = $@"DECLARE @var{dropConstraintCount} nvarchar(128)
SELECT @var{dropConstraintCount} = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'{tableName}')
AND col_name(parent_object_id, parent_column_id) = '{columnName}';
IF @var{dropConstraintCount} IS NOT NULL
    EXECUTE('ALTER TABLE {tableName} DROP CONSTRAINT [' + @var{dropConstraintCount} + ']')";

            dropConstraintCount = dropConstraintCount + 1;
            return str;
        }
    }
}
