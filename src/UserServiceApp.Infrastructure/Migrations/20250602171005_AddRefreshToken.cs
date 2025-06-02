using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserServiceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a044e109-7782-4f78-b753-18febec8f383"));

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Culture", "DateCreated", "DateModified", "Email", "FullName", "IsAdmin", "Language", "MobileNumber", "Password", "PasswordHash", "Username" },
                values: new object[] { new Guid("e0f804e9-08f9-4689-ac97-42acfb87114d"), "en-US", new DateTime(2025, 6, 2, 17, 10, 4, 651, DateTimeKind.Utc).AddTicks(3028), new DateTime(2025, 6, 2, 17, 10, 4, 651, DateTimeKind.Utc).AddTicks(3028), "admin@localhost", "Admin", true, "English", "+65467891324586", "*****************", "$2a$11$xRxKzxILymjey3vsuQY/xOkEaNy1Tpxu.eaqrTSawFH2qJ0SXntm6", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Id",
                table: "RefreshTokens",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e0f804e9-08f9-4689-ac97-42acfb87114d"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Culture", "DateCreated", "DateModified", "Email", "FullName", "IsAdmin", "Language", "MobileNumber", "Password", "PasswordHash", "Username" },
                values: new object[] { new Guid("a044e109-7782-4f78-b753-18febec8f383"), "en-US", new DateTime(2024, 12, 15, 11, 59, 58, 639, DateTimeKind.Utc).AddTicks(9749), new DateTime(2024, 12, 15, 11, 59, 58, 639, DateTimeKind.Utc).AddTicks(9749), "admin@localhost", "Admin", true, "English", "+65467891324586", "*****************", "$2a$11$r72ohlRsT0WgSAfNkcsFcOJNdjgl3nZguF41tL2Md0xX3ha16xh8W", "admin" });
        }
    }
}
