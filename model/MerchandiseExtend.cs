using System;

namespace JuYuan.model
{
    /// <summary>
    /// 商品扩展信息
    /// </summary>
    class MerchandiseExtend
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string MerchandiseID { get; set; }
        /// <summary>
        /// 标签打印数量
        /// </summary>
        public int LabelPrintCount { get; set; }
        /// <summary>
        /// 配料（成分）
        /// </summary>
        public string Ingredients { get; set; }
        /// <summary>
        /// 添加剂
        /// </summary>
        public string Additive { get; set; }
        /// <summary>
        /// 净含量
        /// </summary>
        public string NetContent { get; set; }
        /// <summary>
        /// 保质期
        /// </summary>
        public string ShelfLife { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProductionDate { get; set; }
    }
}
