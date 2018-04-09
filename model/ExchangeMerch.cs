using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 兑换商品信息
    /// </summary>
    class ExchangeGoods
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public String GoodsID
        {
            get;
            set;
        }
        
        /// <summary>
        /// 商品数量
        /// </summary>
        public int Num
        {
            get;
            set;
        }
        /// <summary>
        /// 兑换积分
        /// </summary>
        public int ExchangeScore
        {
            get;
            set;
        }
    }
}
