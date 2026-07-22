using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaborStats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImportHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "import");

            migrationBuilder.CreateTable(
                name: "import_histories",
                schema: "import",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    voivodeship = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    period = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    import_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    processed_records_count = table.Column<int>(type: "integer", nullable: false),
                    created_by = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_import_histories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "labor_stat_records",
                schema: "import",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuidv7()"),
                    import_history_id = table.Column<Guid>(type: "uuid", nullable: false),
                    voivodeship = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    powiat = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    occupation_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    insurance_title_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    total_contracts_count = table.Column<int>(type: "integer", nullable: true),
                    long_term_contracts_count = table.Column<int>(type: "integer", nullable: true),
                    newly_registered_count = table.Column<int>(type: "integer", nullable: true),
                    data_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_labor_stat_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_labor_stat_records_import_histories_import_history_id",
                        column: x => x.import_history_id,
                        principalSchema: "import",
                        principalTable: "import_histories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_labor_stat_records_import_history_id",
                schema: "import",
                table: "labor_stat_records",
                column: "import_history_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "labor_stat_records",
                schema: "import");

            migrationBuilder.DropTable(
                name: "import_histories",
                schema: "import");
        }
    }
}
