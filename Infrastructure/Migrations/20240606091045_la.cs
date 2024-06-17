using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class la : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateCriterias_Candidates_CandidateId",
                table: "CandidateCriterias");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateCriterias_Candidates_CandidateId",
                table: "CandidateCriterias",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateCriterias_Candidates_CandidateId",
                table: "CandidateCriterias");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateCriterias_Candidates_CandidateId",
                table: "CandidateCriterias",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
