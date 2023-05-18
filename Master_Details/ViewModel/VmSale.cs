using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monthly_Final_Maser_Details.ViewModel
{
    public class VmSale
    {
        public long SaleId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string AttachmentURL { get; set; }

        public List<VmSaleDetail> SaleDetails { get; set; }
        = new List<VmSaleDetail>();
        public string[] ProductName { get; set; }
        public decimal?[] Qty { get; set; }
        public decimal?[] Price { get; set; }

        public class VmSaleDetail
        {
            public long SaleDetailId { get; set; }

            public long? SaleId { get; set; }

            public string ProductName { get; set; }

            public decimal? Qty { get; set; }

            public decimal? Price { get; set; }
        }
    }
}