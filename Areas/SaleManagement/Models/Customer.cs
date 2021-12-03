
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SaleManagement.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerId { set; get; }


        [Required(ErrorMessage = "Must have customer name")]
        [Display(Name = "Customer name")]
        [StringLength(160)]
        public string CustomerName { set; get; }


        [Required(ErrorMessage = "Must have address")]
        [Display(Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }


        [Phone(ErrorMessage = "Must be phone format")]
        [Display(Name = "Phone number")]
        [StringLength(13)]
        public string PhoneNumber { get; set; }
    }

}