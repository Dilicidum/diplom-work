using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Value : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Tasks_VacancyId",
                table: "Criterias");

            migrationBuilder.AlterColumn<int>(
                name: "VacancyId",
                table: "Criterias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "CandidateCriterias",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Tasks_VacancyId",
                table: "Criterias",
                column: "VacancyId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Tasks_VacancyId",
                table: "Criterias");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "CandidateCriterias");

            migrationBuilder.AlterColumn<int>(
                name: "VacancyId",
                table: "Criterias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Tasks_VacancyId",
                table: "Criterias",
                column: "VacancyId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
