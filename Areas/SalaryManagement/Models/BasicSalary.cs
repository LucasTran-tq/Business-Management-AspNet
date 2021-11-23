using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.EmployeeManagement.Models;

namespace App.Areas.SalaryManagement.Models
{
    [Table("BasicSalary")]
    public class BasicSalary
    {
        [Key]
        public int BasicSalaryId {set;get;}
        
        
        public int ContractTypeId{set;get;}

        [ForeignKey("ContractTypeId")]
        public ContractType ContractType{set;get;}


        [Required(ErrorMessage = "Must have Basic Salary Name")]
        [Display(Name = "Basic Salary Name")]
        [StringLength(200)]
        public string BasicSalaryName { set; get; }


        [Required(ErrorMessage = "Must have Money")]
        [Display(Name = "Money")]
        public double Money{set;get;}


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