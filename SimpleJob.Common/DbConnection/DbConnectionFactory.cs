using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimpleQuartz.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleQuartz.Common.DbConnection
{
    public class DbConnectionFactory
    {
        public DbConnectionFactory()
        {
        }

        public IDbConnection CreateConnectionByEnum(EnumDbConnection enumConnection)
        {
            var name = nameof(enumConnection);
            var connectionString = GetConnectionString(name);
            return new SqlConnection(connectionString);
        }

        private string GetConnectionString(string name)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            return config.GetConnectionString(name);
        }
    }
}
