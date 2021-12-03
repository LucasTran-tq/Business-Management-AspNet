
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SaleManagement.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { set; get; }


        [Required(ErrorMessage = "Must have product name")]
        [Display(Name = "Product name")]
        [StringLength(160)]
        public string ProductName { set; get; }


        public int SupplierId { set; get; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { set; get; }


        public int ProductTypeId { set; get; }

        [ForeignKey("ProductTypeId")]
        public ProductType ProductType { set; get; }


        [Required(ErrorMessage = "Must have Unit")]
        [Display(Name = "Unit")]
        [StringLength(50)]
        public string Unit { get; set; }



    }

}