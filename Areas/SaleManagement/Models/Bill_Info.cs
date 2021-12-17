using System.Collections.Generic;

namespace App.Areas.SaleManagement.Models{
    public class Bill_Info{
        public Bill bill { set; get; }

        public DetailBill detail { set; get; }
        public List<DetailBill> detailBills { get; set; }
    }
}