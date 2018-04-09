using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 兑换商品记录
    /// </summary>
    public class ExchangeGoodsRecord
    {
        /// <summary>
        /// 兑换商品ID
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardID { get; set; }

        public string MemberID { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string GoodsID { get; set; }
        /// <summary>
        /// 兑换日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 兑换数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 兑换积分
        /// </summary>
        public int ExchangeScore { get; set; }
        /// <summary>
        /// 剩余积分
        /// </summary>
        public int RemainScore { get; set; }
        /// <summary>
        /// 管理员
        /// </summary>
        public string Opter { get; set; }
    }
}
