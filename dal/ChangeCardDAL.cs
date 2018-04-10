using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mysqlApp.utils;
using JuYuan.model;
using MySql.Data.MySqlClient;

namespace JuYuan.dal
{
    
    class ReplaceCardRecordDB : MySqlUtils
    {
        public int SaveReplaceCardRecord(ChangeCardRecord zkjl)
        {
            return ExecuteNonQuery(@"insert into value_card_replace(member_id,card_id_new,card_id_old,state,passwd,dt) 
                   values(@member_id,@card_id_new,@card_id_old,@state,@passwd,@dt)",
                   new MySqlParameter("@hyid", zkjl.member_id),
                   new MySqlParameter("@card_id_new", zkjl.new_cardID),
                   new MySqlParameter("@card_id_old", zkjl.old_cardID),
                   new MySqlParameter("@state", zkjl.state),
                   new MySqlParameter("@passwd", zkjl.passwd),
                   new MySqlParameter("@dt", zkjl.date)
               );
        }
    }
}
