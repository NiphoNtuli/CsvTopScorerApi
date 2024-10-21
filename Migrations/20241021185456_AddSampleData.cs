using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using CsvTopScorerApi.Models;

#nullable disable

namespace CsvTopScorerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestData.csv");

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            }))
            {
                csv.Context.RegisterClassMap<ScoreMap>();

                var records = csv.GetRecords<Score>().ToList();
                
                foreach (var record in records)
                {
                    migrationBuilder.InsertData(
                        table: "Scores",
                        columns: new[] { "FirstName", "SecondName", "ScoreValue" },
                        values: new object[] { record.FirstName, record.SecondName, record.ScoreValue });
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Scores");
        }
    }

    public class ScoreMap : ClassMap<Score>
    {
        public ScoreMap()
        {
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.SecondName).Name("Second Name");
            Map(m => m.ScoreValue).Name("Score");
        }
    }
}