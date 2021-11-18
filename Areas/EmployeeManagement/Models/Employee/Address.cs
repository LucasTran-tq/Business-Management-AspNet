using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.EmployeeManagement.Models
{
    [Table("Address")]
    [Keyless]
    public class Address
    {
    
        [Required(ErrorMessage = "Must have House number")]
        [Display(Name = "House number")]
        [StringLength(50)]
        public string HouseNumber { set; get; }


        [Required(ErrorMessage = "Must have Street")]
        [Display(Name = "Street")]
        [StringLength(50)]
        public string Street { set; get; }


        [Required(ErrorMessage = "Must have Ward")]
        [Display(Name = "Ward")]
        [StringLength(50)]
        public string Ward { set; get; }


        [Required(ErrorMessage = "Must have District")]
        [Display(Name = "District")]
        [StringLength(50)]
        public string District { set; get; }


        [Required(ErrorMessage = "Must have Province")]
        [Display(Name = "Province")]
        [StringLength(50)]
        public string Province { set; get; }
    }

}