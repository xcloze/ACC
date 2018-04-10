using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using mysqlApp.utils;
using MySql.Data.MySqlClient;
using JuYuan.ui;
using Model.model;

namespace JuYuan.dal
{
    
    class ConsumeMerchDAL : MySqlUtils
    {
       
       
        public List<ConsumeMerchData> QueryGoodsByConsumeID2(string consume_id)
        {
            List<ConsumeMerchData> consume_list = new List<ConsumeMerchData>();

            DataSet ds = ExecuteDataSet(@"select sales_goods.uuid,sales_goods.goods_id,sales_goods.order_id,
                    sales_goods.price,sales_goods.num,sales_goods.total_amount,sales_goods.pledge_amount,goods.code,goods.name,goods.units,goods.ean"
                + " from sales_goods left join goods on sales_goods.goods_id = goods.goods_id where sales_goods.order_id = @order_id;"
                + "select goods_return.goods_id, goods_return.num from goods_return left join sales_return on goods_return.return_id = sales_return.uuid where sales_return.order_id = @order_id",
                new MySqlParameter("@order_id", consume_id)
               );
            
            if (null != ds && ds.Tables.Count >= 2)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ConsumeMerchData data = BuildMerchData(dr, ds.Tables[1].Rows);
                    if (null != data)
                    {
                        consume_list.Add(data);
                    }
                }
            }

            return consume_list;
        }
        

  
        private ConsumeMerchData BuildMerchData(DataRow dr, DataRowCollection goodsReturned)
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

            data.NumReturned = 0;
            for (int i = 0; i < goodsReturned.Count; ++i)
            {
                DataRow row = goodsReturned[i];
                if ((string)row["goods_id"] == data.GoodsID)
                {
                    data.NumReturned += (int)(float)row["num"];
                }
            }
         
            data.NumRemain = (int)data.Amount - data.NumReturned;
            if (data.NumRemain < 0)
            {
                data.NumRemain = 0;
                data.MoneyRemain = 0;
            }
            else
            {
                data.MoneyRemain = data.UintPrice * data.NumRemain;
            }
            
            return data;
        }
        
    }
}
