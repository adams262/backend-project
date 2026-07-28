using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaborStats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "voivodeships",
                columns: table => new
                {
                    teryt = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_voivodeships", x => x.teryt);
                });

            migrationBuilder.CreateTable(
                name: "counties",
                columns: table => new
                {
                    teryt = table.Column<string>(type: "character(4)", fixedLength: true, maxLength: 4, nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    is_city_with_county_rights = table.Column<bool>(type: "boolean", nullable: false),
                    voivodeship_teryt = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_counties", x => x.teryt);
                    table.ForeignKey(
                        name: "fk_counties_voivodeships_voivodeship_teryt",
                        column: x => x.voivodeship_teryt,
                        principalTable: "voivodeships",
                        principalColumn: "teryt",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_counties_voivodeship_teryt",
                table: "counties",
                column: "voivodeship_teryt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "counties");

            migrationBuilder.DropTable(
                name: "voivodeships");
        }
    }
}
