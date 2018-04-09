using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using JuYuan.model;
using mysqlApp.utils;

namespace JuYuan.dal
{
    class InventoryDAL : MySqlUtils
    {
        public int AddInvent(Inventory s)
        {
            try
            {
                /*return ExecuteNonQuery(@"insert into inventory (id, pro_id,theoret,actual,date,operator,remark) values (@id, @pro_id,@theoret,@actual,@date,@operator,@remark)",
                    new MySqlParameter("@id", s.Id),
                    new MySqlParameter("@pro_id", s.ProId),
                    new MySqlParameter("@theoret", s.Theoret),
                    new MySqlParameter("@catual", s.Actual),
                    new MySqlParameter("@date", s.Date),
                    new MySqlParameter("@operator", s.Operator),
                    new MySqlParameter("@remark", s.Remark)
                );*/
                return ExecuteNonQuery(@"insert into stock_count (uuid, goods_id,in_theory,in_fact,check_dt,operator,comment) values (@id,@pro_id,@theoret,@actual,@date,@operator,@remark);",
                    new MySqlParameter("@id", s.Id),
                    new MySqlParameter("@pro_id", s.ProId),
                    new MySqlParameter("@theoret", s.Theoret),
                    new MySqlParameter("@actual", s.Actual),
                    new MySqlParameter("@date", s.Date),
                    new MySqlParameter("@operator", s.Operator),
                    new MySqlParameter("@remark", s.Remark)
                );
            }
            catch (Exception ex)
            {
                JuYuan.utils.ErrorCatchUtil.showErrorMsg(ex);
                return 0;
            }
        }

        public int UpdateInvent(Inventory s)
        {
            return ExecuteNonQuery(@"update stock_count set in_theory=@theoret,in_fact=@actual,check_dt=@date,operator=@operator,comment=@remark where uuid=@id",
                new MySqlParameter("id", s.Id),
                new MySqlParameter("@theoret", s.Theoret),
                new MySqlParameter("@catual", s.Actual),
                new MySqlParameter("@date", s.Date),
                new MySqlParameter("@operator", s.Operator),
                new MySqlParameter("@remark", s.Remark)
            );
        }

        public int DeleteInvent(string id)
        {
            return ExecuteNonQuery(@"delete from stock_count where uuid=@id", new MySqlParameter("@id", id));
        }

        /*
        public DataSet QueryInvent(string st, string et, string lbid, string searcher)
        {
            if (!"-1".Equals(lbid))
            {
                return ExecuteDataSet(@"select a.*, b.pc_code, b.spmc from inventory a, spxx b, splb c where a.pro_id=b.id and b.lbid=c.id and (a.date between @st and @et) and (b.pc_code like @pc_code or b.spmc like @spmc or b.jm like @jm) and b.lbid=@lbid",
                    new MySqlParameter("@st", st),
                    new MySqlParameter("@et", et),
                    new MySqlParameter("@lbid", lbid),
                    new MySqlParameter("@pc_code", searcher),
                    new MySqlParameter("@spmc", searcher),
                    new MySqlParameter("@jm", searcher)
                );
            }
            return ExecuteDataSet(@"select a.*, b.pc_code, b.spmc from inventory a, spxx b, splb c where a.pro_id=b.id and b.lbid=c.id and (a.date between @st and @et) and (b.pc_code like @pc_code or b.spmc like @spmc or b.jm like @jm)", 
                new MySqlParameter("@st", st),
                new MySqlParameter("@et", et)
            );
        }
         */
    }
}
