using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.EmployeeManagement.Models;

namespace App.Areas.SalaryManagement.Models
{
    [Table("Contract")]
    public class Contract
    {
        [Key]
        public int id {set;get;}
        
        public int EmployeeId{set;get;}

        public int ContractTypeId{set;get;}

        [ForeignKey("EmployeeId")]
        public Employee Employee{set;get;}

        [ForeignKey("ContractTypeId")]
        public ContractType ContractType{set;get;}


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