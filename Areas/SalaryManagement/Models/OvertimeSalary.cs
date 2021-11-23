using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SalaryManagement.Models
{
    [Table("OvertimeSalary")]
    public class OvertimeSalary
    {
        [Key]
        public int OvertimeSalaryId { set; get; }


        [Required(ErrorMessage = "Must have Overtime Salary Name")]
        [Display(Name = "Overtime Salary Name")]
        [StringLength(200)]
        public string OvertimeSalaryName { set; get; }


        [Required(ErrorMessage = "Must have money per session")]
        [Display(Name = "Money per session")]
        public double moneyPerSession {set;get;}


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