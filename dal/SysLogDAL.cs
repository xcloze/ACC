using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using mysqlApp.utils;
using MySql.Data.MySqlClient;
using System.Data;
using Model.model;

namespace JuYuan.dal
{
    public class OperatorLogUnit
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public DateTime DT { get; set; }
    }

    /// <summary>
    /// 日志数据操作
    /// </summary>
    class OperationLogDAL:MySqlUtils
    {
       
        public List<OperatorLogUnit> QueryOperatorOnDuty(string beginDate, string endDate, string name)
        {
            DataSet ds;
            if (string.IsNullOrEmpty(name) || @"全部".Equals(name))
            {
                ds = ExecuteDataSet(@"select type, operator_id, dt from operator_log where type in (0,1) and dt between @begin and @end order by serial",
                    new MySqlParameter("@begin", beginDate),
                    new MySqlParameter("@end", endDate));
            }
            else
            {
                ds = ExecuteDataSet(@"select type, operator_id, dt from operator_log where type in (0,1) and operator = @name and dt between @begin and @end order by serial",
                    new MySqlParameter("@name", name),
                    new MySqlParameter("@begin", beginDate),
                    new MySqlParameter("@end", endDate));
            }

            List<OperatorLogUnit> logs = new List<OperatorLogUnit>();
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    OperatorLogUnit log = new OperatorLogUnit();
                    log.Type = (int)dr["type"];
                    UserDAL dal = new UserDAL();
                    log.Name = dal.QueryByOptrID((int)dr["operator_id"]).name;
                    log.DT = (DateTime)dr["dt"];

                    logs.Add(log);
                }
            }
            return logs;
        }
           
        public List<OperationLog> QueryAllEx(string st, string et, string oper, string content)
        {
            DataTable dt;
            List<OperationLog> data_list = new List<OperationLog>();
            UserDAL dal = new UserDAL();
            if (string.IsNullOrEmpty(oper) || @"全部".Equals(oper)) 
            {
                dt = ExecuteDataTable(@"select * from operator_log where content like @rznr and dt between @kssj and @jssj order by dt desc ",
                    new MySqlParameter("@rznr", "%" + content + "%"), new MySqlParameter("@kssj", st),
                    new MySqlParameter("@jssj", et));
            }
            else
            {
                dt = ExecuteDataTable(@"select * from operator_log where content like @rznr and operator_id=@oper and dt between @kssj and @jssj order by dt desc ",
                   new MySqlParameter("@rznr", "%" + content + "%"), new MySqlParameter("@kssj", st),
                   new MySqlParameter("@oper", dal.QueryByUserName(oper).OptrID),
                   new MySqlParameter("@jssj", et));
            }

            if (null != dt)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    OperationLog data = BuildData(dr);
                    if (null != data)
                        data_list.Add(data);
                }
            }

            return data_list;

        }

        private OperationLog BuildData(DataRow dr)
        {
            OperationLog data = new OperationLog();

            data.ID = (int)dr["serial"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
            {
                data.Time = (DateTime)dr["dt"];
            }
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                data.OperatorID = (int)dr["operator_id"];
            }
            if (!string.IsNullOrEmpty(dr["content"].ToString()))
            {
                data.Content = (string)dr["content"];
            }

            return data;
        }
        
    }
}
