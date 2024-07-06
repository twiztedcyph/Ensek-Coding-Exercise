using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnsekCodingExercise.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MeterReadValue",
                table: "Readings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MeterReadValue",
                table: "Readings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
