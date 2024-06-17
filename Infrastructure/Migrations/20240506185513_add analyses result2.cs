using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addanalysesresult2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacancies_Vacancies_TasksId",
                table: "Vacancies");

            migrationBuilder.DropIndex(
                name: "IX_Vacancies_TasksId",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "TasksId",
                table: "Vacancies");

            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacancyId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analyses_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Analyses_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_CandidateId",
                table: "Analyses",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_VacancyId",
                table: "Analyses",
                column: "VacancyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.AddColumn<int>(
                name: "TasksId",
                table: "Vacancies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_TasksId",
                table: "Vacancies",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacancies_Vacancies_TasksId",
                table: "Vacancies",
                column: "TasksId",
                principalTable: "Vacancies",
                principalColumn: "Id");
        }
    }
}
