using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_ASP.NET_Core_Learn.Migrations
{
    public partial class supplementUserDeposit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InterestPayment",
                table: "UserDeposits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "UserDeposits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestPayment",
                table: "UserDeposits");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "UserDeposits");
        }
    }
}
