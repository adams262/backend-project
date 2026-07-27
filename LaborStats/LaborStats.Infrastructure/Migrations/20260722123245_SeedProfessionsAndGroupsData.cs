using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaborStats.Infrastructure.Migrations;

/// <inheritdoc />
public partial class SeedProfessionsAndGroupsData : Migration
{
    private const string SeedResourceName = "LaborStats.Infrastructure.Data.Seed.SeedProfessionsAndGroupsData.sql";
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string sql = ReadEmbeddedSql(SeedResourceName);
        migrationBuilder.Sql(sql);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(
            """
            DELETE FROM prof.professions;
            DELETE FROM prof.profession_groups;
            """);
    }
    private static string ReadEmbeddedSql(string resourceName)
    {
        using Stream stream =
            typeof(SeedProfessionsAndGroupsData)
                .Assembly
                .GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"SQL file was not found: {resourceName}");

        using StreamReader reader = new(stream);

        return reader.ReadToEnd();
    }
}
