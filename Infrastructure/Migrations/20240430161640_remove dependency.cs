using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Tasks_VacancyId",
                table: "Candidates");

            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Tasks_VacancyId",
                table: "Criterias");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_BaseTaskId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_BaseTaskId",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Vacancies");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "Vacancies",
                newName: "IX_Vacancies_UserId");

            migrationBuilder.AddColumn<int>(
                name: "TasksId",
                table: "Vacancies",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vacancies",
                table: "Vacancies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_TasksId",
                table: "Vacancies",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Vacancies_VacancyId",
                table: "Candidates",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Vacancies_VacancyId",
                table: "Criterias",
                column: "VacancyId",
                principalTable: "Vacancies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_AspNetUsers_UserId",
                table: "Vacancies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Vacancies_TasksId",
                table: "Vacancies",
                column: "TasksId",
                principalTable: "Vacancies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Vacancies_VacancyId",
                table: "Candidates");

            migrationBuilder.DropForeignKey(
                name: "FK_Criterias_Vacancies_VacancyId",
                table: "Criterias");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_AspNetUsers_UserId",
                table: "Vacancies");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Vacancies_TasksId",
                table: "Vacancies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vacancies",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_TasksId",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "TasksId",
                table: "Vacancies");

            migrationBuilder.RenameTable(
                name: "Vacancies",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_Vacancies_UserId",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_BaseTaskId",
                table: "Tasks",
                column: "BaseTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Tasks_VacancyId",
                table: "Candidates",
                column: "VacancyId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Criterias_Tasks_VacancyId",
                table: "Criterias",
                column: "VacancyId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_BaseTaskId",
                table: "Tasks",
                column: "BaseTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
