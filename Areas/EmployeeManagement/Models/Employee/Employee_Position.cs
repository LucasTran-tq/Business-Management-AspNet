using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.EmployeeManagement.Models
{
    [Table("Employee_Position")]
    public class Employee_Position
    {
        [Key]
        public int id {set;get;}
        
        public int EmployeeId{set;get;}

        public int PositionId{set;get;}

        [ForeignKey("EmployeeId")]
        public Employee Employee{set;get;}

        [ForeignKey("PositionId")]
        public Position Position{set;get;}


        [Display(Name = "start time Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartTime{set;get;}

        [Display(Name = "End time Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndTime{set;get;}

    }

}