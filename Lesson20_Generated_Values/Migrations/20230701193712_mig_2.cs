using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lesson20_Generated_Values.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValueSql: "FLOOR(RAND()*1000)",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Salary",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 100,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "FLOOR(RAND()*1000)");
        }
    }
}
