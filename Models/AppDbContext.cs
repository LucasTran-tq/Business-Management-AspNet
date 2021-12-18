using App.Areas.EmployeeManagement.Models;
using App.Areas.SalaryManagement.Models;
using App.Areas.SaleManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace App.Models 
{
    // App.Models.AppDbContext
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // builder.UseMySQL("server=sql6.freesqldatabase.com;database=sql6459153;user=sql6459153;password=XJSEAavrZd");
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            // modelBuilder.Entity<Department>()
            // .ToTable("Department", t => t.ExcludeFromMigrations());

            // modelBuilder.Entity<Employee_Skill>( entity => {
            //     entity.HasKey( c => new {c.EmployeeId, c.SkillId});
            // });


        }
       

        public DbSet<Department> Departments { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employee_Skill> Employee_Skills { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee_Position> Employee_Positions { get; set; }
        public DbSet<OvertimeSalary> OvertimeSalaries { get; set; }
        public DbSet<BonusSalary> BonusSalaries { get; set; }
        public DbSet<AllowanceSalary> AllowanceSalaries { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<BasicSalary> BasicSalaries { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<DetailBill> DetailBills { get; set; }


    }
}
