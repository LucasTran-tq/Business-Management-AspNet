using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SalaryManagement.Models
{
    [Table("BonusSalary")]
    public class BonusSalary
    {
        [Key]
        public int BonusSalaryId { set; get; }


        // [Required(ErrorMessage = "Must have Bonus Level")]
        // [Display(Name = "Bonus Level")]
        // public int BonusLevel {set;get;}


        [Required(ErrorMessage = "Must have Bonus Salary Name")]
        [Display(Name = "Bonus Salary Name")]
        [StringLength(200)]
        public string BonusSalaryName { set; get; }


        [Required(ErrorMessage = "Must have Prize Money")]
        [Display(Name = "Prize Money")]
        public double PrizeMoney {set;get;}


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