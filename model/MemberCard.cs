using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 会员卡信息，包括0是折扣卡,1储值卡
    /// </summary>
    public class MemberCard
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public string MemberID { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardID { get; set; }
        
        /// <summary>
        /// 卡状态
        /// </summary>
        public string CardState { get; set; }
        /// <summary>
        /// 加入日期
        /// </summary>
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 卡内余额
        /// </summary>
        public decimal CardRemain { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
       
        /// <summary>
        /// 截止状态
        /// </summary>
        public int ClosingState { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? ClosingDate { get; set; }
        
    }
}
