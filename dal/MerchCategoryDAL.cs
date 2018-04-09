using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using System.Data.SqlClient;
using JuYuan.utils;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
/**
 * 商品类别数据库操作类
 */ 
namespace JuYuan.dal
{
    class MerchCategoryDAL : MySqlUtils
    {
        
        /// <summary>
        /// 查询全部类型
        /// </summary>
        /// <returns></returns>
        public List<GoodsCategory> SelectAll()
        {
            List<GoodsCategory> list = new List<GoodsCategory>();
            GoodsCategory s = new GoodsCategory();
            s.CategoryId = "-1";
            s.CategoryName = "所有类型";
            list.Add(s);
            try
            {
                DataTable table = ExecuteDataTable(@"select * from goods_category");

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    GoodsCategory rt = new GoodsCategory();
                    DataRow row = table.Rows[i];
                    rt.CategoryId = (string)row["id"];
                    rt.CategoryName = (string)row["category_name"];

                    list.Add(rt);
                }
            }
            catch (Exception ex)
            {
                JuYuan.utils.ErrorCatchUtil.showErrorMsg(ex);
            }

            return list;
        }
    }
}
