using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Identity.Models.RoleViewModels
{
  public class EditClaimModel
  {
    [Display(Name = "Type (name) claim")]
    [Required(ErrorMessage = "Must enter {0}")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} must be {2} to {1} characters long")]
    public string ClaimType { get; set; }

    [Display(Name = "Value")]
    [Required(ErrorMessage = "Must enter {0}")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} must be {2} to {1} characters long")]
    public string ClaimValue { get; set; }

    public IdentityRole role { get; set; }


  }
}
