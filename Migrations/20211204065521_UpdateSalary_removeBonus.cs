using Microsoft.EntityFrameworkCore.Migrations;

namespace AppMvc.Net.Migrations
{
    public partial class UpdateSalary_removeBonus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusLevel",
                table: "Salary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BonusLevel",
                table: "Salary",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
