using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SalaryManagement.Models
{
    [Table("ContractType")]
    public class ContractType
    {
        [Key]
        public int ContractTypeId { set; get; }


        [Required(ErrorMessage = "Must have Contract Type Name")]
        [Display(Name = "Contract Type Name")]
        [StringLength(20)]
        public string ContractTypeName {set;get;}
    }

}