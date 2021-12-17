using System;
using System.Collections.Generic;

namespace App.Areas.SaleManagement.Models
{
    public class TopProduct
    {
        // public TopProduct(string ProductName, double Amount){
        //     this.ProductName = ProductName;
        //     this.Amount = Amount;
        // }

        // public TopProduct()
        // {
        // }

        public int ProductId { set; get; }
        public string ProductName { set; get; }
        public double Amount { set; get; }
        public double ProductPrice { set; get; }

        public string SupplierName { set; get; }

        public string ProductTypeName { set; get; }
        public string BillDate { set; get; }

    }
}