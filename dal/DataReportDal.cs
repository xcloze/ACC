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

namespace JuYuan.dal
{
    class DataReportDal:MySqlUtils
    {
        // 查询消费商品明细
        public List<DataReportConsumeGoods> QueryConsumeGoods(string begin, string end, string content)
        {
            List<DataReportConsumeGoods> ret = new List<DataReportConsumeGoods>();
            UserDAL dal = new UserDAL();
            DataSet ds = new DataSet();
            if (content != null && content != "")
                ds = ExecuteDataSet(@"select sales.dt, sales_goods.goods_id, sales_goods.num, goods.name, goods.ean,sales_goods.total_amount, sales_goods.uuid,
                   sales.operator_id ,pay_mode,goods.code, goods.units , sales.order_id  
                    from sales_goods left join sales on sales_goods.order_id = sales.order_id 
                    left join goods on sales_goods.goods_id = goods.goods_id where (sales.dt  BETWEEN @start_time and @end_time) and sales.operator_id=@optr order by sales.dt desc",
                    new MySqlParameter("@start_time",  begin),
                    new MySqlParameter("@end_time", end),
                    new MySqlParameter("@optr", dal.QueryByUserName(content).OptrID));
            else
                ds = ExecuteDataSet(@"select sales.dt, sales_goods.goods_id, sales_goods.num, goods.name, goods.ean,sales_goods.total_amount, sales_goods.uuid,
                   sales.operator_id ,pay_mode,goods.code, goods.units , sales.order_id  
                    from sales_goods left join sales on sales_goods.order_id = sales.order_id 
                    left join goods on sales_goods.goods_id = goods.goods_id where sales.dt BETWEEN @start_time and @end_time order by sales.dt desc",
                    new MySqlParameter("@start_time", begin),
                    new MySqlParameter("@end_time", end));


            if(ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++ )
                {
                    DataRow dr = dt.Rows[i];
                    DataReportConsumeGoods goods = BuildSalesGoodsInfo(dr);
                    if (goods != null && goods.GoodsID != "" && goods.GoodsID != null)
                        ret.Add(goods);
                }
            }
            // todo 退货逻辑
            if (content != null && content != "")
                ds = ExecuteDataSet(@"select sales_return.dt, sales_return.return_mode, goods_return.goods_id, goods_return.num, goods.name, goods.ean,goods_return.return_value, goods_return.uuid,
                    sales_return.operator_id ,goods.code, goods.units , goods_return.return_id 
                    from goods_return left join sales_return on goods_return.return_id = sales_return.uuid 
                    left join goods on goods_return.goods_id = goods.goods_id where (sales_return.dt  BETWEEN @start_time and @end_time) and sales_return.operator_id=@optr order by sales_return.dt desc",
                    new MySqlParameter("@start_time", begin),
                    new MySqlParameter("@end_time", end),
                    new MySqlParameter("@optr", dal.QueryByUserName(content).OptrID));
            else
                ds = ExecuteDataSet(@"select sales_return.dt, sales_return.return_mode, goods_return.goods_id, goods_return.num, goods.name, goods.ean,goods_return.return_value, goods_return.uuid,
                    sales_return.operator_id,goods.code, goods.units , goods_return.return_id
                    from goods_return left join sales_return on goods_return.return_id = sales_return.uuid 
                    left join goods on goods_return.goods_id = goods.goods_id where (sales_return.dt  BETWEEN @start_time and @end_time) order by sales_return.dt desc",
                    new MySqlParameter("@start_time", begin),
                    new MySqlParameter("@end_time", end));

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    DataReportConsumeGoods goods = BuildReturnGoodsInfo(dr);
                    if (goods != null && goods.GoodsID != "" && goods.GoodsID != null)
                        ret.Add(goods);
                }
            }

            return ret;
        }

        public List<DataReportPayType> QueryConsumeStatistics(string s_time, string e_time, string opter)
        {
            List<DataReportPayType> list = new List<DataReportPayType>();
            DataSet ds = new DataSet();
            DataSet return_dt = new DataSet();

            //张宇
            // 现金
            ds = ExecuteDataSet(@"SELECT total_amount , pay_mode from sales where (dt  BETWEEN @start_time and @end_time)",
                new MySqlParameter("@start_time", s_time),
                new MySqlParameter("@end_time", e_time));

            return_dt = ExecuteDataSet(@"SELECT return_value , return_mode from sales_return  where (dt  BETWEEN @start_time and @end_time)",
                new MySqlParameter("@start_time", s_time),
                new MySqlParameter("@end_time", e_time));

         


            decimal[] moneyArray = new decimal[4];
            int[] moneyCount = new int[4];
            decimal[] returnmoneyArray = new decimal[4];
            int[] returnmoneyCount = new int[4];

            if (ds.Tables.Count > 0)
            {

                foreach (DataTable dtable in ds.Tables)
                {
                    foreach (DataRow drRow in dtable.Rows)
                    {
                        decimal money = Convert.ToDecimal(drRow["total_amount"].ToString());
                        int paymode = Convert.ToInt32(drRow["pay_mode"].ToString());

                        switch (paymode)
                        {
                            case (int)ui.PAYMODEFORNEW.PAYMODEFORNEW_CASH:
                                moneyCount[0]++;
                                moneyArray[0] += money;
                                break;
                            case (int)ui.PAYMODEFORNEW.PAYMODEFORNEW_BANK:
                                moneyCount[1]++;
                                moneyArray[1] += money;
                                break;
                            case (int)ui.PAYMODEFORNEW.PAYMODEFORNEW_REDCODE:
                                moneyCount[2]++;
                                moneyArray[2] += money;
                                break;
                            default:
                                moneyCount[3]++;
                                moneyArray[3] += money;
                                break;

                        }
                    }
                }
            }

            if (return_dt.Tables.Count > 0)
            {
                foreach (DataTable dtable in return_dt.Tables)
                {
                    foreach (DataRow drRow in dtable.Rows)
                    {
                        decimal money = Convert.ToDecimal(drRow["return_value"].ToString());
                        int paymode = Convert.ToInt32(drRow["return_mode"].ToString());

                        switch (paymode)
                        {
                            case (int)ui.PAYMODEFORNEW.PAYMODEFORNEW_CASH:
                                returnmoneyCount[0]++;
                                returnmoneyArray[0] += money;
                                break;
                            case (int)ui.PAYMODEFORNEW.PAYMODEFORNEW_BANK:
                                 returnmoneyCount[1]++;
                                returnmoneyArray[1] += money;
                                break;
                            case (int)ui.PAYMODEFORNEW.PAYMODEFORNEW_REDCODE:
                                  returnmoneyCount[2]++;
                                returnmoneyArray[2] += money;
                                break;
                            default:
                                 returnmoneyCount[3]++;
                                returnmoneyArray[3] += money;
                                break;

                        }
                    }

                }
            }

            string[] useType = { "现金", "银行卡", "红码", "其它" };

            for (int i = 0; i < 4; ++i)
            {
                DataReportPayType pay = new DataReportPayType();
                pay.PayValue = moneyArray[i];
                pay.Num = moneyCount[i];
                pay.Opter = opter;
                pay.PayType = useType[i];
                pay.ReturnNum = returnmoneyCount[i];
                pay.ReturnValue = returnmoneyArray[i];
                pay.Amount = moneyArray[i] - returnmoneyArray[i];
                list.Add(pay);
            }
                    
           
                

            return list;
        }
        private DataReportConsumeGoods BuildSalesGoodsInfo(DataRow dr)
        {
            DataReportConsumeGoods goods = new DataReportConsumeGoods();

            // 构造数据
            goods.ID = (string)dr["order_id"];
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                goods.GoodsID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                goods.GoodsName = (string)dr["name"];
			if (!string.IsNullOrEmpty(dr["ean"].ToString()))
				goods.Ean = (string)dr["ean"];
			if (!string.IsNullOrEmpty(dr["num"].ToString()))
                goods.Num = Convert.ToInt32( dr["num"].ToString());
            if (!string.IsNullOrEmpty(dr["total_amount"].ToString()))
                goods.Amount = Convert.ToDecimal( dr["total_amount"].ToString());
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                goods.Opter = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                goods.Units = dr["units"].ToString();
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                goods.Date = ((DateTime)dr["dt"]).ToString();

            int pay_mode = 0;
            if (!string.IsNullOrEmpty(dr["pay_mode"].ToString()))
                pay_mode = Convert.ToInt32(dr["pay_mode"].ToString());

            
            goods.PayType = PayMode.getStringbyMode(pay_mode);

            return goods;
        }

        private DataReportConsumeGoods BuildReturnGoodsInfo(DataRow dr)
        {
            DataReportConsumeGoods goods = new DataReportConsumeGoods();

            // 构造数据
            goods.ID = (string)dr["return_id"];
            goods.PayType = "现金";
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                goods.GoodsID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                goods.GoodsName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["num"].ToString()))
                goods.Num = -Convert.ToInt32(dr["num"].ToString());
            if (!string.IsNullOrEmpty(dr["return_value"].ToString()))
                goods.Amount = -Convert.ToDecimal(dr["return_value"].ToString());
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                goods.Opter = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                goods.Units = dr["units"].ToString();
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                goods.Date = ((DateTime)dr["dt"]).ToString();
            if (!string.IsNullOrEmpty(dr["return_mode"].ToString()))
            {
                int mode = (int)dr["return_mode"];
                if(mode == (int)PAYMODEFORNEW.PAYMODEFORNEW_BANK)
                    goods.PayType = "银行卡";
                else if(mode == (int)PAYMODEFORNEW.PAYMODEFORNEW_REDCODE)
                    goods.PayType = "红码";
            }

            return goods;
        }


        /////////

        public List<VIPConsumeGoods> QueryVIPConsumeGoods(string bt, string et)
        {
            List<VIPConsumeGoods> list = new List<VIPConsumeGoods>();

            DataTable dt = ExecuteDataTable(@"select sales.dt, sales_goods.goods_id, sales_goods.num,sales_goods.price, goods.name,goods.ean,goods.units,goods.selling_price,goods.selling_vip_price,sales_goods.total_amount, sales_goods.uuid, sales_goods.order_id,
                    sales.pay_mode, sales.operator_id ,goods.code,member.name as vip_name
                    from sales_goods left join sales on sales_goods.order_id = sales.order_id 
                    left join member on sales.member_id = member.uuid  
                    left join goods on sales_goods.goods_id = goods.goods_id where member.vip_card_id !='' and sales.dt BETWEEN @start_time and @end_time order by sales.dt desc",
                    new MySqlParameter("@start_time", bt),
                    new MySqlParameter("@end_time", et));

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(BuildVIPConsumeGoodsInfo(dt.Rows[i]));
                }
            }

            // 暂时未处理退货 todo*

            return list;
        }


        private Dictionary<string, string> child2parent = new Dictionary<string, string>();
        private Dictionary<string, GoodsBase> ID2goods_dict;
        public List<StockSellInventory> QueryStockSellSaveGoods(string st, string et)
        {
            List<StockSellInventory> data_list = new List<StockSellInventory>();
            Child2ParentGoods();

            // 期前库存
            Dictionary<string, GoodsStock> periodDict = QueryAllGoodsStockByTime(st);           // 期前
            Dictionary<string, GoodsStock> breakageDict = QueryAllGoodsBreakage(st, et);        // 报损
            Dictionary<string, GoodsStock> callOutDict = QueryAllGoodsCallOut(st, et);          // 调出
            Dictionary<string, GoodsStock> callInDict = QueryAllGoodsCallIn(st, et);            // 调入
            Dictionary<string, GoodsStock> replenishDict = QueryAllGoodsReplenish(st, et);      // 入库
            Dictionary<string, GoodsStock> sellDict = QueryAllGoodsSellInfoByTime(st, et);      // 销售
            Dictionary<string, GoodsStock> endofDict = QueryAllGoodsStockByTime(et);            // 期末.

            foreach (var kv in ID2goods_dict)
            {
                if (child2parent.ContainsKey(kv.Key))   // 子商品都转为父商品库存及成本进行处理
                    continue;
                StockSellInventory data = new StockSellInventory();

                data.GoodsID = kv.Value.GoodsID;
                data.GoodsCode = kv.Value.Code;
                data.GoodsName = kv.Value.GoodsName;
                data.Ean = kv.Value.Ean;
                data.Units = kv.Value.Units;
                data.CategoryName = kv.Value.CategoryName;

                if (periodDict.ContainsKey(kv.Value.GoodsID))
                {
                    data.BeginNum = periodDict[kv.Value.GoodsID].Num;
                    data.BeginAmount = Decimal.Round(periodDict[kv.Value.GoodsID].CostAmount, 2);
                    data.BeginExpectAmount = Math.Round(periodDict[kv.Value.GoodsID].Num * kv.Value.SellPrice, 2);
                }

                if (breakageDict.ContainsKey(kv.Value.GoodsID))
                {
                    data.ProfitLossNum = breakageDict[kv.Value.GoodsID].Num;
                    data.ProfitLossAmount = Math.Round(breakageDict[kv.Value.GoodsID].CostAmount, 2);
                }

                if (callOutDict.ContainsKey(kv.Value.GoodsID))
                {
                    data.CallOutNum = callOutDict[kv.Value.GoodsID].Num;
                    data.CallOutAmount = Math.Round(callOutDict[kv.Value.GoodsID].CostAmount, 2);
                }

                if (callInDict.ContainsKey(kv.Value.GoodsID))
                {
                    data.CallInNum = callInDict[kv.Value.GoodsID].Num;
                    data.CallInAmount = Math.Round(callInDict[kv.Value.GoodsID].CostAmount, 2);
                }

                if (replenishDict.ContainsKey(kv.Value.GoodsID))
                {
                    data.InNum = replenishDict[kv.Value.GoodsID].Num;
                    data.InAmount = Math.Round(replenishDict[kv.Value.GoodsID].CostAmount, 2);
                    data.InExpectAmount = Math.Round(data.InNum * kv.Value.SellPrice, 2);
                }

                if (sellDict.ContainsKey(kv.Value.GoodsID))
                {
                    if (sellDict[kv.Value.GoodsID].Num != 0)
                    {
                        data.SellNum = sellDict[kv.Value.GoodsID].Num;
                        data.SellAmount = Math.Round(sellDict[kv.Value.GoodsID].ActulSaleAmount, 2);
                        data.SellPrice = Math.Round(data.SellAmount / data.SellNum, 2);
                        data.SellCost = Math.Round(sellDict[kv.Value.GoodsID].CostAmount, 2);
                        data.SellGrossMargin = Math.Round(data.SellAmount - data.SellCost, 2);
                        data.RateOfMargin = Math.Round(data.SellGrossMargin / data.SellAmount, 2);
                    }
                }

                if (endofDict.ContainsKey(kv.Value.GoodsID))
                {
                    data.EndPeriodNum = endofDict[kv.Value.GoodsID].Num;
                    data.EndPeriodAmount = Decimal.Round(endofDict[kv.Value.GoodsID].CostAmount, 2);
                    data.EndExpectAmount = Math.Round(data.EndPeriodNum * kv.Value.SellPrice, 2);
                }

                data_list.Add(data);
            }

            return data_list;
        }


        private void Child2ParentGoods()
        {
            MerchandiseDAL dal_goods = new MerchandiseDAL();
            child2parent = dal_goods.GetChild2ParentGoodsInfo(out ID2goods_dict);
        }

        /// <summary>
        /// 计算某一时刻的库存量
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private Dictionary<string, GoodsStock> QueryAllGoodsStockByTime(string t)
        {
            Dictionary<string, GoodsStock> id2Stock = new Dictionary<string, GoodsStock>();

            DataTable dt = ExecuteDataTable(@"
                            SELECT
                            	goods_id,
                            	num,
                                cost_amount 
                            FROM
                            	goods");

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string goods_id = dt.Rows[i]["goods_id"].ToString();

                    if (ID2goods_dict.ContainsKey(goods_id) && child2parent.ContainsKey(goods_id))                 // 关联商品 子商品切换回父商品
                    {
                        if (id2Stock.ContainsKey(child2parent[goods_id]))   // 已经存在父商品
                        {
                            id2Stock[child2parent[goods_id]].Num += Convert.ToDecimal(dt.Rows[i]["num"]) / ID2goods_dict[goods_id].SplitNum;
                            id2Stock[child2parent[goods_id]].CostAmount += Convert.ToDecimal(dt.Rows[i]["cost_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[child2parent[goods_id]]);
                            item.Num = Convert.ToDecimal(dt.Rows[i]["num"]) / ID2goods_dict[goods_id].SplitNum;
                            item.CostAmount += Convert.ToDecimal(dt.Rows[i]["cost_amount"]);

                            id2Stock.Add(child2parent[goods_id], item);
                        }
                        continue;
                    }

                    // 非关联商品 或 父商品
                    if (ID2goods_dict.ContainsKey(goods_id))
                    {
                        if (id2Stock.ContainsKey(goods_id))
                        {
                            id2Stock[goods_id].Num += Convert.ToDecimal(dt.Rows[i]["num"]);
                            id2Stock[goods_id].CostAmount += Convert.ToDecimal(dt.Rows[i]["cost_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[goods_id]);

                            item.Num = Convert.ToDecimal(dt.Rows[i]["num"]);
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["cost_amount"]);

                            id2Stock.Add(goods_id, item);
                        }
                    }
                }
            }

            string now = DateTime.Now.ToLocalTime().ToString();

            Dictionary<string, GoodsStock> breakageDict = QueryAllGoodsBreakage(t, now);        // 报损
            Dictionary<string, GoodsStock> callOutDict = QueryAllGoodsCallOut(t, now);          // 调出
            Dictionary<string, GoodsStock> callInDict = QueryAllGoodsCallIn(t, now);            // 调入
            Dictionary<string, GoodsStock> replenishDict = QueryAllGoodsReplenish(t, now);      // 入库
            Dictionary<string, GoodsStock> sellDict = QueryAllGoodsSellInfoByTime(t, now);      // 销售

            foreach (var kv in id2Stock)
            {
                if (breakageDict.ContainsKey(kv.Key))
                {
                    kv.Value.Num += breakageDict[kv.Key].Num;
                    kv.Value.CostAmount += breakageDict[kv.Key].CostAmount;
                }

                if (callOutDict.ContainsKey(kv.Key))
                {
                    kv.Value.Num += callOutDict[kv.Key].Num;
                    kv.Value.CostAmount += callOutDict[kv.Key].CostAmount;
                }

                if (callInDict.ContainsKey(kv.Key))
                {
                    kv.Value.Num -= callInDict[kv.Key].Num;
                    kv.Value.CostAmount -= callInDict[kv.Key].CostAmount;
                }

                if (replenishDict.ContainsKey(kv.Key))
                {
                    kv.Value.Num -= replenishDict[kv.Key].Num;
                    kv.Value.CostAmount -= replenishDict[kv.Key].CostAmount;
                }

                if (sellDict.ContainsKey(kv.Key))
                {
                    kv.Value.Num += sellDict[kv.Key].Num;
                    kv.Value.CostAmount += sellDict[kv.Key].CostAmount;
                }

            }

            return id2Stock;
        }

        /// <summary>
        /// 报损货物统计 盘点出库
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private Dictionary<string, GoodsStock> QueryAllGoodsBreakage(string st, string et)
        {
            Dictionary<string, GoodsStock> id2Stock = new Dictionary<string, GoodsStock>();

            DataTable dt = ExecuteDataTable(@"SELECT
                            	goods_id,
                            	sum(num) AS sum_num,
                            	sum(cost_amount) AS total_amount 
                            FROM 
                            	goods_inout 
                            WHERE
                                (`type`='07' or `type`='09') and 
                            	dt BETWEEN @st AND @et
                            GROUP BY 
                            	goods_id",
                            new MySqlParameter("@st", st),
                            new MySqlParameter("@et", et));

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string goods_id = dt.Rows[i]["goods_id"].ToString();

                    if (ID2goods_dict.ContainsKey(goods_id) && child2parent.ContainsKey(goods_id))                 // 关联商品 子商品切换回父商品
                    {
                        if (id2Stock.ContainsKey(child2parent[goods_id]))   // 已经存在父商品
                        {
                            id2Stock[child2parent[goods_id]].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            id2Stock[child2parent[goods_id]].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[child2parent[goods_id]]);
                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                            id2Stock.Add(child2parent[goods_id], item);
                        }
                        continue;
                    }

                    // 非关联商品 或 父商品
                    if (ID2goods_dict.ContainsKey(goods_id))
                    {
                        if (id2Stock.ContainsKey(goods_id))
                        {
                            id2Stock[goods_id].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            id2Stock[goods_id].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[goods_id]);

                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_amount"]);

                            id2Stock.Add(goods_id, item);
                        }
                    }
                }
            }

            return id2Stock;
        }

        /// <summary>
        /// 调出货物统计
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private Dictionary<string, GoodsStock> QueryAllGoodsCallOut(string st, string et)
        {
            Dictionary<string, GoodsStock> id2Stock = new Dictionary<string, GoodsStock>();

            DataTable dt = ExecuteDataTable(@"
                            SELECT
                            	goods_id,
                            	sum(num) AS sum_num,
                            	sum(cost_amount) AS total_amount 
                            FROM
                            	goods_inout
                            WHERE
                                `type`='05' 
                            AND	dt BETWEEN @st AND @et
                            GROUP BY
                            	goods_id",
                            new MySqlParameter("@st", st),
                            new MySqlParameter("@et", et));

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string goods_id = dt.Rows[i]["goods_id"].ToString();

                    if (ID2goods_dict.ContainsKey(goods_id) && child2parent.ContainsKey(goods_id))                 // 关联商品 子商品切换回父商品
                    {
                        if (id2Stock.ContainsKey(child2parent[goods_id]))   // 已经存在父商品
                        {
                            id2Stock[child2parent[goods_id]].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            id2Stock[child2parent[goods_id]].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[child2parent[goods_id]]);
                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                            id2Stock.Add(child2parent[goods_id], item);
                        }
                        continue;
                    }

                    // 非关联商品 或 父商品
                    if (ID2goods_dict.ContainsKey(goods_id))
                    {
                        if (id2Stock.ContainsKey(goods_id))
                        {
                            id2Stock[goods_id].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            id2Stock[goods_id].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[goods_id]);

                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_amount"]);

                            id2Stock.Add(goods_id, item);
                        }
                    }
                }
            }

            return id2Stock;
        }

        /// <summary>
        /// 调入货物统计
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private Dictionary<string, GoodsStock> QueryAllGoodsCallIn(string st, string et)
        {
            Dictionary<string, GoodsStock> id2Stock = new Dictionary<string, GoodsStock>();

            DataTable dt = ExecuteDataTable(@"
                            SELECT
                            	goods_id,
                            	sum(num) AS sum_num,
                            	sum(cost_amount) AS total_amount
                            FROM
                            	goods_inout
                            WHERE
                                `type`='02' 
                            AND	dt BETWEEN @st AND @et
                            GROUP BY
                            	goods_id",
                            new MySqlParameter("@st", st),
                            new MySqlParameter("@et", et));

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string goods_id = dt.Rows[i]["goods_id"].ToString();

                    if (ID2goods_dict.ContainsKey(goods_id) && child2parent.ContainsKey(goods_id))                 // 关联商品 子商品切换回父商品
                    {
                        if (id2Stock.ContainsKey(child2parent[goods_id]))   // 已经存在父商品
                        {
                            id2Stock[child2parent[goods_id]].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            id2Stock[child2parent[goods_id]].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[child2parent[goods_id]]);
                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                            id2Stock.Add(child2parent[goods_id], item);
                        }
                        continue;
                    }

                    // 非关联商品 或 父商品
                    if (ID2goods_dict.ContainsKey(goods_id))
                    {
                        if (id2Stock.ContainsKey(goods_id))
                        {
                            id2Stock[goods_id].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            id2Stock[goods_id].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_amount"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[goods_id]);

                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_amount"]);

                            id2Stock.Add(goods_id, item);
                        }
                    }
                }
            }

            return id2Stock;
        }


        /// <summary>
        /// 进货统计
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private Dictionary<string, GoodsStock> QueryAllGoodsReplenish(string st, string et)
        {
            Dictionary<string, GoodsStock> id2Stock = new Dictionary<string, GoodsStock>();

            DataTable dt = ExecuteDataTable(@"
                            SELECT
                            	goods_id,
                            	sum(num) AS sum_num,
                            	sum(cost_amount) as total_money
                            FROM
                            	goods_inout
                            WHERE
                                (`type`='01' or `type`='10')
                            AND dt between @st and @et
                            GROUP BY
                            	goods_id",
                            new MySqlParameter("@st", st),
                            new MySqlParameter("@et", et));

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string goods_id = dt.Rows[i]["goods_id"].ToString();

                    if (ID2goods_dict.ContainsKey(goods_id) && child2parent.ContainsKey(goods_id))                 // 关联商品 子商品切换回父商品
                    {
                        if (id2Stock.ContainsKey(child2parent[goods_id]))   // 已经存在父商品
                        {
                            id2Stock[child2parent[goods_id]].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            id2Stock[child2parent[goods_id]].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_money"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[child2parent[goods_id]]);
                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]) / ID2goods_dict[goods_id].SplitNum;
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_money"]);
                            id2Stock.Add(child2parent[goods_id], item);
                        }
                        continue;
                    }

                    // 非关联商品 或 父商品
                    if (ID2goods_dict.ContainsKey(goods_id))
                    {
                        if (id2Stock.ContainsKey(goods_id))
                        {
                            id2Stock[goods_id].Num += Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            id2Stock[goods_id].CostAmount += Convert.ToDecimal(dt.Rows[i]["total_money"]);
                        }
                        else
                        {
                            GoodsStock item = new GoodsStock();
                            item.SetValue(ID2goods_dict[goods_id]);

                            item.Num = Convert.ToDecimal(dt.Rows[i]["sum_num"]);
                            item.CostAmount = Convert.ToDecimal(dt.Rows[i]["total_money"]);

                            id2Stock.Add(goods_id, item);
                        }
                    }
                }
            }

            return id2Stock;
        }

        /// <summary>
        /// 时间段内 销售商品情况
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private Dictionary<string, GoodsStock> QueryAllGoodsSellInfoByTime(string st, string et)
        {
            Dictionary<string, GoodsStock> id2Stock = new Dictionary<string, GoodsStock>();

            // 销售统计
            DataTable dt = ExecuteDataTable(@"
                            SELECT
                            	sales_goods.goods_id,
                            	sum(sales_goods.num) AS total_num,
                            	sum(sales_goods.total_amount) AS total_money,
                                sum(sales_goods.cost_amount) AS cost_money
                            FROM
                            	sales_goods
                            LEFT JOIN sales ON sales.order_id = sales_goods.order_id
                            WHERE
                            	sales.dt BETWEEN @st AND @et
                            GROUP BY
                            	goods_id",
                            new MySqlParameter("@st", st),
                            new MySqlParameter("@et", et));


            if (dt == null && dt.Rows.Count <= 0)
                return null;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string goods_id = dt.Rows[i]["goods_id"].ToString();

                if (ID2goods_dict.ContainsKey(goods_id) && child2parent.ContainsKey(goods_id))                 // 关联商品 子商品切换回父商品
                {
                    if (id2Stock.ContainsKey(child2parent[goods_id]))   // 已经存在父商品
                    {
                        id2Stock[child2parent[goods_id]].Num += Convert.ToDecimal(dt.Rows[i]["total_num"]) / ID2goods_dict[goods_id].SplitNum;
                        id2Stock[child2parent[goods_id]].ActulSaleAmount += Convert.ToDecimal(dt.Rows[i]["total_money"]);
                        id2Stock[child2parent[goods_id]].CostAmount += Convert.ToDecimal(dt.Rows[i]["cost_money"]);
                    }
                    else
                    {
                        GoodsStock item = new GoodsStock();
                        item.SetValue(ID2goods_dict[child2parent[goods_id]]);
                        item.Num = Convert.ToDecimal(dt.Rows[i]["total_num"]) / ID2goods_dict[goods_id].SplitNum;
                        item.ActulSaleAmount = Convert.ToDecimal(dt.Rows[i]["total_money"]);
                        item.CostAmount += Convert.ToDecimal(dt.Rows[i]["cost_money"]);

                        id2Stock.Add(child2parent[goods_id], item);
                    }
                    continue;
                }

                // 非关联商品 或 父商品
                if (ID2goods_dict.ContainsKey(goods_id))
                {
                    if (id2Stock.ContainsKey(goods_id))
                    {
                        id2Stock[goods_id].Num += Convert.ToDecimal(dt.Rows[i]["total_num"]);
                        id2Stock[goods_id].ActulSaleAmount += Convert.ToDecimal(dt.Rows[i]["total_money"]);
                        id2Stock[goods_id].CostAmount += Convert.ToDecimal(dt.Rows[i]["cost_money"]);
                    }
                    else
                    {
                        GoodsStock item = new GoodsStock();
                        item.SetValue(ID2goods_dict[goods_id]);

                        item.Num = Convert.ToDecimal(dt.Rows[i]["total_num"]);
                        item.ActulSaleAmount = Convert.ToDecimal(dt.Rows[i]["total_money"]);
                        item.CostAmount += Convert.ToDecimal(dt.Rows[i]["cost_money"]);

                        id2Stock.Add(goods_id, item);
                    }
                }
            }

            // 退货统计
            DataTable ret_dt = ExecuteDataTable(@"
                            SELECT
                            	goods_return.goods_id,
                            	sum(goods_return.num) AS total_num,
                            	sum(goods_return.return_value) AS total_money,
                                sum(goods_return.cost_amount) AS cost_money
                            FROM
                            	goods_return
                            LEFT JOIN sales_return ON sales_return.uuid = goods_return.return_id
                            WHERE
                            	sales_return.dt BETWEEN @st AND @et
                            GROUP BY
                            	goods_id",
                            new MySqlParameter("@st", st),
                            new MySqlParameter("@et", et));

            if (ret_dt != null && ret_dt.Rows.Count > 0)
            {
                for (int i = 0; i < ret_dt.Rows.Count; i++)
                {
                    string goods_id = ret_dt.Rows[i]["goods_id"].ToString();

                    if (child2parent.ContainsKey(goods_id) && id2Stock.ContainsKey(child2parent[goods_id])) // 子商品转换为父商品销售量
                    {
                        id2Stock[child2parent[goods_id]].Num -= Convert.ToDecimal(ret_dt.Rows[i]["total_num"]) / ID2goods_dict[goods_id].SplitNum;
                        id2Stock[child2parent[goods_id]].ActulSaleAmount -= Convert.ToDecimal(ret_dt.Rows[i]["total_money"]);
                        id2Stock[child2parent[goods_id]].CostAmount -= Convert.ToDecimal(ret_dt.Rows[i]["cost_money"]);

                        continue;
                    }

                    if (id2Stock.ContainsKey(goods_id))
                    {
                        id2Stock[goods_id].Num -= Convert.ToDecimal(ret_dt.Rows[i]["total_num"]);
                        id2Stock[goods_id].ActulSaleAmount -= Convert.ToDecimal(ret_dt.Rows[i]["total_money"]);
                        id2Stock[goods_id].CostAmount -= Convert.ToDecimal(ret_dt.Rows[i]["cost_money"]);
                    }

                    // todo 可能需要处理前一天销售 当天未退货情况
                }
            }

            return id2Stock;
        }

        private VIPConsumeGoods BuildVIPConsumeGoodsInfo(DataRow dr)
        {
            VIPConsumeGoods goods = new VIPConsumeGoods();

            goods.ID = (string)dr["order_id"];
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                goods.GoodsID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                goods.GoodsName = (string)dr["name"];
			if (!string.IsNullOrEmpty(dr["ean"].ToString()))
				goods.Ean = (string)dr["ean"];
			if (!string.IsNullOrEmpty(dr["num"].ToString()))
                goods.Num = Convert.ToInt32(dr["num"].ToString());
            if (!string.IsNullOrEmpty(dr["total_amount"].ToString()))
                goods.Amount = Convert.ToDecimal(dr["total_amount"].ToString());
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                goods.Opter = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                goods.Units = dr["units"].ToString();
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                goods.Date = ((DateTime)dr["dt"]).ToString(@"yyyy-MM-dd HH:mm:ss");

            // 新增字段
            if (!string.IsNullOrEmpty(dr["ean"].ToString()))
                goods.Ean = dr["ean"].ToString();
            if (!string.IsNullOrEmpty(dr["vip_name"].ToString()))
                goods.VIPName = dr["vip_name"].ToString();
            if (!string.IsNullOrEmpty(dr["selling_price"].ToString()))
                goods.Price = Convert.ToDecimal(dr["selling_price"].ToString());
            if (!string.IsNullOrEmpty(dr["price"].ToString()))  // 实际卖价   selling_vip_price
                goods.VIPPrice = Convert.ToDecimal(dr["price"].ToString());
            goods.SurrenderProfits = (goods.Price - goods.VIPPrice) * goods.Num;

            goods.PayType = "现金";
            if (!string.IsNullOrEmpty(dr["pay_mode"].ToString()))
            {
                int mode = Convert.ToInt32(dr["pay_mode"]);

                if (mode == 4)
                {
                    goods.PayType = "红码";
                }
                else if (mode == 5)
                {
                    goods.PayType = "银行卡";
                }
            }

            return goods;
        }
        
    }
}
