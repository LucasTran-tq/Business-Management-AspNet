using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.EmployeeManagement.Models
{
    [Table("Level")]
    public class Level
    {
        [Key]
        public int LevelId { set; get; }

        [Required(ErrorMessage = "Must have level name")]
        [Display(Name = "Level name")]
        [StringLength(50)]
        public string LevelName {set;get;}

    }

}