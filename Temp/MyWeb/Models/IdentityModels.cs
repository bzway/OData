using OpenData.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OpenData.MyWeb.Models
{

    public class MyUser : EntityBase
    {
        public string UserName
        {
            get
            {
                if (this.ContainsKey("UserName") && this["UserName"] != null)
                {
                    return this["UserName"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["UserName"] = value;
            }
        }
        public string Password
        {
            get
            {
                if (this.ContainsKey("Password") && this["Password"] != null)
                {
                    return this["Password"].ToString();
                }
                return string.Empty;
            }
            set
            {
                this["Password"] = value;
            }
        }
        public bool IsAdmin
        {
            get
            {
                if (this.ContainsKey("IsAdmin") && this["IsAdmin"] != null)
                {
                    return (bool)this["IsAdmin"];
                }
                return false;
            }
            set
            {
                this["IsAdmin"] = value;
            }
        }
    }
    public enum TickStatus
    {
        请选择,
        客票已作废,
        客票已退票,
        有效未使用,
        已办理登机牌,
        客票已使用,
        客票已挂起,
        客票已被换开,
    }
    public enum TicketMode
    {
        请选择,
        申请,
        普通,
        免票,
        包机,
    }
    public class MyOrderEntity : EntityBase
    {
        [Display(Name = "订单来源")]
        public string Source { get { if (this.ContainsKey("Source") && this["Source"] != null) { return this["Source"].ToString(); } return string.Empty; } set { this["Source"] = value; } }
        [Display(Name = "订单号码")]
        public string OrderNumber { get { if (this.ContainsKey("OrderNumber") && this["OrderNumber"] != null) { return this["OrderNumber"].ToString(); } return string.Empty; } set { this["OrderNumber"] = value; } }
        [Display(Name = "航段")]
        public string FligthLine { get { if (this.ContainsKey("FligthLine") && this["FligthLine"] != null) { return this["FligthLine"].ToString(); } return string.Empty; } set { this["FligthLine"] = value; } }

        [Display(Name = "航班号")]
        public string FlightNumber { get { if (this.ContainsKey("FlightNumber") && this["FlightNumber"] != null) { return this["FlightNumber"].ToString(); } return string.Empty; } set { this["FlightNumber"] = value; } }

        [Display(Name = "去程起飞日期")]
        public string GoDate { get { if (this.ContainsKey("GoDate") && this["GoDate"] != null) { return this["GoDate"].ToString(); } return string.Empty; } set { this["GoDate"] = value; } }
        [Display(Name = "起飞时间")]
        public string FlightTime { get { if (this.ContainsKey("FlightTime") && this["FlightTime"] != null) { return this["FlightTime"].ToString(); } return string.Empty; } set { this["FlightTime"] = value; } }

        [Display(Name = "航班号")]
        public string FlightNumberBack { get { if (this.ContainsKey("FlightNumberBack") && this["FlightNumberBack"] != null) { return this["FlightNumberBack"].ToString(); } return string.Empty; } set { this["FlightNumberBack"] = value; } }
        [Display(Name = "回程起飞时间")]
        public string BackDate { get { if (this.ContainsKey("BackDate") && this["BackDate"] != null) { return this["BackDate"].ToString(); } return string.Empty; } set { this["BackDate"] = value; } }
        [Display(Name = "起飞时间")]
        public string FlightTimeBack { get { if (this.ContainsKey("FlightTimeBack") && this["FlightTimeBack"] != null) { return this["FlightTimeBack"].ToString(); } return string.Empty; } set { this["FlightTimeBack"] = value; } }

        [Display(Name = "录入员姓名")]
        public string Operator { get { if (this.ContainsKey("Operator") && this["Operator"] != null) { return this["Operator"].ToString(); } return string.Empty; } set { this["Operator"] = value; } }
        [Display(Name = "票号")]
        public string TicketNumber { get { if (this.ContainsKey("TicketNumber") && this["TicketNumber"] != null) { return this["TicketNumber"].ToString(); } return string.Empty; } set { this["TicketNumber"] = value; } }
        [Display(Name = "大编码")]
        public string BigNumber { get { if (this.ContainsKey("BigNumber") && this["BigNumber"] != null) { return this["BigNumber"].ToString(); } return string.Empty; } set { this["BigNumber"] = value; } }
        [Display(Name = "小编码")]
        public string SmallNumber { get { if (this.ContainsKey("SmallNumber") && this["SmallNumber"] != null) { return this["SmallNumber"].ToString(); } return string.Empty; } set { this["SmallNumber"] = value; } }
        [Display(Name = "开票外联")]
        public string ExternalNumber { get { if (this.ContainsKey("ExternalNumber") && this["ExternalNumber"] != null) { return this["ExternalNumber"].ToString(); } return string.Empty; } set { this["ExternalNumber"] = value; } }
        [Display(Name = "开票方式")]
        public TicketMode TicketMode
        {
            get
            {
                if (this.ContainsKey("TicketMode") && this["TicketMode"] != null)
                {
                    return (TicketMode)Enum.ToObject(typeof(TicketMode), this["TicketMode"]);
                } return TicketMode.请选择;
            }
            set { this["TicketMode"] = value; }
        }
        [Display(Name = "收款价格")]
        public string Price { get { if (this.ContainsKey("Price") && this["Price"] != null) { return this["Price"].ToString(); } return string.Empty; } set { this["Price"] = value; } }
        [Display(Name = "乘机人姓名")]
        public string FullName { get { if (this.ContainsKey("FullName") && this["FullName"] != null) { return this["FullName"].ToString(); } return string.Empty; } set { this["FullName"] = value; } }
        [Display(Name = "护照号码")]
        public string PassportNumber { get { if (this.ContainsKey("PassportNumber") && this["PassportNumber"] != null) { return this["PassportNumber"].ToString(); } return string.Empty; } set { this["PassportNumber"] = value; } }
        [Display(Name = "票号状态")]
        public TickStatus Status
        {
            get
            {
                if (this.ContainsKey("Status") && this["Status"] != null)
                {
                    return (TickStatus)Enum.ToObject(typeof(TickStatus), this["Status"]);
                }
                return TickStatus.请选择;
            }
            set { this["Status"] = value; }
        }
        [Display(Name = "改期日期")]
        public string ChangeToDate { get { if (this.ContainsKey("ChangeToDate") && this["ChangeToDate"] != null) { return this["ChangeToDate"].ToString(); } return string.Empty; } set { this["ChangeToDate"] = value; } }
        [Display(Name = "改期收款")]
        public string ChangePrice { get { if (this.ContainsKey("ChangePrice") && this["ChangePrice"] != null) { return this["ChangePrice"].ToString(); } return string.Empty; } set { this["ChangePrice"] = value; } }
        [Display(Name = "外联退款")]
        public string Return { get { if (this.ContainsKey("Return") && this["Return"] != null) { return this["Return"].ToString(); } return string.Empty; } set { this["Return"] = value; } }
        [Display(Name = "退款金额")]
        public string ReturnNumber { get { if (this.ContainsKey("ReturnNumber") && this["ReturnNumber"] != null) { return this["ReturnNumber"].ToString(); } return string.Empty; } set { this["ReturnNumber"] = value; } }
        [Display(Name = "备注")]
        public string Remark { get { if (this.ContainsKey("Remark") && this["Remark"] != null) { return this["Remark"].ToString(); } return string.Empty; } set { this["Remark"] = value; } }

    }

    public class MyOrder
    {
        [Key]
        public string UUID { get; set; }


        [Display(Name = "订单来源")]
        public string Source { get; set; }
        [Display(Name = "订单号码")]
        public string OrderNumber { get; set; }

        [Display(Name = "航段")]
        public string FligthLine { get; set; }
        [Display(Name = "航班号")]
        public string FlightNumber { get; set; }

        [Display(Name = "去程起飞日期")]
        public string GoDate { get; set; }
        [Display(Name = "起飞时间")]
        public string FlightTime { get; set; }

        [Display(Name = "航班号")]
        public string FlightNumberBack { get; set; }


        [Display(Name = "回程起飞时间")]
        public string BackDate { get; set; }
        [Display(Name = "起飞时间")]
        public string FlightTimeBack { get; set; }


        [Display(Name = "录入员姓名")]
        public string Operator { get; set; }
        [Display(Name = "票号")]
        public string TicketNumber { get; set; }
        [Display(Name = "大编码")]
        public string BigNumber { get; set; }
        [Display(Name = "小编码")]
        public string SmallNumber { get; set; }
        [Display(Name = "开票外联")]
        public string ExternalNumber { get; set; }
        [Display(Name = "开票方式")]
        public TicketMode TicketMode { get; set; }
        [Display(Name = "收款价格")]
        public string Price { get; set; }
        [Display(Name = "乘机人姓名")]
        public string FullName { get; set; }
        [Display(Name = "护照号码")]
        public string PassportNumber { get; set; }
        [Display(Name = "票号状态")]
        public TickStatus Status { get; set; }
        [Display(Name = "改期日期")]
        public string ChangeToDate { get; set; }
        [Display(Name = "改期收款")]
        public string ChangePrice { get; set; }
        [Display(Name = "外联退款")]
        public string Return { get; set; }
        [Display(Name = "退款金额")]
        public string ReturnNumber { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
    }
}