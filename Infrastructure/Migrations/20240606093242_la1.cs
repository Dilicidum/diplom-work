using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class la1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Vacancies_VacancyId",
                table: "Criterias");

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Vacancies_VacancyId",
                table: "Criterias",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Vacancies_VacancyId",
                table: "Criterias");

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Vacancies_VacancyId",
                table: "Criterias",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");
        }
    }
}
