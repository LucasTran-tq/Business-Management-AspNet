using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.EmployeeManagement.Models
{
    [Table("Employee_Skill")]
    public class Employee_Skill
    {
        [Key]
        public int id {set;get;}
        
        public int EmployeeId{set;get;}

        public int SkillId{set;get;}

        [ForeignKey("EmployeeId")]
        public Employee Employee{set;get;}

        [ForeignKey("SkillId")]
        public Skill Skill{set;get;}


        [Required(ErrorMessage = "Must have level")]
        [Display(Name = "Level")]
        [StringLength(50)]
        public string Level{set;get;}


        [Display(Name = "Evaluation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EvaluationDate{set;get;}

    }

}