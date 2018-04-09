using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 消费商品信息
    /// </summary>
    class ConsumeGoodsInfo
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public string goods_id { get; set; }
        /// <summary>
        /// 消费流水号
        /// </summary>
        public string consume_id { get; set; }
      
        /// <summary>
        /// 单价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public float num { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal total_money { get; set; }
        /// <summary>
        /// 打折率
        /// </summary>
        public float discount_rate { get; set; }
        /// <summary>
        /// 折后金额
        /// </summary>
        public decimal final_money { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal deposit { get; set; }
        /// <summary>
        /// 通知后厨状态，0未通知，1已通知
        /// </summary>
        //public int Tzhczt { get; set; }
        /// <summary>
        /// 是否即制产品
        /// </summary>
        public int imm_pro { get; set; }
    }
}
