using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaborStats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Offers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "off");

            migrationBuilder.CreateTable(
                name: "offers",
                schema: "off",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    year = table.Column<int>(type: "integer", nullable: false),
                    period = table.Column<byte>(type: "smallint", nullable: false),
                    profession_group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: true),
                    job_title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    publication_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_offers", x => x.id);
                    table.ForeignKey(
                        name: "fk_offers_profession_group_profession_group_id",
                        column: x => x.profession_group_id,
                        principalSchema: "prof",
                        principalTable: "profession_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_offers_expiration_date",
                schema: "off",
                table: "offers",
                column: "expiration_date");

            migrationBuilder.CreateIndex(
                name: "ix_offers_profession_group_id",
                schema: "off",
                table: "offers",
                column: "profession_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_offers_publication_date",
                schema: "off",
                table: "offers",
                column: "publication_date");

            migrationBuilder.CreateIndex(
                name: "ix_offers_year_period",
                schema: "off",
                table: "offers",
                columns: new[] { "year", "period" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "offers",
                schema: "off");
        }
    }
}
