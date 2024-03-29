﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.Models.AccountViewModels
{
    public class VerifyAuthenticatorCodeViewModel
    {
        [Required(ErrorMessage = "Must enter {0}")]
        [Display(Name = "Enter the saved code")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember for this browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember login information?")]
        public bool RememberMe { get; set; }
    }
}
