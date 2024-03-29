using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_ASP.NET_Core_Learn.Migrations
{
    public partial class RenameDeposit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositTerms_Deposits_DepositId",
                table: "DepositTerms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeposits_Deposits_DepositId",
                table: "UserDeposits");

            migrationBuilder.DropTable(
                name: "Deposits");

            migrationBuilder.RenameColumn(
                name: "DepositId",
                table: "DepositTerms",
                newName: "DepositTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_DepositTerms_DepositId",
                table: "DepositTerms",
                newName: "IX_DepositTerms_DepositTemplateId");

            migrationBuilder.AlterColumn<int>(
                name: "SelectedTerm",
                table: "UserDeposits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DepositTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Replenishment = table.Column<bool>(type: "bit", nullable: false),
                    InterestPayment = table.Column<int>(type: "int", nullable: false),
                    InterestRateNoEarlyClosure = table.Column<double>(type: "float", nullable: true),
                    InterestRateEarlyClosure = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositTemplates", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DepositTerms_DepositTemplates_DepositTemplateId",
                table: "DepositTerms",
                column: "DepositTemplateId",
                principalTable: "DepositTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeposits_DepositTemplates_DepositId",
                table: "UserDeposits",
                column: "DepositId",
                principalTable: "DepositTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositTerms_DepositTemplates_DepositTemplateId",
                table: "DepositTerms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeposits_DepositTemplates_DepositId",
                table: "UserDeposits");

            migrationBuilder.DropTable(
                name: "DepositTemplates");

            migrationBuilder.RenameColumn(
                name: "DepositTemplateId",
                table: "DepositTerms",
                newName: "DepositId");

            migrationBuilder.RenameIndex(
                name: "IX_DepositTerms_DepositTemplateId",
                table: "DepositTerms",
                newName: "IX_DepositTerms_DepositId");

            migrationBuilder.AlterColumn<int>(
                name: "SelectedTerm",
                table: "UserDeposits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Deposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterestPayment = table.Column<int>(type: "int", nullable: false),
                    InterestRateEarlyClosure = table.Column<double>(type: "float", nullable: true),
                    InterestRateNoEarlyClosure = table.Column<double>(type: "float", nullable: true),
                    Replenishment = table.Column<bool>(type: "bit", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deposits", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DepositTerms_Deposits_DepositId",
                table: "DepositTerms",
                column: "DepositId",
                principalTable: "Deposits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeposits_Deposits_DepositId",
                table: "UserDeposits",
                column: "DepositId",
                principalTable: "Deposits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
