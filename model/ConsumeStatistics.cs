using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 消费统计
    /// </summary>
    class ConsumeStatistics
    {
        public decimal Xjxf { get; set; }  //现金消费
        public decimal Czkxf { get; set; } //储值卡消费
        public decimal Xykxf { get; set; } //信用卡消费
        public decimal Djqxf { get; set; } //代金券消费
        public decimal Xjjf { get; set; }  //现金交费
        public decimal Xykjf { get; set; } //信用卡交费
        public decimal Djqjf { get; set; } //代金券交费
    }
}
