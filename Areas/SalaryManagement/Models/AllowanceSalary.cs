using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.EmployeeManagement.Models;

namespace App.Areas.SalaryManagement.Models
{
    [Table("AllowanceSalary")]
    public class AllowanceSalary
    {
        [Key]
        public int AllowanceSalaryId { set; get; }


        public int PositionId{set;get;}

        [ForeignKey("PositionId")]
        public Position Position{set;get;}


        [Required(ErrorMessage = "Must have Allowance Salary Name")]
        [Display(Name = "Allowance Salary Name")]
        [StringLength(200)]
        public string AllowanceSalaryName { set; get; }


        [Required(ErrorMessage = "Must have Prize Money")]
        [Display(Name = "Prize Money")]
        public double Allowance {set;get;}


        [Display(Name = "Start time Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartTime{set;get;}


        [Display(Name = "End time Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndTime{set;get;}

    }

}