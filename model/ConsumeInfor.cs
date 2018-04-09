using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 消费信息实体
    /// </summary>
    public class ConsumeInfor
    {


       /* `order_id` varchar(255) NOT NULL COMMENT '消费单号',
          `member_id` varchar(255) DEFAULT '0' COMMENT '会员id',
          `consume_value` decimal(8,2) DEFAULT NULL COMMENT '消费金额',
          `dt` datetime DEFAULT NULL COMMENT '消费日期',
          `discounted_value` decimal(8,2) DEFAULT NULL COMMENT '打折后金额',
          `score` int(11) DEFAULT NULL COMMENT '所得积分',
          `pledge_amount` decimal(8,2) DEFAULT NULL COMMENT '押金总金额',
          `total_amount` decimal(8,2) DEFAULT NULL COMMENT '合计总金额',
          `comment` varchar(2000) DEFAULT NULL COMMENT '备注',
          `operator` varchar(255) DEFAULT NULL COMMENT '操作员',
          `pay_mode` int(4) DEFAULT '0' COMMENT '支付方式,0-现金,1-微信,2-支付宝,4-银行卡,8-红码',
          `pay_value` decimal(8,2) DEFAULT '0.00' COMMENT '支付金额',
          `from_value_card` decimal(8,2) DEFAULT NULL COMMENT '储值卡',
         */
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
      
        public string _operator { get; set; }     // 操作员
      
        public decimal from_value_card { get; set; }      //储值卡


        public int pay_mode { get; set; } //   支付方式,0-现金,1-微信,2-支付宝,4-银行卡,8-红码  9-代金卷


        public decimal pay_value { get; set; }  //支付金额
      
       
    }
}
