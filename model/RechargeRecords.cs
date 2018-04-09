using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 会员缴费信息
    /// </summary>
    public class RechargeRecords
    {
        /// <summary>
        /// 缴费id
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardID { get; set; }

        public string MemberID { get; set; }

        public string ActiveNO { get; set; }
        /// <summary>
        /// 缴费金额
        /// </summary>
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 缴费日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public int OpetrID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 实际缴费
        /// </summary>
        public decimal ActualPayMoney { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 储值卡
        /// </summary>
        public decimal ValueCard { get; set; }
        /// <summary>
        /// 信用卡
        /// </summary>
        public decimal BankCard { get; set; }
        /// <summary>
        /// 代金券
        /// </summary>
        public decimal Voucher { get; set; }
    }
}
