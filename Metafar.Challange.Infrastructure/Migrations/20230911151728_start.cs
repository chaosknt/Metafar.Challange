using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metafar.Challange.Data.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreditCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    AccountBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastExtraction = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "accountmovements",
                schema: "dbo",
                columns: table => new
                {
                    AccountMovementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountmovements", x => x.AccountMovementId);
                    table.ForeignKey(
                        name: "FK_accountmovements_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accountmovements_UserId",
                schema: "dbo",
                table: "accountmovements",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountmovements",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dbo");
        }
    }
}
