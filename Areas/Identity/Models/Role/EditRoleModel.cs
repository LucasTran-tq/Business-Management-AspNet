using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Identity.Models.RoleViewModels
{
  public class EditRoleModel
    {
        [Display(Name = "The name of the role")]
        [Required(ErrorMessage = "Must enter {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} must be {2} to {1} characters long")]
        public string Name { get; set; }
        public List<IdentityRoleClaim<string>> Claims { get; set; }

        public IdentityRole role { get; set; }




    }
}
