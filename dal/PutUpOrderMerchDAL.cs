using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using Model.model;

namespace JuYuan.dal
{
    class PutUpOrderMerchDAL : MySqlUtils
    {
        public DataSet QueryMerch(string orderID)
        {
            return ExecuteDataSet(@"select a.*, b.code, b.name, b.units  from put_up_order_goods a, goods b where a.order_id=@orderID and a.goods_id=b.goods_id", 
                new MySqlParameter("@orderID", orderID)
               );
        }
    }
}
