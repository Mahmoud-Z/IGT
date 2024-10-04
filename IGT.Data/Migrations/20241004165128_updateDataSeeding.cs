using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IGT.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12ce16c4-ffb6-46fe-858e-0b7a48425a5f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93e5f7ab-048c-424a-807f-f117b5ca3067");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedUserId", "Discriminator", "Name", "NormalizedName", "SystemStatusCodeId" },
                values: new object[,]
                {
                    { "594efda7-754f-4ce9-9868-20e346ed6754", "3", "-99", "Role", "Customer", "Customer", null },
                    { "8eacacb4-c241-4b55-b90e-dfa0b96ad3bc", "2", "-99", "Role", "User", "User", null },
                    { "9b836ebf-07e5-4bcb-9a79-60e347204f4a", "1", "-99", "Role", "Admin", "Admin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "594efda7-754f-4ce9-9868-20e346ed6754");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8eacacb4-c241-4b55-b90e-dfa0b96ad3bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b836ebf-07e5-4bcb-9a79-60e347204f4a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedUserId", "Discriminator", "Name", "NormalizedName", "SystemStatusCodeId" },
                values: new object[,]
                {
                    { "12ce16c4-ffb6-46fe-858e-0b7a48425a5f", "2", "-99", "Role", "User", "User", null },
                    { "93e5f7ab-048c-424a-807f-f117b5ca3067", "1", "-99", "Role", "Admin", "Admin", null }
                });
        }
    }
}
