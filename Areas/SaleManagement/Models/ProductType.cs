
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SaleManagement.Models
{
    [Table("ProductType")]
    public class ProductType
    {
        [Key]
        public int ProductTypeId { set; get; }

        [Required(ErrorMessage = "Must have Product Type Name")]
        [Display(Name = "Product Type Name")]
        [StringLength(50)]
        public string ProductTypeName {set;get;}

        //
    }

}