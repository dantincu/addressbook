using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public static class SqlCode
    {
        public static string EncodeSqlStrValue(
            string strValue) => strValue.Replace("'", "''");

        public static string CreateSqlInsertStatementCode(
            string tableName,
            params SqlValue[] sqlValues)
        {
            string columnNamesEnumerationStr = string.Join(
                ", ", sqlValues.Select(value => value.ColumnName));

            string valuesEnumerationStr = string.Join(
                ", ", sqlValues.Select(value => value.ToString()));

            string sqlInsertStatementCodeStr = string.Join(
                Environment.NewLine,
                $"INSERT INTO {tableName}",
                $"({columnNamesEnumerationStr})",
                "VALUES",
                $"({valuesEnumerationStr})");

            return sqlInsertStatementCodeStr;
        }

        public class SqlValue
        {
            public SqlValue()
            {
            }

            public SqlValue(
                string columnName,
                object? value,
                bool? isString = null)
            {
                ColumnName = columnName;
                Value = value;
                IsString = isString;
            }

            public string ColumnName { get; set; }
            public object? Value { get; set; }
            public bool? IsString { get; set; }

            public override string ToString()
            {
                bool isNull = this.Value == null;
                string retStr = isNull ? "NULL" : this.Value!.ToString()!;

                if (!isNull && (this.IsString ?? this.Value!.GetType() == typeof(string)))
                {
                    retStr = EncodeSqlStrValue(retStr);
                    retStr = $"'{retStr}'";
                }

                return retStr;
            }
        }
    }
}
