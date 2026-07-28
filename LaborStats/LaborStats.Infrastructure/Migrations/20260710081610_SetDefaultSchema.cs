using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaborStats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "geo");

            migrationBuilder.RenameTable(
                name: "voivodeships",
                newName: "voivodeships",
                newSchema: "geo");

            migrationBuilder.RenameTable(
                name: "counties",
                newName: "counties",
                newSchema: "geo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "voivodeships",
                schema: "geo",
                newName: "voivodeships");

            migrationBuilder.RenameTable(
                name: "counties",
                schema: "geo",
                newName: "counties");
        }
    }
}
