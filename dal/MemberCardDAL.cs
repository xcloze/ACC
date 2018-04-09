using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using MySql.Data.MySqlClient;
using System.Data.SqlTypes;
using mysqlApp.utils;
using System.Data;

namespace JuYuan.dal
{
    /// <summary>
    /// 会员储值卡数据操作
    /// </summary>
    class MemberCardDAL : MySqlUtils
    {
        /// <summary>
        /// 根据会员卡号删除会员储值卡信息
        /// </summary>
        /// <param name="card_id">会员卡号</param>
        /// <returns></returns>
        public int DeleteValueCardByCardID(string card_id)
        {
            return ExecuteNonQuery(@"delete from value_card where card_id=@card_id or member_id=@id",
               new MySqlParameter("@card_id", card_id),
               new MySqlParameter("@id", card_id)
           );
        }

        /// <summary>
        /// 保存会员卡信息
        /// </summary>
        /// <param name="hyczk"></param>
        /// <returns></returns>
        public int SaveMemberValueCardInfo(MemberCard mem_valuecard)
        {
            return ExecuteNonQuery(@"insert into value_card(member_id,card_id,state,join_dt,passwd,remain_value,comment,is_closing,closing_dt)
                 values(@id,@card_id,@state,@join_dt,@passwd,@remain_value,@comment,@is_closing,@closing_dt,)",
                new MySqlParameter("@id", mem_valuecard.MemberID),
                new MySqlParameter("@card_id", mem_valuecard.CardID),
                new MySqlParameter("@state", mem_valuecard.CardState),
                new MySqlParameter("@join_dt", mem_valuecard.JoinDate),
                new MySqlParameter("@passwd", mem_valuecard.Password),
                new MySqlParameter("@remain_value", mem_valuecard.CardRemain),
                new MySqlParameter("@comment", mem_valuecard.Comment),
                new MySqlParameter("@is_closing", mem_valuecard.ClosingState),
                new MySqlParameter("@closing_dt", mem_valuecard.ClosingDate)
                );
        }

        /// <summary>
        /// 根据会员卡号获取会员卡信息
        /// </summary>
        /// <param name="card_id">会员卡号</param>
        /// <returns></returns>
        public MemberCard QueryMemValueCardByID(string card_id)
        {
            DataTable table = ExecuteDataTable(@"select * from value_card where card_id=@card_id", new MySqlParameter("@card_id", card_id));
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                MemberCard value_card = new MemberCard();
                value_card.MemberID = (string)row["member_id"];
                value_card.CardID = (string)row["card_id"];
                value_card.CardState = (string)row["state"];
                value_card.JoinDate = (DateTime)row["join_dt"];
                value_card.Password = (string)row["passwd"];
                value_card.CardRemain = (decimal)row["remain_value"];
                value_card.Comment = (string)row["comment"];
                value_card.ClosingState = (int)row["is_closing"];
                value_card.ClosingDate = (DateTime?)FromDBValue(row["closing_dt"]);
                return value_card;
            }
            return null;
        }


        /// <summary>
        /// 更新会员卡信息
        /// </summary>
        /// <param name="value_card"></param>
        public int UpdateMemberValueCardInfo(MemberCard value_card)
        {
            return ExecuteNonQuery(@"update value_card set state=@state,join_dt=@join_dt,passwd=@passwd,
                    remain_value=@remain_value,comment=@comment,is_closing=@is_closing,closing_dt=@closing_dt where member_id=@id ",
                    new MySqlParameter("@id", value_card.MemberID),
                    new MySqlParameter("@card_id", value_card.CardID),
                    new MySqlParameter("@state", value_card.CardState),
                    new MySqlParameter("@join_dt", value_card.JoinDate),
                    new MySqlParameter("@passwd", value_card.Password),
                    new MySqlParameter("@remain_value", value_card.CardRemain),
                    new MySqlParameter("@comment", value_card.Comment),
                    new MySqlParameter("@is_closing", value_card.ClosingState),
                   new MySqlParameter("@closing_dt", value_card.ClosingDate)
                );
        }
        
        public int ExchangeCard(string card, string newCard)
        {
            return ExecuteNonQuery(@"update value_card set card_id=@card where card_id=@oldcard",
                new MySqlParameter("@card", newCard),
                new MySqlParameter("@oldcard", card)
               );
        }

        public int UpdateCardStatus(string cardID, string status)
        {
            return ExecuteNonQuery(@"update value_card set state=@status where card_id=@id", 
                new MySqlParameter("@status", status),
                new MySqlParameter("@id", cardID)
               );
        }
    }
}
