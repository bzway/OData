using OpenData.Data;
using System;

namespace OpenData.Site.Entity
{
    public class Transaction : BaseEntity
    {
        public string TransactionCode { get; set; }

        public string MemberCode { get; set; }

        public string ProductCode { get; set; }

        public string ShopCode { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        public decimal Remission { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime TransactionTime { get; set; }

        public string Remark { get; set; }
    }

}
