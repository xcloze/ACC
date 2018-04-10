using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using JuYuan.ui;
using JuYuan.ui.Misc;

namespace JuYuan.dal
{
    
    class MerchandiseDAL : MySqlUtils
    {        
        public List<MerchandiseData> QueryInven(string lbid, string searcher)
        {
            DataSet ds;
            List<MerchandiseData> data_list = new List<MerchandiseData>();

            if (!string.IsNullOrEmpty(lbid))
            {
                ds = ExecuteDataSet(@"select goods.goods_id,goods.code,goods.ean,goods.sell_status,goods.name,goods.units,goods.selling_price,goods.selling_vip_price,
                    goods.purchasing_price,goods.num,goods_category.category_name, ifnull(exchange.exchange_score, 0) as exchange_score
                    from goods left join goods_category on goods.category = goods_category.id 
                    left join exchange on exchange.goods_id = goods.goods_id
                    where (goods.code like @code or goods.ean like @ean or goods.name like @name) and goods_category.id = @category order by goods.code",
                    new MySqlParameter("@category", lbid),
                    new MySqlParameter("@code", "%" + searcher + "%"),
                    new MySqlParameter("@ean", "%" + searcher + "%"),
                    new MySqlParameter("@name", "%" + searcher + "%")
                );
            }
            else
            {
                ds = ExecuteDataSet(@"select goods.goods_id,goods.code,goods.ean,goods.sell_status,goods.name,goods.units,goods.selling_price,goods.selling_vip_price,
                        goods.purchasing_price,goods.num,goods_category.category_name, ifnull(exchange.exchange_score, 0) as exchange_score
                        from goods left join goods_category on goods.category = goods_category.id 
                        left join exchange on exchange.goods_id = goods.goods_id
                        where (goods.code like @code or goods.ean like @ean or goods.name like @name) order by goods.code",
                        new MySqlParameter("@code", "%" + searcher + "%"),
                        new MySqlParameter("@ean", "%" + searcher + "%"),
                        new MySqlParameter("@name", "%" + searcher + "%")
                    );
            }
           
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    MerchandiseData data = BuildMerchandiseData(dr);
                    data_list.Add(data);
                }
            }

            return data_list;
        }

 
        public Dictionary<string, string> GetChild2ParentGoodsInfo(out Dictionary<string, GoodsBase> ID2goods_dict)
        {
            Dictionary<string, string> child2parent = new Dictionary<string, string>();
            ID2goods_dict = new Dictionary<string, GoodsBase>();

            DataTable dt = ExecuteDataTable(@"select goods_id,name,ean,goods.code,category,selling_price,selling_vip_price,purchasing_price,units,product_parent_id,product_num,sell_status,goods_category.category_name from goods left join goods_category on goods.category = goods_category.id order by goods.code");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GoodsBase goods = new GoodsBase();

                    goods.GoodsID = dt.Rows[i]["goods_id"].ToString();
                    goods.GoodsName = dt.Rows[i]["name"].ToString();
                    goods.Ean = dt.Rows[i]["ean"].ToString();
                    goods.Code = dt.Rows[i]["code"].ToString();
                    goods.CategoryID = dt.Rows[i]["category"].ToString();
                    goods.CategoryName = dt.Rows[i]["category_name"].ToString();
                    goods.ParentID = dt.Rows[i]["product_parent_id"].ToString();
                    goods.Units = dt.Rows[i]["units"].ToString();

                    goods.SellPrice = Convert.ToDecimal(dt.Rows[i]["selling_price"]);
                    goods.PurchasingPrice = Convert.ToDecimal(dt.Rows[i]["purchasing_price"]);
                    goods.VIPPrice = Convert.ToDecimal(dt.Rows[i]["selling_vip_price"]);
                    goods.SplitNum = Convert.ToDecimal(dt.Rows[i]["product_num"]);
                    goods.SellState = (int)dt.Rows[i]["sell_status"];

                    ID2goods_dict.Add(goods.GoodsID, goods);                    
                    if (!string.IsNullOrEmpty(goods.ParentID))
                    {
                        child2parent.Add(goods.GoodsID, goods.ParentID);        
                    }
                }
            }

            return child2parent;
        }

        
        public decimal GetGoodsCostAmount(string id, decimal num, string child_id, decimal split)
        {
            decimal cost = 0, p_num = 0, c_num = 0;
            DataTable dt = ExecuteDataTable(@"select cost_amount,num from goods where goods_id=@id",
                new MySqlParameter("@id", id));

            DataTable cdt = ExecuteDataTable(@"select num from goods where goods_id=@id",
                new MySqlParameter("@id", child_id));

            if (dt != null && dt.Rows.Count >= 1)
            {
                cost = Convert.ToDecimal(dt.Rows[0]["cost_amount"]);
                p_num = Convert.ToDecimal(dt.Rows[0]["num"]);
            }
            if (cdt != null && cdt.Rows.Count >= 1)
            {
                c_num = Convert.ToDecimal(cdt.Rows[0]["num"]);
            }

            if ((p_num + c_num / split) != 0)
            {
                cost = num * cost / (p_num + c_num / split);
            }
            return cost;
        }

        
        public decimal GetGoodsCostAmount(string id, decimal num)
        {
            decimal cost = 0;
            DataTable dt = ExecuteDataTable(@"select (cost_amount/num) as cost_price from goods where goods_id=@id and num !=0",
                new MySqlParameter("@id", id));

            if (dt != null && dt.Rows.Count >= 1)
            {
                cost = num * Convert.ToDecimal(dt.Rows[0]["cost_price"]);
            }

            return cost;
        }

        private MerchandiseData BuildMerchandiseData(DataRow dr)
        {
            MerchandiseData data = new MerchandiseData();

            data.ID = (string)dr["goods_id"];
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                data.MerchID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MerchName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["ean"].ToString()))
                data.Ean = (string)dr["ean"];
            if (!string.IsNullOrEmpty(dr["sell_status"].ToString()))
                data.State = (int)dr["sell_status"];
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                data.Unit = (string)dr["units"];
            if (!string.IsNullOrEmpty(dr["selling_price"].ToString()))
                data.UnitPrice = (decimal)dr["selling_price"];
            if (!string.IsNullOrEmpty(dr["selling_vip_price"].ToString()))
                data.VipPrice = (decimal)dr["selling_vip_price"];
            if (!string.IsNullOrEmpty(dr["purchasing_price"].ToString()))
                data.Bid = (decimal)dr["purchasing_price"];
            if (!string.IsNullOrEmpty(dr["num"].ToString()))
                data.Amount = (float)dr["num"];
            if (!string.IsNullOrEmpty(dr["category_name"].ToString()))
                data.Category = (string)dr["category_name"];
            if (!string.IsNullOrEmpty(dr["exchange_score"].ToString()))
                data.Integral = (long)dr["exchange_score"];
           
            data.RealAmount = 0;
            data.DiffAmount = 0;
            data.Remark = "";

            return data;
        }

        public decimal GetGoodsNum(string id)
        {
            decimal count = -1;
            DataTable dt = ExecuteDataTable(@"select num from goods where goods_id=@id",
                new MySqlParameter("@id", id));

            if(dt != null &&dt.Rows.Count >= 1)
            {
                count = Convert.ToDecimal(dt.Rows[0]["num"].ToString());
            }

             return count;
        }

        public DataSet QueryBySearch(string content)
        {
            return ExecuteDataSet(@"select * from goods where product_type != 2 and sell_status != 1 and num >= 0 
            and (code like @code or goods_id=@id or ean like @ean or name like @name) order by code",
                new MySqlParameter("@id",content),
                new MySqlParameter("@code", "%" + content + "%"),
                new MySqlParameter("@ean", "%" + content + "%"),
                new MySqlParameter("@name", "%" + content + "%")
            );
        }

        public DataSet QueryDepositBySearch(string content)
        {
            return ExecuteDataSet(@"select * from goods where is_pledge='1' and state = 1 and (code like @code or ean like @ean or name like @name) order by code",
                new MySqlParameter("@code", "%" + content + "%"),
                new MySqlParameter("@ean", "%" + content + "%"),
                new MySqlParameter("@name", "%" + content + "%")
            );
        }

     
        public int UpdateProSl(MySqlConnection conn, string goods_id, float cur_num)
        {
            return ExecuteNonQuery(conn, @"update goods set num = num - @num where goods_id= @id",
                new MySqlParameter("@num", cur_num),
                new MySqlParameter("@id", goods_id)
            );
        }


        public DataSet QueryByLbIdAndSpxxAndDate(string category, string goods_name, string begin_date, string end_date, string oper)
        {
            StringBuilder temp = new StringBuilder();
            temp.Append(@"select goods_inout.id,goods_inout.dt,goods_inout.goods_id,goods.code,goods.name,goods.units,
                goods.purchasing_price,goods.selling_price,goods_inout.num,goods_inout.type,goods_inout_type.type as inout_type,
                goods.discount,goods_inout.num*goods.purchasing_price as total_price,goods_category.category_name,
                goods_inout_type.operation,goods_inout.operator_id,goods_inout.comment,goods.ean 
                from goods_inout, goods, goods_category,goods_inout_type
                where goods_inout.goods_id = goods.goods_id and goods.category = goods_category.id and goods_inout.type=goods_inout_type.id 
                and goods_inout.dt between @b_date and @e_date
                and (goods.code like @id or goods.name like @name) ");
            List<MySqlParameter> paramsList = new List<MySqlParameter>();
            paramsList.Add(new MySqlParameter("@id", "%" + goods_name + "%"));
            paramsList.Add(new MySqlParameter("@name", "%" + goods_name + "%"));
            paramsList.Add(new MySqlParameter("@b_date", begin_date));
            paramsList.Add(new MySqlParameter("@e_date", end_date));

            if (!@"所有类型".Equals(category) && !string.IsNullOrEmpty(category))
            {
                temp.Append(@" and goods_category.id = @category ");
                paramsList.Add(new MySqlParameter("@category", category));
            }
            if (!string.IsNullOrEmpty(oper))
            {
                temp.Append(@" and goods_inout.operator_id=@oper");
                UserDAL dal = new UserDAL();
                paramsList.Add(new MySqlParameter("@oper", dal.QueryByUserName(oper).OptrID));
            }
            temp.Append(@" order by goods_inout.dt desc");

            return ExecuteDataSet(temp.ToString(), paramsList.ToArray());            
        }

        public DataSet QueryBreakDownMerchData(string category, string goods_name, string b_date, string e_date, string oper)
        {
            StringBuilder temp = new StringBuilder();
            temp.Append(@"select goods_inout.id,goods_inout.dt,goods.`code`,goods.`name`,goods_category.category_name,goods.units,
                goods.selling_price,goods.purchasing_price,goods_inout.num,goods_inout.operator_id,goods_inout.`comment`,
                goods_inout_type.operation,goods_inout.type 
                from goods_inout left join goods on goods_inout.goods_id = goods.goods_id
                left join goods_category on goods.category = goods_category.id
                left join goods_inout_type on goods_inout.type = goods_inout_type.id 
                where goods_inout.type = '07' and goods_inout.dt between @begin and @end 
                and (goods.code like @code or goods.name like @name) ");

            List<MySqlParameter> paramsList = new List<MySqlParameter>();
            paramsList.Add(new MySqlParameter("@code", "%" + goods_name + "%"));
            paramsList.Add(new MySqlParameter("@name", "%" + goods_name + "%"));
            paramsList.Add(new MySqlParameter("@begin", b_date));
            paramsList.Add(new MySqlParameter("@end", e_date));

            if (!@"-1".Equals(category) && !string.IsNullOrEmpty(category))
            {
                temp.Append(@" and goods.category = @category ");
                paramsList.Add(new MySqlParameter("@category", category));
            }
            if (!string.IsNullOrEmpty(oper))
            {
                temp.Append(@" and goods_inout.operator_id=@oper");
                UserDAL dal = new UserDAL();
                paramsList.Add(new MySqlParameter("@oper", dal.QueryByUserName(oper).OptrID));
            }
            temp.Append(@" order by goods_inout.dt desc");

            return ExecuteDataSet(temp.ToString(), paramsList.ToArray());
        }

        public List<GatherStorageRecordData> QueryGoodsInOutSummary(string beginDT, string endDT, string category, string content, bool isAllGoods = false)
        {
            DataSet ds;
            if (!string.IsNullOrEmpty(category) && category != "所有类型")
            {
                ds = ExecuteDataSet(@"select goods.goods_id,goods.code,goods.ean,goods.name,goods.units,goods.num,
                    goods_category.category_name,goods.product_parent_id,goods.product_num                      
                    from goods left join goods_category on goods.category = goods_category.id 
                    where (goods.code like @code or goods.ean like @ean or goods.name like @name) and goods_category.id = @category order by goods.code",
                    new MySqlParameter("@category", category),
                    new MySqlParameter("@code", "%" + content + "%"),
                    new MySqlParameter("@ean", "%" + content + "%"),
                    new MySqlParameter("@name", "%" + content + "%")
                );
            }
            else
            {
                ds = ExecuteDataSet(@"select goods.goods_id,goods.code,goods.ean,goods.name,goods.units,goods.num,
                        goods_category.category_name,goods.product_parent_id,goods.product_num 
                        from goods left join goods_category on goods.category = goods_category.id 
                        where (goods.code like @code or goods.ean like @ean or goods.name like @name) order by goods.code",
                        new MySqlParameter("@code", "%" + content + "%"),
                        new MySqlParameter("@ean", "%" + content + "%"),
                        new MySqlParameter("@name", "%" + content + "%")
                    );
            }

            Dictionary<string, GatherStorageRecordData> goodsDict = new Dictionary<string, GatherStorageRecordData>();
            SortedSet<string> goods_set = new SortedSet<string>();
            

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    GatherStorageRecordData data = buildGatherStorageRecordData(dr);
                    goodsDict.Add(data.ID, data);
                }
            }

            if (goodsDict.Count > 0)
            {
                string goods_ids_str = "";
                foreach (string goods_id in goodsDict.Keys)
                {
                    goods_ids_str += "'" + goods_id + "',";
                }
                goods_ids_str = goods_ids_str.Substring(0, goods_ids_str.Length - 1); // 去掉最后的逗号

                ds = ExecuteDataSet(
                    @"select sales_goods.goods_id, sales_goods.num, sales_goods.total_amount 
                    from sales_goods left join sales on sales_goods.order_id = sales.order_id 
                    where goods_id in (" + goods_ids_str + @") and sales.dt between @begin and @end;
                    select exchange_record.goods_id, exchange_record.num from exchange_record
                    where goods_id in (" + goods_ids_str + @") and exchange_record.dt between @begin and @end;
                    select goods_return.goods_id, goods_return.num, goods_return.return_value 
                    from goods_return left join sales_return on goods_return.return_id = sales_return.uuid 
                    where goods_id in (" + goods_ids_str + @") and sales_return.dt between @begin and @end;
                    select goods_inout.goods_id, goods_inout.num from goods_inout
                    where goods_id in (" + goods_ids_str + @") and type = '07' and goods_inout.dt between @begin and @end;
                    select goods_inout.goods_id, goods_inout.num from goods_inout
                    where goods_id in (" + goods_ids_str + @") and type = '08' and goods_inout.dt between @begin and @end; ",
                    new MySqlParameter("@begin", beginDT),
                    new MySqlParameter("@end", endDT));

                if (null != ds && ds.Tables.Count > 4)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        string goods_id = (string)dr["goods_id"];
                        if (goodsDict.ContainsKey(goods_id))
                        {
                            goodsDict[goods_id].ConsumeAmount += (float)dr["num"];
                            goodsDict[goods_id].RealConsPay += Convert.ToDecimal(dr["total_amount"]);
                            goods_set.Add(goods_id);
                        }
                    }
                    for (int i = 0; i < ds.Tables[1].Rows.Count; ++i)
                    {
                        DataRow dr = ds.Tables[1].Rows[i];
                        string goods_id = (string)dr["goods_id"];
                        if (goodsDict.ContainsKey(goods_id))
                        {
                            goodsDict[goods_id].ExchangeAmount += (int)dr["num"];
                            goods_set.Add(goods_id);
                        }
                    }
                    for (int i = 0; i < ds.Tables[2].Rows.Count; ++i)
                    {
                        DataRow dr = ds.Tables[2].Rows[i];
                        string goods_id = (string)dr["goods_id"];
                        if (goodsDict.ContainsKey(goods_id))
                        {
                            goodsDict[goods_id].ReturnAmount += (float)dr["num"];
                            goodsDict[goods_id].RealConsPay -= Convert.ToDecimal(dr["return_value"]);
                            goods_set.Add(goods_id);
                        }
                    }
                    for (int i = 0; i < ds.Tables[3].Rows.Count; ++i)
                    {
                        DataRow dr = ds.Tables[3].Rows[i];
                        string goods_id = (string)dr["goods_id"];
                        if (goodsDict.ContainsKey(goods_id))
                        {
                            goodsDict[goods_id].BreakDownAmount += -(float)dr["num"];
                            goods_set.Add(goods_id);
                        }
                    }
                    for (int i = 0; i < ds.Tables[4].Rows.Count; ++i)
                    {
                        DataRow dr = ds.Tables[4].Rows[i];
                        string goods_id = (string)dr["goods_id"];
                        if (goodsDict.ContainsKey(goods_id))
                        {
                            goodsDict[goods_id].ExchangeAmount += -(float)dr["num"];
                            goods_set.Add(goods_id);
                        }
                    }
                }
            }

            if (isAllGoods)
            {
                return goodsDict.Values.ToList();
            }
            else
            {
                List<GatherStorageRecordData> goods_list = new List<GatherStorageRecordData>();
                foreach (var id in goods_set)
                {
                    goods_list.Add(goodsDict[id]);
                }
                return goods_list;
            }
        }

        private GatherStorageRecordData buildGatherStorageRecordData(DataRow dr)
        {
            GatherStorageRecordData data = new GatherStorageRecordData();
            data.ID = (string)dr["goods_id"];
            data.MerchID = (string)dr["code"];
            if(!string.IsNullOrEmpty(dr["ean"].ToString()))
                data.Ean = (string)dr["ean"];
            data.MerchName = (string)dr["name"]; 
            data.Uint = (string)dr["units"];
            data.Category = (string)dr["category_name"];
            data.RealAmount = (float)dr["num"];

            if (!string.IsNullOrEmpty(dr["product_parent_id"].ToString()))
                data.ParentGoodsId = (string)dr["product_parent_id"];
            else
                data.ParentGoodsId = "";

            if (!string.IsNullOrEmpty(dr["product_parent_id"].ToString()))
                data.CountPerParent = (int)dr["product_num"];
            else
                data.CountPerParent = 0;

            return data;
        }

        public List<PrizeGoodsItem> QueryPrizeGoods()
        {
            List<PrizeGoodsItem> items = new List<PrizeGoodsItem>();
            DataSet ds = ExecuteDataSet("select goods.goods_id, goods.`code`, goods.`name`, goods.units, goods.num from goods where goods.product_type = 2 and goods.num > 0");
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    PrizeGoodsItem data = buildPrizeGoodsItem(dr);
                    items.Add(data);
                }
            }
            return items;
        }

        private PrizeGoodsItem buildPrizeGoodsItem(DataRow dr)
        {
            PrizeGoodsItem item = new PrizeGoodsItem();
            item.GoodsID = (string)dr["goods_id"];
            item.Code = (string)dr["code"];
            item.Name = (string)dr["name"];
            item.Units = (string)dr["units"];
            item.Num = (float)dr["num"];
            return item;
        }
    }
} 
