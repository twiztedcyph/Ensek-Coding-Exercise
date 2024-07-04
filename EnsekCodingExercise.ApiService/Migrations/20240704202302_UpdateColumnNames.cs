using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnsekCodingExercise.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Readings",
                newName: "MeterReadValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MeterReadValue",
                table: "Readings",
                newName: "Value");
        }
    }
}
