using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using DAL.Database.Migrations.Countries;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Data.SqlClient;
using DAL.Helpers;

namespace DAL.Database.Migrations
{
    public class StaticCataloguesInsertComponent
    {
        public void InsertData(
            MigrationBuilder migrationBuilder)
        {
            int cntrPk = 0;
            int countyPk = 0;

            foreach (var country in WorldCountries.GetAll())
            {
                string sqlScript = SqlCode.CreateSqlInsertStatementCode(
                    "Countries", [
                        new ("Id", ++cntrPk),
                        new ("Name", country.Name),
                        new ("Code", country.Code)]);

                migrationBuilder.Sql(sqlScript);

                if (country.Counties != null)
                {
                    foreach (var countyObj in country.Counties)
                    {
                        sqlScript = SqlCode.CreateSqlInsertStatementCode(
                            "Counties", [
                                new ("Id", ++countyPk),
                                new ("Name", countyObj.Name),
                                new ("CountryId", cntrPk)]);

                        migrationBuilder.Sql(sqlScript);
                    }
                }
            }
        }
    }
}
