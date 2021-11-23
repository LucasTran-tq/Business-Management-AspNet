using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AppMvc.Net.Migrations
{
    public partial class AddSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllowanceSalary",
                columns: table => new
                {
                    AllowanceSalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    AllowanceSalaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Allowance = table.Column<double>(type: "float", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowanceSalary", x => x.AllowanceSalaryId);
                    table.ForeignKey(
                        name: "FK_AllowanceSalary_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasicSalary",
                columns: table => new
                {
                    BasicSalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    BasicSalaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Money = table.Column<double>(type: "float", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicSalary", x => x.BasicSalaryId);
                    table.ForeignKey(
                        name: "FK_BasicSalary_ContractType_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "ContractType",
                        principalColumn: "ContractTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BonusSalary",
                columns: table => new
                {
                    BonusSalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonusLevel = table.Column<int>(type: "int", nullable: false),
                    BonusSalaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrizeMoney = table.Column<double>(type: "float", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusSalary", x => x.BonusSalaryId);
                });

            migrationBuilder.CreateTable(
                name: "OvertimeSalary",
                columns: table => new
                {
                    OvertimeSalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OvertimeSalaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    moneyPerSession = table.Column<double>(type: "float", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OvertimeSalary", x => x.OvertimeSalaryId);
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    SalaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    BasicSalaryId = table.Column<int>(type: "int", nullable: false),
                    AllowanceSalaryId = table.Column<int>(type: "int", nullable: false),
                    BonusSalaryId = table.Column<int>(type: "int", nullable: false),
                    OvertimeSalaryId = table.Column<int>(type: "int", nullable: false),
                    BonusLevel = table.Column<int>(type: "int", nullable: false),
                    NumberOfSession = table.Column<double>(type: "float", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.SalaryId);
                    table.ForeignKey(
                        name: "FK_Salary_AllowanceSalary_AllowanceSalaryId",
                        column: x => x.AllowanceSalaryId,
                        principalTable: "AllowanceSalary",
                        principalColumn: "AllowanceSalaryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salary_BasicSalary_BasicSalaryId",
                        column: x => x.BasicSalaryId,
                        principalTable: "BasicSalary",
                        principalColumn: "BasicSalaryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salary_BonusSalary_BonusSalaryId",
                        column: x => x.BonusSalaryId,
                        principalTable: "BonusSalary",
                        principalColumn: "BonusSalaryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salary_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Salary_OvertimeSalary_OvertimeSalaryId",
                        column: x => x.OvertimeSalaryId,
                        principalTable: "OvertimeSalary",
                        principalColumn: "OvertimeSalaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowanceSalary_PositionId",
                table: "AllowanceSalary",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicSalary_ContractTypeId",
                table: "BasicSalary",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_AllowanceSalaryId",
                table: "Salary",
                column: "AllowanceSalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_BasicSalaryId",
                table: "Salary",
                column: "BasicSalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_BonusSalaryId",
                table: "Salary",
                column: "BonusSalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_EmployeeId",
                table: "Salary",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_OvertimeSalaryId",
                table: "Salary",
                column: "OvertimeSalaryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "AllowanceSalary");

            migrationBuilder.DropTable(
                name: "BasicSalary");

            migrationBuilder.DropTable(
                name: "BonusSalary");

            migrationBuilder.DropTable(
                name: "OvertimeSalary");
        }
    }
}
