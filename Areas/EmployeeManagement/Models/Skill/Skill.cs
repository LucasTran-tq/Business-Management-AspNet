using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.EmployeeDepartment.Models
{
    [Table("Skill")]
    public class Skill
    {
        [Key]
        public int SkillId { set; get; }

        [Required(ErrorMessage = "Must have skill name")]
        [Display(Name = "Skill name")]
        [StringLength(160)]
        public string SkillName {set;get;}

    }

}