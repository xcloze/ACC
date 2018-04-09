using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 门店信息
    /// </summary>
    class Store
    {
        /// <summary>
        /// 门店码
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 门店全名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public int NO { get; set; }
        /// <summary>
        /// 门店类型
        /// </summary>
        public int Type { get; set; }        
    }
}
