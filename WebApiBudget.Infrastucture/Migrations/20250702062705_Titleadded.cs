using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBudget.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class Titleadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tittle",
                table: "ExpenseRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tittle",
                table: "ExpenseRecords");
        }
    }
}
