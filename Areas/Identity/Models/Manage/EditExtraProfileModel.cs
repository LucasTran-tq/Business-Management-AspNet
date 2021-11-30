using System;
using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.Models.ManageViewModels
{
  public class EditExtraProfileModel
  {
      [Display(Name = "Username")]
      public string UserName { get; set; }

      [Display(Name = "Email")]
      public string UserEmail { get; set; }
      [Display(Name = "Phone number")]
      public string PhoneNumber { get; set; }

      [Display(Name = "Address")]
      [StringLength(400)]
      public string HomeAdress { get; set; }


      [Display(Name = "Date of birth")]
      public DateTime? BirthDate { get; set; }
  }
}