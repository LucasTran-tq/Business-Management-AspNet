using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.EmployeeManagement.Models;

namespace App.Areas.SalaryManagement.Models
{
    [Table("Salary")]
    public class Salary
    {
        [Key]
        public int SalaryId {set;get;}


        [Required(ErrorMessage = "Must have Total Salary")]
        [Display(Name = "TotalSalary")]
        public double TotalSalary {set;get;}  


        public int EmployeeId{set;get;}

         public int BasicSalaryId {set;get;}

        public int AllowanceSalaryId { set; get; }

        public int BonusSalaryId { set; get; }

        public int OvertimeSalaryId { set; get; }

        [ForeignKey("EmployeeId")]
        public Employee Employee{set;get;}

        [ForeignKey("BasicSalaryId")]
        public BasicSalary BasicSalary{set;get;}

        [ForeignKey("AllowanceSalaryId")]
        public AllowanceSalary AllowanceSalary{set;get;}

        [ForeignKey("BonusSalaryId")]
        public BonusSalary BonusSalary{set;get;}

        [ForeignKey("OvertimeSalaryId")]
        public OvertimeSalary OvertimeSalary{set;get;}


        // [Required(ErrorMessage = "Must have Bonus Level")]
        // [Display(Name = "Bonus Level")]
        // public int BonusLevel {set;get;}


        [Required(ErrorMessage = "Must have Number of session")]
        [Display(Name = "Number of session")]
        public double NumberOfSession {set;get;}  


        [Display(Name = "Salary Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SalaryDate{set;get;}

    }

}