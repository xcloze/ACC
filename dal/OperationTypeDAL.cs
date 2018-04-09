using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JuYuan.model;
using mysqlApp.utils;
using MySql.Data.MySqlClient;
using Model.model;

namespace JuYuan.dal
{
    class OperationTypeDAL : MySqlUtils
    {
        public List<OperationType> SelectAll(int type)
        {
            List<OperationType> list = new List<OperationType>();

            DataTable dt = ExecuteDataTable(@"select * from goods_inout_type where type=@type", new MySqlParameter("@type", type));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OperationType opeType = new OperationType();
                DataRow dr = dt.Rows[i];
                opeType.ID = (GoodsInOutType)dr["id"];
                opeType.Operation = (string)dr["operation"];
                opeType.Type = (int)dr["type"];
                list.Add(opeType);
            }

            return list;
        }

    }
}
