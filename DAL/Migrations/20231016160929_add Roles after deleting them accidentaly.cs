using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    /// 
    [ExcludeFromCodeCoverage]

    public partial class addRolesafterdeletingthemaccidentaly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c0e9038-4a3f-42b8-b464-aa2f1ae7efd4", "e2ce48eb-9c55-4e96-8e60-9664cda20a12", "Admin", null },
                    { "2f66b254-558e-4ada-aa03-c84a49d42835", "97f1e761-e5f7-451a-bd50-a218452aabd9", "User", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c0e9038-4a3f-42b8-b464-aa2f1ae7efd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f66b254-558e-4ada-aa03-c84a49d42835");
        }
    }
}
