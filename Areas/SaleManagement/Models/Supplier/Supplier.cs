
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SaleManagement.Models
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public int SupplierId { set; get; }

        [Required(ErrorMessage = "Must have supplier name")]
        [Display(Name = "Supplier name")]
        [StringLength(160)]
        public string SupplierName {set;get;}

        //
    }

}