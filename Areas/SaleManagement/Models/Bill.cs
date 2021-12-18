
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.EmployeeManagement.Models;

namespace App.Areas.SaleManagement.Models
{
    [Table("Bill")]
    public class Bill
    {
        [Key]
        public int BillId { set; get; }


         public int EmployeeId{set;get;}

        [ForeignKey("EmployeeId")]
        public Employee Employee{set;get;}


         public int CustomerId{set;get;}

        [ForeignKey("CustomerId")]
        public Customer Customer{set;get;}

        [Display(Name = "Total Bill")]
        public double TotalBill{set;get;}


        [Display(Name = "Make Bill Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime MakeBillTime{set;get;}


        [Display(Name = "Export Bill Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExportBillTime{set;get;}
        
    }

}