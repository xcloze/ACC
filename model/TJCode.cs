using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 天鉴码信息
    /// </summary>
    class TJCode
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public string GoodsID { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 天鉴码
        /// </summary>
        public string TraceCode { get; set; }
        /// <summary>
        /// 状态 0表示入库，1表示销售
        /// </summary>
        public int Status { get; set; }  
        /// <summary>
        /// 数量(用于一批一码类型的天鉴码)
        /// </summary>
        public int CodeCount { get; set; }
        /// <summary>
        /// 是否一瓶一码，0表示一瓶一码，1表示一批一码
        /// </summary>
        public int ypym { get; set; }
        /// <summary>
        /// 是否店内消费，0表示否，1表示是
        /// </summary>
        public int dnxf { get; set; }

        public int oper_id { get; set; }
    }
}
