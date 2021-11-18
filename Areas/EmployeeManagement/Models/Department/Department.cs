
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.EmployeeManagement.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int DepartmentId { set; get; }

        [Required(ErrorMessage = "Must have department name")]
        [Display(Name = "Department name")]
        [StringLength(160)]
        public string DepartmentName {set;get;}

        [StringLength(50)]
        [Phone(ErrorMessage = "Must be phone format")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }

}