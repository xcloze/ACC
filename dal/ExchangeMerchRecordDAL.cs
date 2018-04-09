using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using mysqlApp.utils;
using MySql.Data.MySqlClient;
using JuYuan.ui;

namespace JuYuan.dal
{
    /// <summary>
    /// 兑换商品记录数据操作
    /// </summary>
    class ExchangeMerchRecordDAL:MySqlUtils
    {
        
        /// <summary>
        /// 添加兑换商品信息
        /// </summary>
        /// <param name="e_record"></param>
        /// <returns></returns>
        public int AddExchangeRecord(ExchangeGoodsRecord e_record)
        {
            UserDAL dal = new UserDAL();
            return ExecuteNonQuery(@"insert into exchange_record(id,card_id,member_id,goods_id,dt,num,score,score_remain,operator_id) 
                    values(@id,@card_id,@member_id,@goods_id,@dt,@num,@score,@score_remain,@operator)",
                    new MySqlParameter("@id", e_record.UUID), new MySqlParameter("@card_id", e_record.CardID),
                    new MySqlParameter("@goods_id", e_record.GoodsID), new MySqlParameter("@dt", e_record.Date),
                    new MySqlParameter("@num", e_record.Num), new MySqlParameter("@score", e_record.ExchangeScore),
                    new MySqlParameter("@score_remain", e_record.RemainScore), new MySqlParameter("@operator", dal.QueryByUserName( e_record.Opter).OptrID),
                    new MySqlParameter("@member_id", e_record.MemberID)
                );
        }

        public List<ExchangeMerchRecordData> QueryExchangeMerchRecord(string st, string et, string content, string oper)
        {
            List<ExchangeMerchRecordData> record_list = new List<ExchangeMerchRecordData>();
            DataSet ds;
            if (string.IsNullOrEmpty(oper))
            {
                // todo
                ds = ExecuteDataSet(@"select ex_record.*,g_exchange.goods_name,g_exchange.goods_code,mem.card_id,mem.person_code,mem.name,mem.mobile from exchange_record ex_record, exchange g_exchange, member mem where " +
                    @"ex_record.goods_id=g_exchange.goods_id and ex_record.member_id=mem.uuid and (ex_record.dt between @st and @et) and (mem.card_id like @card_id or mem.name like @Name or " +
                    "mem.mobile like @Mobile) order by ex_record.dt desc",
                    new MySqlParameter("@st", st),
                    new MySqlParameter("@et", et),
                    new MySqlParameter("@card_id", "%" + content + "%"),
                    new MySqlParameter("@Name", "%" + content + "%"),
                    new MySqlParameter("@Mobile", "%" + content + "%")
                   );
            }
            else
            {
                // todo
                UserDAL dal = new UserDAL();
                ds = ExecuteDataSet(@"select ex_record.*,g_exchange.goods_name,g_exchange.goods_code,mem.card_id,mem.person_code,mem.name,mem.mobile from exchange_record ex_record, exchange g_exchange, member mem where " +
                    @"ex_record.goods_id=g_exchange.goods_id and ex_record.member_id=mem.uuid and (ex_record.dt between @st and @et) and (mem.card_id like @card_id or mem.name like @Name or " +
                    "mem.mobile like @Mobile) and ex_record.operator_id=@oper order by ex_record.dt desc",
                    new MySqlParameter("@st", st),
                    new MySqlParameter("@et", et),
                    new MySqlParameter("@card_id", "%" + content + "%"),
                    new MySqlParameter("@Name", "%" + content + "%"),
                    new MySqlParameter("@Mobile", "%" + content + "%"),
                    new MySqlParameter("@oper", dal.QueryByUserName(oper).OptrID)
                   );
            }

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ExchangeMerchRecordData data = BuildExchangeData(dr);
                    record_list.Add(data);
                }
            }

            return record_list;
        }

        private ExchangeMerchRecordData BuildExchangeData(DataRow dr)
        {
            ExchangeMerchRecordData data = new ExchangeMerchRecordData();

            data.ID = (string)dr["goods_id"];
            if (!string.IsNullOrEmpty(dr["goods_code"].ToString()))
                data.MerchID = (string)dr["goods_code"];
            if (!string.IsNullOrEmpty(dr["goods_name"].ToString()))
                data.MerchName = (string)dr["goods_name"];
            if (!string.IsNullOrEmpty(dr["card_id"].ToString()))
                data.CardID = (string)dr["card_id"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MemberName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["mobile"].ToString()))
                data.Phone = (string)dr["mobile"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                data.ExchangeDate = ((DateTime)dr["dt"]).ToString(@"yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(dr["num"].ToString()))
                data.Amount = (int)dr["num"];
            if (!string.IsNullOrEmpty(dr["score"].ToString()))
                data.TotalIntegral = (int)dr["score"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                data.Operator = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }

            return data;
        }

        
    }
}
