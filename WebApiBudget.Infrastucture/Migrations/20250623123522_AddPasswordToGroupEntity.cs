using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBudget.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToGroupEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Groups");
        }
    }
}
