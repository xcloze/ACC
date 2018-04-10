using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using JuYuan.ui;
using JuYuan.PayMgr;
using Model.model;

namespace JuYuan.dal
{
   
    class FailedSalesDAL : MySqlUtils
    {
        
        
        public List<ConsumeOrderData> QueryFailedConsumeInfo(string memberID)
        {
            List<ConsumeOrderData> order_list = new List<ConsumeOrderData>();
            
            DataSet ds = ExecuteDataSet(@"SELECT
                                          	order_id,
                                          	member_id,
                                          	dt,
                                          	consume_value,
                                          	score,
                                          	pledge_amount,
                                          	total_amount,
                                          	operator_id,
                                          	comment,
                                          	pay_mode
                                          FROM
                                          	sales_failed
                                          WHERE
                                          	member_id =@memberID
                                          AND recording_state = 0
                                          ORDER BY
                                          	dt DESC",
                    new MySqlParameter("@memberID", memberID)
                   );

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ConsumeOrderData data = BuildOrderData(dr);
                    if (null != data)
                    {
                        order_list.Add(data);
                    }
                }
            }

            return order_list;
        }
        
        private ConsumeOrderData BuildOrderData(DataRow dr)
        {
            ConsumeOrderData data = new ConsumeOrderData();

            data.OrderID = (string)dr["order_id"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                data.ConsumeDate = ((DateTime)dr["dt"]).ToString(@"yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(dr["total_amount"].ToString()))
                data.TotalMoney = (decimal)dr["total_amount"];
            if (!string.IsNullOrEmpty(dr["member_id"].ToString()))
                data.MemberID = (string)dr["member_id"];
            if (!string.IsNullOrEmpty(dr["score"].ToString()))
                data.Integral = (int)dr["score"];
            if (!string.IsNullOrEmpty(dr["pledge_amount"].ToString()))
                data.Deposit = (decimal)dr["pledge_amount"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                data.Operator = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }
            if (!string.IsNullOrEmpty(dr["comment"].ToString()))
                data.Remark = (string)dr["comment"];

            int pay_mode = 0;
            if (!string.IsNullOrEmpty(dr["pay_mode"].ToString()))
                pay_mode = (int)dr["pay_mode"];
            
      
            data.PayMode = @"现金";
            switch(pay_mode)
            {
                case (int)PAYMODE.PAYMODE_MEMBER_CARD:
                    data.PayMode = @"储值卡";
                    break;
                case (int)PAYMODE.PAYMODE_WEIXIN:
                    data.PayMode = @"红码";       // todo 微信 为红码支付
                    break;
                case (int)PAYMODE.PAYMODE_ALI:
                    data.PayMode = @"支付宝";
                    break;
                case (int)PAYMODE.PAYMODE_EWALLENT:
                    data.PayMode = @"会员电子钱包";
                    break;
                case (int)PAYMODE.PAYMODE_VOUCHERS:
                    data.PayMode = @"代金券";
                    break;
                case (int)PAYMODE.PAYMODE_BANKCARD:
                    data.PayMode = @"银行卡";
                    break;
                case (int)PAYMODE.PAYMODE_UNITE:
                    data.PayMode = @"联合付款";
                    break;
                default:
                    break;
            }

            return data;
        }
      
    
        public List<ConsumeMerchData> QueryGoodsByConsumeID(string consume_id)
        {
            List<ConsumeMerchData> consume_list = new List<ConsumeMerchData>();

            DataSet ds = ExecuteDataSet(@"select sales_goods_failed.uuid,sales_goods_failed.goods_id,sales_goods_failed.order_id,sales_goods_failed.price,sales_goods_failed.num,sales_goods_failed.total_amount,sales_goods_failed.pledge_amount,goods.code,goods.name,goods.units,goods.ean"
                + " from sales_goods_failed left join goods on sales_goods_failed.goods_id = goods.goods_id where sales_goods_failed.order_id = @order_id;",
                new MySqlParameter("@order_id", consume_id)
               );

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ConsumeMerchData data = BuildMerchData(dr);
                    if (null != data)
                    {
                        consume_list.Add(data);
                    }
                }
            }

            return consume_list;
        }

        private ConsumeMerchData BuildMerchData(DataRow dr)
        {
            ConsumeMerchData data = new ConsumeMerchData();
            data.RecordID = (string)dr["uuid"];
            data.OrderID = (string)dr["order_id"];
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                data.MerchID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MerchName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["ean"].ToString()))
                data.Ean = (string)dr["ean"];
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                data.Uint = (string)dr["units"];
            if (!string.IsNullOrEmpty(dr["price"].ToString()))
                data.UintPrice = (decimal)dr["price"];
            if (!string.IsNullOrEmpty(dr["num"].ToString()))
                data.Amount = (float)dr["num"];
            if (!string.IsNullOrEmpty(dr["total_amount"].ToString()))
                data.TotalMoney = (decimal)dr["total_amount"];
            if (!string.IsNullOrEmpty(dr["pledge_amount"].ToString()))
                data.Deposit = (decimal)dr["pledge_amount"];
            if (!string.IsNullOrEmpty(dr["goods_id"].ToString()))
                data.GoodsID = dr["goods_id"].ToString();

            return data;
        }
    }
    
}
