using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Areas.Identity.Models.UserViewModels
{
  public class SetUserPasswordModel
  {
      [Required(ErrorMessage = "Must enter {0}")]
      [StringLength(100, ErrorMessage = "{0} must be {2} to {1} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "New password")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("NewPassword", ErrorMessage = "Repeated incorrect password.")]
      public string ConfirmPassword { get; set; }


  }
}