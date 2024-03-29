using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_ASP.NET_Core_Learn.Migrations
{
    public partial class addDepositTerms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositTerm_Deposits_DepositId",
                table: "DepositTerm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepositTerm",
                table: "DepositTerm");

            migrationBuilder.RenameTable(
                name: "DepositTerm",
                newName: "DepositTerms");

            migrationBuilder.RenameIndex(
                name: "IX_DepositTerm_DepositId",
                table: "DepositTerms",
                newName: "IX_DepositTerms_DepositId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepositTerms",
                table: "DepositTerms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositTerms_Deposits_DepositId",
                table: "DepositTerms",
                column: "DepositId",
                principalTable: "Deposits",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositTerms_Deposits_DepositId",
                table: "DepositTerms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepositTerms",
                table: "DepositTerms");

            migrationBuilder.RenameTable(
                name: "DepositTerms",
                newName: "DepositTerm");

            migrationBuilder.RenameIndex(
                name: "IX_DepositTerms_DepositId",
                table: "DepositTerm",
                newName: "IX_DepositTerm_DepositId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepositTerm",
                table: "DepositTerm",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositTerm_Deposits_DepositId",
                table: "DepositTerm",
                column: "DepositId",
                principalTable: "Deposits",
                principalColumn: "Id");
        }
    }
}
