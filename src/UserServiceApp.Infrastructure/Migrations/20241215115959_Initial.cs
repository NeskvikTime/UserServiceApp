using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserServiceApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Culture", "DateCreated", "DateModified", "Email", "FullName", "IsAdmin", "Language", "MobileNumber", "Password", "PasswordHash", "Username" },
                values: new object[] { new Guid("a044e109-7782-4f78-b753-18febec8f383"), "en-US", new DateTime(2024, 12, 15, 11, 59, 58, 639, DateTimeKind.Utc).AddTicks(9749), new DateTime(2024, 12, 15, 11, 59, 58, 639, DateTimeKind.Utc).AddTicks(9749), "admin@localhost", "Admin", true, "English", "+65467891324586", "*****************", "$2a$11$r72ohlRsT0WgSAfNkcsFcOJNdjgl3nZguF41tL2Md0xX3ha16xh8W", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
