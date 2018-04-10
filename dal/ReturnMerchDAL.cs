using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using mysqlApp.utils;
using MySql.Data.MySqlClient;
using Model.model;

namespace JuYuan.dal
{
   
    class ReturnMerchDAL : MySqlUtils
    {
      
        public List<ui.ReturnMerchandises> QuerySalesReturnInfoByID(string return_id)
        {
            List<ui.ReturnMerchandises> data_list = new List<ui.ReturnMerchandises>();

            DataSet ds = ExecuteDataSet(@"select a.*,b.code,b.name,b.goods_id,b.units from goods_return a, goods b where return_id=@return_id and a.goods_id=b.goods_id", new MySqlParameter("@return_id", return_id));
            
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ui.ReturnMerchandises data = BuildReturnMerchData(dr);
                    data_list.Add(data);
                }
            }

            return data_list;
        }

        private ui.ReturnMerchandises BuildReturnMerchData(DataRow dr)
        {
            ui.ReturnMerchandises data = new ui.ReturnMerchandises();
            data.ID = (string)dr["uuid"];
            if (!string.IsNullOrEmpty(dr["return_id"].ToString()))
                data.OrderID = (string)dr["return_id"];
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                data.MerchID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MerchName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                data.Uint = (string)dr["units"];
            if (!string.IsNullOrEmpty(dr["price"].ToString()))
                data.UintPrice = (decimal)dr["price"];
            if (!string.IsNullOrEmpty(dr["num"].ToString()))
                data.Amount = (float)dr["num"];
            if (!string.IsNullOrEmpty(dr["return_value"].ToString()))
                data.TotalMoney = (decimal)dr["return_value"];
            if (!string.IsNullOrEmpty(dr["pledge"].ToString()))
                data.TotalDeposit = (decimal)dr["pledge"];
            if (!string.IsNullOrEmpty(dr["goods_id"].ToString()))
                data.GoodsID = (string)dr["goods_id"];

            return data;
        }
    }
}
