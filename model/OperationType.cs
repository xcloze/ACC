using Model.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuYuan.model
{
    /// <summary>
    /// 出入库类型
    /// </summary>
    class OperationType
    {
        public GoodsInOutType ID { get; set; }

        public string Operation { get; set; }

        public int Type { get; set; }

        public string Remark { get; set; }
    }
}
