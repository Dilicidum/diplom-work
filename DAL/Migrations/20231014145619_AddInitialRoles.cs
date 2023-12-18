using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    /// 
    [ExcludeFromCodeCoverage]

    public partial class AddInitialRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1690468f-5331-4432-88f0-65ca23ffdf0b", "7bfc730a-9396-4cb9-9024-a600aaaf1c3c", "Admin", null },
                    { "2450be2b-5001-4ee4-9f75-dd9428acd648", "9139786e-995e-4770-bda4-7bf564dd9379", "User", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1690468f-5331-4432-88f0-65ca23ffdf0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2450be2b-5001-4ee4-9f75-dd9428acd648");
        }
    }
}
