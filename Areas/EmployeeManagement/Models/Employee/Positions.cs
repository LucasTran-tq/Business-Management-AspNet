using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.EmployeeManagement.Models
{
    [Table("Position")]
    public class Position
    {
        [Key]
        public int PositionId { set; get; }

        [Required(ErrorMessage = "Must have position name")]
        [Display(Name = "Position")]
        [StringLength(160)]
        public string PositionName {set;get;}

    }

}