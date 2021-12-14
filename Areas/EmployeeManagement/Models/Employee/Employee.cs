using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.EmployeeManagement.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmployeeId { set; get; }

        [Required(ErrorMessage = "Must have employee name")]
        [Display(Name = "Employee name")]
        [StringLength(50)]
        public string EmployeeName { set; get; }


        [Display(Name = "Day of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { set; get; }


        [Required(ErrorMessage = "Must have sex")]
        [Display(Name = "Sex")]
        [StringLength(10)]
        public string Sex { set; get; }


        [Required(ErrorMessage = "Must have place of birth")]
        [Display(Name = "Place of birth")]
        [StringLength(50)]
        public string PlaceOfBirth { set; get; }

        public string Address{get;set;}
        

        public List<Employee_Skill> Employee_Skills { get; set; }


        public int DepartmentId { set; get; }
        [ForeignKey("DepartmentId")]
        public Department Department{set;get;}

        
        public int LevelId { set; get; }
        [ForeignKey("LevelId")]
        public Level Level{set;get;}

        [Display(Name = "Avatar")]
        public byte[] ImageByte {get;set;}
    }

}