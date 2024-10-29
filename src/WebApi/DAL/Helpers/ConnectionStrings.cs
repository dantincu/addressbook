using Common.Database;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public static class ConnectionStrings
    {
        public const string CONNECTION_STRING_NAME_CFG_KEY = "ConnectionStringName";
        public const string CONNECTION_STRING_NAME_EVN_VAR = "CONNECTION_STRING_NAME";

        public static string CreateConnectionString()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine("..", "..", "..", "Data", "data.db"),
                Mode = SqliteOpenMode.ReadWriteCreate
            };

            string connectionString = connectionStringBuilder.ToString();
            return connectionString;
        }

        public static DbConnectionType GetDbConnectionType(
            IConfiguration config)
        {
            DbConnectionType preferredConnectionStringType;

            string? preferredConnectionStringName = Environment.GetEnvironmentVariable(
                    CONNECTION_STRING_NAME_EVN_VAR) ?? config.GetValue<string>(
                CONNECTION_STRING_NAME_CFG_KEY) ?? throw new InvalidOperationException(
                        string.Join(" ", $"The connection string name is required but not present",
                            $"in either the {CONNECTION_STRING_NAME_EVN_VAR} environment variable",
                            $"or in the {CONNECTION_STRING_NAME_CFG_KEY} key in appsettings.json file."));

            if (!Enum.TryParse(
                preferredConnectionStringName,
                out preferredConnectionStringType))
            {
                var allowedValues = Enum.GetValues<DbConnectionType>(
                    ).ToUserFriendlyEnumeration(
                        null, ", ", " or ");

                throw new InvalidOperationException(string.Join(" ",
                    $"The connection string name must have",
                    $"one of the following values: {allowedValues}."));
            }

            return preferredConnectionStringType;
        }
    }
}
