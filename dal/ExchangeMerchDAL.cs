using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using MySql.Data.MySqlClient;
using System.Data;
using mysqlApp.utils;
using JuYuan.ui;
/**
 * 兑换商品数据库操作类 
 */ 
namespace JuYuan.dal
{
    class ExchangeMerchDAL : MySqlUtils
    {
        /// <summary>
        /// 查询数据集
        /// </summary>
        /// <returns></returns>
        public List<ExchangeMerchData> QueryAll()
        {
            List<ExchangeMerchData> exchange_list = new List<ExchangeMerchData>();

            DataSet ds = ExecuteDataSet("select exchange.*,goods.units from exchange,goods where exchange.goods_id = goods.goods_id");

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ExchangeMerchData data = BuildData(dr);
                    if (null != data)
                    {
                        exchange_list.Add(data);
                    }
                }
            }

            return exchange_list;
        }

        /// <summary>
        /// 根据条件查询商品
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<ExchangeMerchData> QueryByCondition(string condition)
        {
            List<ExchangeMerchData> exchange_list = new List<ExchangeMerchData>();

            DataSet ds = ExecuteDataSet("select exchange.*,goods.units from exchange,goods where exchange.goods_id=goods.goods_id and goods_name like @condition1 or good_abbr like @condition2 or goods_code like @pccode",
                new MySqlParameter("@condition1", "%"+condition+"%"),
                new MySqlParameter("@condition2", "%"+condition+"%"),
                new MySqlParameter("@pccode", "%"+condition+"%")
            );

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ExchangeMerchData data = BuildData(dr);
                    if (null != data)
                    {
                        exchange_list.Add(data);
                    }
                }
            }

            return exchange_list;
        }
        
        private ExchangeMerchData BuildData(DataRow dr)
        {
            ExchangeMerchData data = new ExchangeMerchData();

            data.ID = (string)dr["goods_id"];
            if (!string.IsNullOrEmpty(dr["goods_code"].ToString()))
                data.MerchID = (string)dr["goods_code"];
            if (!string.IsNullOrEmpty(dr["goods_name"].ToString()))
                data.MerchName = (string)dr["goods_name"];
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                data.Uint = (string)dr["units"];
            if (!string.IsNullOrEmpty(dr["exchange_score"].ToString()))
                data.Integral = (int)dr["exchange_score"];

            return data;
        }

    }
}
