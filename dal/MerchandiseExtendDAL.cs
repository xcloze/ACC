using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using JuYuan.model;
using JuYuan.ui;

namespace JuYuan.dal
{
    /// <summary>
    /// 商品扩展信息
    /// </summary>
    class MerchandiseExtendDAL : MySqlUtils
    {
       
        /// <summary>
        /// 根据 商品编号/名称 和 类型 检索商品信息及扩展信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="cagegoryID"></param>
        /// <returns></returns>
        public List<MerchandiseLabelData> QueryMerchandise(string content, string cagegoryID)
        {
            DataSet ds;
            List<MerchandiseLabelData> exinfo_list = new List<MerchandiseLabelData>();

            if (string.IsNullOrEmpty(cagegoryID))
            {
                ds = ExecuteDataSet(@"select a.goods_id,a.code,a.name,a.selling_price,a.units,b.*,c.category_name from goods a,
                    goods_info_ext b,goods_category c where a.goods_id=b.goods_id and c.id=a.category and (a.code like @id or a.name like @name or a.abbr like @abbr)",
                    new MySqlParameter("@id", "%" + content + "%"),
                    new MySqlParameter("@name", "%" + content + "%"),
                    new MySqlParameter("@abbr", "%" + content + "%")
                   );
            }
            else
            {
                ds = ExecuteDataSet(@"select a.goods_id,a.code,a.name,a.selling_price,a.units,b.*,c.category_name from goods a, 
                    goods_info_ext b,splb c where a.goods_id=b.goods_id and c.id=a.category and (a.code like @id or a.name like @name or a.abbr like @abbr) and a.category=@category",
                    new MySqlParameter("@id", "%" + content + "%"),
                    new MySqlParameter("@name", "%" + content + "%"),
                    new MySqlParameter("@abbr", "%" + content + "%"),
                    new MySqlParameter("@category", cagegoryID)
                   );
            }

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    MerchandiseLabelData data = BuildLabelData(dr);
                    exinfo_list.Add(data);
                }
            }

            return exinfo_list;
        }

        private MerchandiseLabelData BuildLabelData(DataRow dr)
        {
            MerchandiseLabelData data = new MerchandiseLabelData();

            data.ID = (string)dr["goods_id"];
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                data.MerchID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MerchName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["category_name"].ToString()))
                data.Category = (string)dr["category_name"];
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                data.Uint = (string)dr["units"];
            if (!string.IsNullOrEmpty(dr["selling_price"].ToString()))
                data.UintPrice = (decimal)dr["selling_price"];
            if (!string.IsNullOrEmpty(dr["label_print_num"].ToString()))
                data.TotalPrint = (int)dr["label_print_num"];
            if (!string.IsNullOrEmpty(dr["net_content"].ToString()))
                data.NetContent = (string)dr["net_content"];
            if (!string.IsNullOrEmpty(dr["additive"].ToString()))
                data.Additive = (string)dr["additive"];
            if (!string.IsNullOrEmpty(dr["expiration"].ToString()))
                data.Expiration = (string)dr["expiration"];
            if (!string.IsNullOrEmpty(dr["material"].ToString()))
                data.Ingredient = (string)dr["material"];
           
            data.MakeDate = DateTime.Now;

            return data;
        }

        /// <summary>
        /// 检测是否存在指定商品编号的扩展信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ExsitMerchanExtend(string id)
        {
            if (string.IsNullOrEmpty(id))
                return 0;

            return Convert.ToInt32(ExecuteScalar("select count(*) from goods_info_ext where goods_id =@spid", new MySqlParameter("@spid", id)));
        }

        /// <summary>
        /// 更新商品扩展信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int UpdateMerchExtend(MerchandiseExtend data)
        {
            if (null == data)
                return 0;
            return ExecuteNonQuery(@"update goods_info_ext set label_print_num=label_print_num+@label_print_num,material=@material,additive=@additive,net_content=@net_content,expiration=@expiration,produce_dt=@produce_dt where goods_id=@goods_id",
                new MySqlParameter("@goods_id", data.MerchandiseID),
                new MySqlParameter("@label_print_num", data.LabelPrintCount),
                new MySqlParameter("@material", data.Ingredients),
                new MySqlParameter("@additive", data.Additive),
                new MySqlParameter("@net_content", data.NetContent),
                new MySqlParameter("@expiration", data.ShelfLife),
                new MySqlParameter("@produce_dt", data.ProductionDate)
            );
        }
    }
}
