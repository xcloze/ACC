using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 消费商品信息
    /// </summary>
    class ConsumeMerch
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string Spid { get; set; }
        /// <summary>
        /// 消费流水号
        /// </summary>
        public string Xfid { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
       // public string Hykh { get; set; }
        /// <summary>
        /// 会员码
        /// </summary>
      //  public string Hyid { get; set; }
        /// <summary>
        /// 会员微信Openid
        /// </summary>
     //   public string WXOpenid { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Dj { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public float Sl { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal Hjje { get; set; }
        /// <summary>
        /// 打折率
        /// </summary>
        public float Dzl { get; set; }
        /// <summary>
        /// 折后金额
        /// </summary>
        public decimal Zhje { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit { get; set; }
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
