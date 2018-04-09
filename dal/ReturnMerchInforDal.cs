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
    /// <summary>
    /// 退货信息数据处理
    /// </summary>
    class ReturnMerchInforDal : MySqlUtils
    {
        public List<ui.ReturnMerchRecord> QueryReturnMerchRecord(string memberId, string st, string et, string opera)
        {
            List<ui.ReturnMerchRecord> data_list = new List<ui.ReturnMerchRecord>();
            DataSet ds;
            UserDAL dal = new UserDAL();
            if (string.IsNullOrEmpty(opera))
            {
                ds = ExecuteDataSet(@"select * from sales_return where member_id=@id and (dt BETWEEN @st and @et)",
                    new MySqlParameter("@id", memberId),
                    new MySqlParameter("@st", st),
                    new MySqlParameter("@et", et)
                   );
            }
            else
            {
                ds = ExecuteDataSet(@"select * from sales_return where member_id=@id and (dt BETWEEN @st and @et) and operator_id=@operator",
                    new MySqlParameter("@id", memberId),
                    new MySqlParameter("@st", st),
                    new MySqlParameter("@et", et),
                    new MySqlParameter("@operator", dal.QueryByUserName(opera).OptrID)
                   );
            }

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ui.ReturnMerchRecord record = BuildRecordData(dr);
                    data_list.Add(record);
                }
            }

            return data_list;
        }

        private ui.ReturnMerchRecord BuildRecordData(DataRow dr)
        {
            ui.ReturnMerchRecord record = new ui.ReturnMerchRecord();
            record.ReturnID = (string)dr["uuid"];
            if (!string.IsNullOrEmpty(dr["order_id"].ToString()))
                record.OrderID = (string)dr["order_id"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                record.ReturnDate = (DateTime)dr["dt"];
            if (!string.IsNullOrEmpty(dr["member_id"].ToString()))
                record.MemberID = (string)dr["member_id"];
            if (!string.IsNullOrEmpty(dr["total"].ToString()))
                record.TotalMoney = (decimal)dr["total"];
            if (!string.IsNullOrEmpty(dr["return_value"].ToString()))
                record.TotalMerchMoney = (decimal)dr["return_value"];
            if (!string.IsNullOrEmpty(dr["pledge"].ToString()))
                record.TotalDeposit = (decimal)dr["pledge"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                record.Operator = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }
            if (!string.IsNullOrEmpty(dr["comment"].ToString()))
                record.Remark = (string)dr["comment"];

            return record;
        }
        

        public List<ui.ReturnMerchRecord> QueryReturnMerchRecord2(string content, string st, string et, string opera)
        {
            List<ui.ReturnMerchRecord> data_list = new List<ui.ReturnMerchRecord>();
            DataSet ds;
            UserDAL dal = new UserDAL();
            if (string.IsNullOrEmpty(opera))
            {
                ds = ExecuteDataSet(@"select a.*,b.card_id,b.name,b.mobile from sales_return a, member b where a.member_id = b.uuid and (b.card_id like @Hykh or " +
                    "b.name like @Xm or b.mobile like @Dh) and (dt BETWEEN @st and @et)",
                    new MySqlParameter("@Hykh", "%" + content + "%"),
                    new MySqlParameter("@Xm", "%" + content + "%"),
                    new MySqlParameter("@Dh", "%" + content + "%"),
                    new MySqlParameter("@st", st),
                    new MySqlParameter("@et", et)
                   );
            }
            else
            {
                ds = ExecuteDataSet(@"select a.*,b.card_id,b.name,b.mobile from sales_return a, member b where a.member_id = b.uuid and (b.card_id like @Hykh or " +
                    "b.name like @Xm or b.mobile like @Dh) and (dt BETWEEN @st and @et) and operator_id=@operator",
                    new MySqlParameter("@Hykh", "%" + content + "%"),
                    new MySqlParameter("@Xm", "%" + content + "%"),
                    new MySqlParameter("@Dh", "%" + content + "%"),
                    new MySqlParameter("@st", st),
                    new MySqlParameter("@et", et),
                    new MySqlParameter("@operator",dal.QueryByUserName(opera).OptrID)
                   );
            }

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ui.ReturnMerchRecord record = BuildRecordData(dr);
                    data_list.Add(record);
                }
            }

            return data_list;
        }
        
    }
}
