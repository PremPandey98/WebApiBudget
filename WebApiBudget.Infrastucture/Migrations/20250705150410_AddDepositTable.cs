using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBudget.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddDepositTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    DepositId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Tittle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AddedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsGroupRelated = table.Column<bool>(type: "bit", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.DepositId);
                    table.ForeignKey(
                        name: "FK_Deposits_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deposits_Users_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_AddedByUserId",
                table: "Deposits",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_GroupId",
                table: "Deposits",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deposits");
        }
    }
}
