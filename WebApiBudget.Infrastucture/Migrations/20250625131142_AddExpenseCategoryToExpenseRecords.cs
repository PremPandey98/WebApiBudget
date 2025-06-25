using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiBudget.Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseCategoryToExpenseRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ExpenseRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "ExpenseCategoryID",
                table: "ExpenseRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Set all existing ExpenseRecords to Miscellaneous (15) before adding FK
            migrationBuilder.Sql("UPDATE ExpenseRecords SET ExpenseCategoryID = 15 WHERE ExpenseCategoryID = 0");

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                columns: table => new
                {
                    ExpenseCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.ExpenseCategoryID);
                });

            migrationBuilder.InsertData(
                table: "ExpenseCategories",
                columns: new[] { "ExpenseCategoryID", "ExpenseCategoryName" },
                values: new object[,]
                {
                    { 1, "Food" },
                    { 2, "Hospital" },
                    { 3, "Investment" },
                    { 4, "Rent" },
                    { 5, "Bill" },
                    { 6, "Education" },
                    { 7, "Transport" },
                    { 8, "Entertainment" },
                    { 9, "Utilities" },
                    { 10, "Grocery" },
                    { 11, "Travel" },
                    { 12, "Insurance" },
                    { 13, "Shopping" },
                    { 14, "Loan" },
                    { 15, "Miscellaneous" },
                    { 16, "creditCardBill" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseRecords_ExpenseCategoryID",
                table: "ExpenseRecords",
                column: "ExpenseCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseRecords_ExpenseCategories_ExpenseCategoryID",
                table: "ExpenseRecords",
                column: "ExpenseCategoryID",
                principalTable: "ExpenseCategories",
                principalColumn: "ExpenseCategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseRecords_ExpenseCategories_ExpenseCategoryID",
                table: "ExpenseRecords");

            migrationBuilder.DropTable(
                name: "ExpenseCategories");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseRecords_ExpenseCategoryID",
                table: "ExpenseRecords");

            migrationBuilder.DropColumn(
                name: "ExpenseCategoryID",
                table: "ExpenseRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ExpenseRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
