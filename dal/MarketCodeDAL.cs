using JuYuan.ui;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuYuan.dal
{
    class MarketCodeDAL : MySqlUtils
    {
        public List<MarketCodeData> QueryMarketCodesUnused(string goodsId)
        {
            List<MarketCodeData> marketCodes = new List<MarketCodeData>();

            DataSet ds = ExecuteDataSet(@"select code, goods_id, activate_dt, order_id from market_code where goods_id = @goods_id and order_id = ''",
                new MySqlParameter("@goods_id", goodsId));
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    MarketCodeData marketCode = buildMarketCodeData(dr);
                    if (null != marketCode)
                    {
                        marketCodes.Add(marketCode);
                    }
                }
            }
            return marketCodes;
        }

        private MarketCodeData buildMarketCodeData(DataRow row)
        {
            MarketCodeData marketCode = new MarketCodeData();
            marketCode.Code = (string)row["code"];
            marketCode.CodeForView = marketCode.Code;
            marketCode.GoodsID = (string)row["goods_id"];
            marketCode.ActivateDatetime = ((DateTime)row["activate_dt"]).ToString(@"yyyy-MM-dd HH:mm:ss");
            marketCode.OrderID = (string)row["order_id"];

            return marketCode;
        }

        //public void ActivateMarketCodes(string goodsId, HashSet<string> codes)
        //{
        //    foreach (string code in codes)
        //    {
        //        ExecuteNonQuery(@"insert into market_code(code, goods_id, activate_dt, order_id) values(@code, @goods_id, now(), '')
        //            on duplicate key update goods_id = @goods_id, activate_dt = now()", 
        //            new MySqlParameter("@code", code),
        //            new MySqlParameter("@goods_id", goodsId)
        //            );
        //    }
        //}

        //public void RemoveMarketCodes(HashSet<string> codes)
        //{
        //    foreach (string code in codes)
        //    {
        //        ExecuteNonQuery(@"delete from market_code where code = @code", new MySqlParameter("@code", code));
        //    }            
        //}

        //public void UpdateOrderIDForMarketCodes(string orderId, HashSet<string> codes)
        //{
        //    foreach (string code in codes)
        //    {
        //        ExecuteNonQuery(@"insert into market_code(code, goods_id, activate_dt, order_id) values(@code, '', '1970-01-01 00:00:00', @order_id)
        //            on duplicate key update order_id = @order_id", 
        //            new MySqlParameter("@code", code),
        //            new MySqlParameter("@order_id", orderId)
        //            );
        //    }
        //}
    }
}
