using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using mysqlApp.utils;
using MySql.Data.MySqlClient;
using System.Data;

namespace JuYuan.dal
{
    
    class RechargeRecordsDAL : MySqlUtils
    {
       
      
        public int SaveMemberPaymentInfo(RechargeRecords payment)
        {
            return ExecuteNonQuery(@"insert into member_recharge(uuid,card_id,recharge_value,dt,promotion_value,operator_id,comment,pay_value,from_cash,from_value_card,from_bank_card,from_voucher)
                    values(@id,@card_id,@recharge_value,@dt,@promotion_value,@operator_id,@comment,@pay_value,@from_cash,@from_value_card,@from_bank_card,@from_voucher)",
                    new MySqlParameter("@id",payment.UUID),
                    new MySqlParameter("@card_id", payment.CardID),
                    new MySqlParameter("@recharge_value", payment.PayMoney),
                    new MySqlParameter("@dt",payment.Date),
                    new MySqlParameter("@promotion_value", payment.DiscountMoney),
                    new MySqlParameter("@operator_id", payment.OpetrID),
                    new MySqlParameter("@comment", payment.Comment),
                    new MySqlParameter("@pay_value", payment.ActualPayMoney),
                    new MySqlParameter("@from_cash", payment.Cash),
                    new MySqlParameter("@from_value_card", payment.ValueCard),
                    new MySqlParameter("@from_bank_card", payment.BankCard),
                    new MySqlParameter("@from_voucher", payment.Voucher)
                );
        }

        public List<ui.RechargeRecordData> QueryMemberPayment(string jfks, string jfjs, string czy, string kmc, string hykh)
        {
            StringBuilder temp = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            temp.Append(" select mem.card_id,mem.name,mem.mobile,mem.card_name,mem.level_name,a.uuid,a.member_id,a.dt,");
            temp.Append("a.recharge_value,a.pay_value,a.operator_id,a.comment from member_recharge a left join member mem on a.card_id = mem.card_id ");
            temp.Append(" where a.dt BETWEEN @jfks and @jfjs ");
            parameters.Add(new MySqlParameter("@jfks", jfks));
            parameters.Add(new MySqlParameter("@jfjs", jfjs));
            if (hykh != "")
            {
                temp.Append(" and (mem.card_id like @card_id or mem.name like @name or mem.mobile like @mobile)");
                parameters.Add(new MySqlParameter("@card_id", "%" + hykh + "%"));
                parameters.Add(new MySqlParameter("@name", "%" + hykh + "%"));
                parameters.Add(new MySqlParameter("@mobile", "%" + hykh + "%"));
            }
            if (czy != "所有操作员" && !string.IsNullOrEmpty(czy))
            {
                temp.Append(" and a.operator_id = @czy");
                UserDAL dal = new UserDAL();
                parameters.Add(new MySqlParameter("@czy", dal.QueryByUserName(czy).OptrID));
            }
            if (kmc != "所有卡等级" && !string.IsNullOrEmpty(kmc))
            {
                temp.Append(" and mem.level_name = @kmc");
                parameters.Add(new MySqlParameter("@kmc", kmc));
            }
            temp.Append("  order by a.dt desc  ");
            System.Data.DataTable dt = ExecuteDataTable(temp.ToString(), parameters.ToArray());

            List<ui.RechargeRecordData> data_list = new List<ui.RechargeRecordData>();

            if (null != dt)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    ui.RechargeRecordData data = BuildRechargeRecordData(dr);
                    data_list.Add(data);
                }
            }

            return data_list;
        }

        private ui.RechargeRecordData BuildRechargeRecordData(DataRow dr)
        {
            ui.RechargeRecordData data = new ui.RechargeRecordData();

            data.ID = (string)dr["uuid"];
            if (!string.IsNullOrEmpty(dr["member_id"].ToString()))
                data.MemberID = (string)dr["member_id"];
            if (!string.IsNullOrEmpty(dr["card_id"].ToString()))
                data.CardID = (string)dr["card_id"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MemberName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["mobile"].ToString()))
                data.Phone = (string)dr["mobile"];
            if (!string.IsNullOrEmpty(dr["recharge_value"].ToString()))
                data.RechargeMoney = (decimal)dr["recharge_value"];
            if (!string.IsNullOrEmpty(dr["pay_value"].ToString()))
                data.RealMoney = (decimal)dr["pay_value"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                data.RechargeDate = (DateTime)dr["dt"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                data.Operator = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }
            if (!string.IsNullOrEmpty(dr["comment"].ToString()))
                data.Remark = (string)dr["comment"];

            return data;
        }

        public List<ui.RechargeRecordData> QueryRechargeRecord(string memberID, string st, string et, string czy)
        {
            StringBuilder temp = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            temp.Append("select * from member_recharge where member_id=@memberID"); //jfrq BETWEEN @Ksrq and @Jsrq 
            parameters.Add(new MySqlParameter("@memberID", memberID));
            if (!string.IsNullOrEmpty(st) && !string.IsNullOrEmpty(et))
            {
                temp.Append(@" and (dt BETWEEN @Ksrq and @Jsrq )");
                parameters.Add(new MySqlParameter("@Ksrq", st));
                parameters.Add(new MySqlParameter("@Jsrq", et));
            }
            if (!string.IsNullOrEmpty(czy))
            {
                temp.Append(" and operator_id = @Czy ");
                UserDAL dal = new UserDAL();
                parameters.Add(new MySqlParameter("@czy", dal.QueryByUserName(czy).OptrID));
            }

            DataSet ds = ExecuteDataSet(temp.ToString(), parameters.ToArray());
            List<ui.RechargeRecordData> data_list = new List<ui.RechargeRecordData>();

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ui.RechargeRecordData data = BuildRechargeRecordData(dr);
                    data_list.Add(data);
                }
            }

            return data_list;
        }

    
    }
}
