using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mysqlApp.utils;
using MySql.Data.MySqlClient;
using JuYuan.model;
using System.Data;

namespace JuYuan.dal
{

    /// <summary>
    /// 分店设置信息
    /// </summary>
    class StoreDAL : MySqlUtils
    {
        /// <summary>
        /// 获取分店设置信息
        /// </summary>
        /// <returns></returns>
        public Store GetStoreInfo()
        {
            Store store = null;
            DataTable dt = ExecuteDataTable(@"select * from store");
            if (dt != null && dt.Rows.Count > 0)
            {
                store = new Store();
                store.ID = Convert.ToString(dt.Rows[0]["store_id"]);
                store.Name = Convert.ToString(dt.Rows[0]["store_name"]);
                store.NO = Convert.ToInt32(dt.Rows[0]["store_no"]);
                store.Type = Convert.ToInt32(dt.Rows[0]["store_type"]);                
            }
            return store;
        }

    }
}
