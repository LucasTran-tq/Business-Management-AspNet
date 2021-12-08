using Microsoft.EntityFrameworkCore.Migrations;

namespace AppMvc.Net.Migrations
{
    public partial class updateDetailBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceId",
                table: "DetailBill",
                newName: "DetailBillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DetailBillId",
                table: "DetailBill",
                newName: "PriceId");
        }
    }
}
