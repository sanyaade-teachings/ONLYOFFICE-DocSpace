using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASC.Migrations.MySql.Migrations.CoreDb
{
    /// <inheritdoc />
    public partial class CoreDbContextUpgrade2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tenants_quota",
                keyColumn: "tenant",
                keyValue: -3,
                column: "features",
                value: "free,oauth,total_size:2147483648,manager:3,room:12");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tenants_quota",
                keyColumn: "tenant",
                keyValue: -3,
                column: "features",
                value: "free,total_size:2147483648,manager:3,room:12");
        }
    }
}
