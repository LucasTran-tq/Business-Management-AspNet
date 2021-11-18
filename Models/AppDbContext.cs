using App.Areas.EmployeeDepartment.Models;
using App.Areas.SaleDepartment.Models;
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
        
    }
}
