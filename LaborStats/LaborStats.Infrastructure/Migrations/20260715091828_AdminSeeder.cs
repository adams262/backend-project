using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaborStats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdminSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "usr",
                newName: "roles",
                newSchema: "usr");

            migrationBuilder.RenameTable(
                name: "Professions",
                schema: "prof",
                newName: "professions",
                newSchema: "prof");

            migrationBuilder.RenameTable(
                name: "ProfessionGroups",
                schema: "prof",
                newName: "profession_groups",
                newSchema: "prof");

            migrationBuilder.InsertData(
                schema: "usr",
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("7fc7f05a-03a2-4e3e-8611-4e160ed685c1"), "BaseUser" },
                    { new Guid("d2db6b7d-e4c2-4a50-85d1-71bd12557cf4"), "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "usr",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("7fc7f05a-03a2-4e3e-8611-4e160ed685c1"));

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("d2db6b7d-e4c2-4a50-85d1-71bd12557cf4"));

            migrationBuilder.RenameTable(
                name: "roles",
                schema: "usr",
                newName: "Roles",
                newSchema: "usr");

            migrationBuilder.RenameTable(
                name: "professions",
                schema: "prof",
                newName: "Professions",
                newSchema: "prof");

            migrationBuilder.RenameTable(
                name: "profession_groups",
                schema: "prof",
                newName: "ProfessionGroups",
                newSchema: "prof");
        }
    }
}
