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
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Culture", "DateCreated", "DateModified", "Email", "FullName", "IsAdmin", "Language", "MobileNumber", "Password", "PasswordHash", "Username" },
                values: new object[] { new Guid("04e2d549-16d1-45bf-97c5-2c7f0ae1c210"), "en-US", new DateTime(2024, 2, 2, 16, 47, 20, 66, DateTimeKind.Utc).AddTicks(3558), new DateTime(2024, 2, 2, 16, 47, 20, 66, DateTimeKind.Utc).AddTicks(3558), "admin@localhost", "Admin", true, "English", "+65467891324586", "************", "$2a$11$brtnF9JLRcDsQTOJqj4KFuTqA1lmDi4lFBXrlgIHovuPSLGz29lvS", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id_Email",
                table: "Users",
                columns: new[] { "Id", "Email" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
