using Microsoft.EntityFrameworkCore.Migrations;

namespace AppMvc.Net.Migrations
{
    public partial class UpdateSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalSalary",
                table: "Salary",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSalary",
                table: "Salary");
        }
    }
}
