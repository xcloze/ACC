using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using JuYuan.ui;
using JuYuan.ui.Misc;
using Model.model;

namespace JuYuan.dal
{
    class CustomGoodsDal : MySqlUtils
    {

        /// <summary>
        /// 查询所有非正常上架商品
        /// </summary>
        /// <returns></returns>
        public List<CustomGoods> GetAllCustomGoods()
        {
            DataTable dt = ExecuteDataTable(@"select goods.goods_id,goods.code,goods.ean,goods.sell_status,goods.name,goods.units,goods.selling_price,goods.selling_vip_price,
                        goods.purchasing_price,goods.category,goods_category.category_name,goods.pledge_value,goods.source_type,goods.product_parent_id,goods.product_num
                        from goods left join goods_category on goods.category = goods_category.id 
                        where goods.source_type != 0");

            if(dt == null || dt.Rows.Count <= 0)
            {
                return null;
            }

            List<CustomGoods> custom_goods = new List<CustomGoods>();

            for(int i=0; i< dt.Rows.Count; i++)
            {
                CustomGoods goods = BulidCustomGoods(dt.Rows[i]);
                if(goods != null)
                {
                    custom_goods.Add(goods);
                }
            }

            return custom_goods;
        }

      
        /// <summary>
        /// 删除指定商品
        /// </summary>
        /// <param name="goods_id"></param>
        public void DeleteCustomGoodsByGoodsID(string goods_id)
        {
            //ExecuteNonQuery(@"delete from goods where goods_id=@goods_id",
            //    new MySqlParameter("@goods_id", goods_id));
        }


        /// <summary>
        /// 构建显示数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private CustomGoods BulidCustomGoods(DataRow dr)
        {
            CustomGoods goods = new CustomGoods();

            goods.ID = dr["goods_id"].ToString();
            goods.Code = dr["code"].ToString();
            if (!string.IsNullOrEmpty(dr["ean"].ToString()))
                goods.Ean = (string)dr["ean"];
            goods.Name = dr["name"].ToString();
            goods.CategoryStr = dr["category_name"].ToString();
            goods.Category = dr["category"].ToString();
            goods.Price = Convert.ToDecimal(dr["selling_price"]);
            goods.VIPPrice = Convert.ToDecimal(dr["selling_vip_price"]);
            goods.PRHPrice = Convert.ToDecimal(dr["purchasing_price"]);
            goods.Units = dr["units"].ToString();
            goods.SellState = (int)dr["sell_status"];
            goods.SourceType = (int)dr["source_type"];

            if (!string.IsNullOrEmpty(dr["pledge_value"].ToString()))
                goods.Pledge = Convert.ToDecimal(dr["pledge_value"]);
            if (!string.IsNullOrEmpty(dr["product_parent_id"].ToString()))
                goods.ParentID = dr["product_parent_id"].ToString();
            if (!string.IsNullOrEmpty(dr["product_num"].ToString()))
                goods.SplitNum = (int)dr["product_num"];

            return goods;
        }
    }
}
