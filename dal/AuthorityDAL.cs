using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using TCPClientAndServer.TcpClient;

namespace JuYuan.dal
{
    /// <summary>
    /// 
    /// </summary>
    class AuthorityDAL : MySqlUtils
    {
        public static List<int> QueryPrivileges(int operator_id)
        {
            List<int> privileges = new List<int>();
            DataSet ds = ExecuteDataSet(@"select op_type from operator_privileges where operator_id=@operator_id",
                new MySqlParameter("@operator_id", operator_id));
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    privileges.Add((int)dr["op_type"]);
                }
            }
            return privileges;
        }
    }
}
