using ClosedXML.Excel;
using LaborStats.Application.Imports.Dtos;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;
using LaborStats.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LaborStats.Tests.Services;

public class DataConversionServiceTests
{
    private async Task<LaborStatsDbContext> GetInMemoryDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<LaborStatsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new LaborStatsDbContext(options);

        var voivodeship = new Voivodeship
        {
            Teryt = "30",
            Name = "WIELKOPOLSKIE"
        };

        dbContext.Voivodeships.Add(voivodeship);

        dbContext.Counties.Add(new LaborStats.Domain.Entities.County 
        { 
            Name = "Poznański", 
            Teryt = "3021",
            VoivodeshipTeryt = "30" 
        });

        dbContext.Profession.Add(new LaborStats.Domain.Entities.Professions 
        { 
            Id = Guid.NewGuid(), 
            KzisCode = 251201, 
            KzisName = "Programista",
            ProfessionGroupId = Guid.NewGuid()
        });

        await dbContext.SaveChangesAsync();
        return dbContext;
    }

    [Fact]
    public async Task ConvertAsync_ShouldSuccessfullyConvertExcelFile()
    {
        await using var dbContext = await GetInMemoryDbContextAsync();
        var service = new DataConversionService(dbContext);

        using var stream = new MemoryStream();
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("2024-Q1"); 

            worksheet.Cell(2, 1).Value = "POWIAT";
            worksheet.Cell(2, 2).Value = "KOD ZAWODU";
            worksheet.Cell(2, 3).Value = "WSZYSTKICH UMÓW";

            worksheet.Cell(3, 1).Value = "Poznański";
            worksheet.Cell(3, 2).Value = "251201";
            worksheet.Cell(3, 3).Value = "1,5 tys.";

            workbook.SaveAs(stream);
        }
        stream.Position = 0; 

        var result = await service.ConvertAsync(stream, "WIELKOPOLSKIE", ImportDataType.Employed, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(1500, result[0].Value);
        Assert.Equal("3021", result[0].CountyId);
    }
}
