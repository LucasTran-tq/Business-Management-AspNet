
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SaleManagement.Models
{
    [Table("DetailBill")]
    public class DetailBill
    {
        [Key]
        public int DetailBillId { set; get; }


        public int ProductId { set; get; }

        [ForeignKey("ProductId")]
        public Product Product { set; get; }


        public int BillId { set; get; }

        [ForeignKey("BillId")]
        public Bill Bill { set; get; }

        [Display(Name = "Price Product")]
        public double PriceProduct{set;get;}


        [Required(ErrorMessage = "Must have Amount")]
        [Display(Name = "Amount")]
        public double Amount { set; get; }


       

    }

}