using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.Models.RoleViewModels
{
  public class CreateRoleModel
    {
        [Display(Name = "The name of the role")]
        [Required(ErrorMessage = "Must enter {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0}  must be {2} to {1} characters long")]
        public string Name { get; set; }


    }
}
