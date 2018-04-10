using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mysqlApp.utils;
using System.Data;
using JuYuan.model;
using MySql.Data.MySqlClient;

namespace JuYuan.dal
{
    class BFDAL:MySqlUtils
    {
        
        public BF Query()
        {
            DataTable table = ExecuteDataTable(@"select * from backup");
           if (table.Rows.Count > 0)
           {
               BF bf = new BF();
               bf.Zdbf = int.Parse(table.Rows[0]["auto"] + "");
               bf.Sj = int.Parse(table.Rows[0]["interval"] + "");
               bf.Lj = (string)table.Rows[0]["save_path"];
               bf.Blts = int.Parse(table.Rows[0]["save_days"] + "");
               return bf;
           }
           return null;
        }

        public int Add(BF bf)
        {
            return ExecuteNonQuery(@"insert into backup(auto,`interval`,save_path,save_days) values(@auto,@interval,@save_path,@save_days)",
                new MySqlParameter("@auto", bf.Zdbf), new MySqlParameter("@interval", bf.Sj),
                new MySqlParameter("@save_path", bf.Lj),
                new MySqlParameter("@save_days", bf.Blts)
            );
        }

       
        public int DeleteAll()
        {
            return ExecuteNonQuery(@"delete from backup");
        }
    }
}
