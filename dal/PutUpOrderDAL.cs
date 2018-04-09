using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using Model.model;

namespace JuYuan.dal
{
    class PutUpOrderDAL : MySqlUtils
    {
        public DataSet QueryUpOrderByMemID(string memberID)
        {
            return ExecuteDataSet(@"select * from put_up_order where member_id=@memberID and state='0'", new MySqlParameter("@memberID", memberID));
        }

        public PutUpOrderRecord QueryUpOrderByID(string orderID)
        {
            PutUpOrderRecord data = null;

            if (string.IsNullOrEmpty(orderID))
            {
                return data;
            }

            DataTable dt = ExecuteDataTable(@"select * from put_up_order where id=@orderID and state='0'", new MySqlParameter("@orderID", orderID));
            if (dt.Rows.Count > 0)
            {
                data = BuildData(dt.Rows[0]);
            }

            return data;
        }
        
        public int GetOrderCount()
        {
            return Convert.ToInt32(ExecuteScalar(@"select count(id) from put_up_order"));
        }

        private PutUpOrderRecord BuildData(DataRow dr)
        {
            PutUpOrderRecord data = new PutUpOrderRecord();

            data.ID = (string)dr["id"];
            if (!string.IsNullOrEmpty(dr["member_id"].ToString()))
                data.MemberID = (string)dr["member_id"];
            if (!string.IsNullOrEmpty(dr["state"].ToString()))
                data.Status = (int)dr["state"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                data.PutUpDate = (DateTime)dr["dt"];
            if (!string.IsNullOrEmpty(dr["cost_value"].ToString()))
                data.ConsumeMoney = (decimal)dr["cost_value"];
            if (!string.IsNullOrEmpty(dr["pledge_value"].ToString()))
                data.Deposit = (decimal)dr["pledge_value"];
            if (!string.IsNullOrEmpty(dr["total_value"].ToString()))
                data.TotalMoney = (decimal)dr["total_value"];
            if (!string.IsNullOrEmpty(dr["score"].ToString()))
                data.Integral = (int)dr["score"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
                data.OperatorID = (int)dr["operator_id"];
            if (!string.IsNullOrEmpty(dr["comment"].ToString()))
                data.Remark = (string)dr["comment"];

            return data;
        }
    }
}
