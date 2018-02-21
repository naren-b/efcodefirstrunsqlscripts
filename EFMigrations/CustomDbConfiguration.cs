using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace EFMigrations
{
    public class CustomDbConfiguration : DbConfiguration
    {
        public CustomDbConfiguration()
        {
            //SetSqlExecutionStrategy();
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }

        private void SetSqlExecutionStrategy()
        {
            int maxTries = 5;
            double maxDelay = 30;

            SetExecutionStrategy("System.Data.SqlClient", () => SuspendExecutionStrategy
                                                                                 ? (IDbExecutionStrategy)new DefaultExecutionStrategy()
                                                                                 : new SqlAzureExecutionStrategy(maxTries, TimeSpan.FromSeconds(maxDelay)));
        }
    }
}
