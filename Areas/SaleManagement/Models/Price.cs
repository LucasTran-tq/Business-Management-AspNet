
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Areas.SaleManagement.Models
{
    [Table("Price")]
    public class Price
    {
        [Key]
        public int PriceId { set; get; }


         public int ProductId{set;get;}

        [ForeignKey("ProductId")]
        public Product Product{set;get;}


        [Required(ErrorMessage = "Must have Price Money")]
        [Display(Name = "Price Money")]
        public double PriceMoney {set;get;}


        [Display(Name = "Start time Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartTime{set;get;}


        [Display(Name = "End time Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndTime{set;get;}
        
    }

}