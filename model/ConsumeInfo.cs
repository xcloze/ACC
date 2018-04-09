using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 消费信息实体
    /// </summary>
    public class ConsumeInfo
    {
        public string order_id { get; set; }  //消费单号

        public string member_id { get; set; } //会员id
        /// <summary>
        /// 
        /// </summary>
        public decimal consume_value { get; set; }   //消费金额
       
        public DateTime dt { get; set; }  // 消费日期
        
        public decimal discounted_value { get; set; }   // 打折后金额

        //所得积分
        public int score { get; set; }   

        public decimal pledge_amount { get; set; }    //押金总金额
      
        public decimal total_amount { get; set; }     // 合计总金额
      
        public string comment { get; set; }  //备注
      
        public string optr { get; set; }     // 操作员
      
        public decimal from_value_card { get; set; }      //储值卡


        public int pay_mode { get; set; } //   支付方式,0-现金,1-微信,2-支付宝,4-银行卡,8-红码  9-代金卷


        public decimal pay_value { get; set; }  //支付金额
      
       
    }
}
