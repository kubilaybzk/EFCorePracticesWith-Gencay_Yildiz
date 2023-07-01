using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lesson20_Generated_Values.Migrations
{
    /// <inheritdoc />
    public partial class mig_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Persons",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "FLOOR(RAND()*1000)");

            migrationBuilder.AlterColumn<int>(
                name: "TotalGain",
                table: "Persons",
                type: "int",
                nullable: false,
                computedColumnSql: "[Salary] + [Premium]",
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalGain",
                table: "Persons",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComputedColumnSql: "[Salary] + [Premium]");

            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValueSql: "FLOOR(RAND()*1000)",
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
