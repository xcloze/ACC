using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using JuYuan.ui;
using Model.model;

namespace JuYuan.dal
{
    /// <summary>
    /// 消费信息数据操作
    /// </summary>
    class ConsumeInfoDB : MySqlUtils
    {
        
        /// <summary>
        /// 根据会员id和 消费单号模糊查询会员的消费记录 （默认查询当前一个月的订单）
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="consumeID"></param>
        /// <returns></returns>
        public List<ConsumeOrderData> QueryConsumeInfo(string memberID, string consumeID, DateTime st, DateTime et)
        {
            List<ConsumeOrderData> order_list = new List<ConsumeOrderData>();

            DataSet ds = ExecuteDataSet(@"select order_id,member_id,dt,consume_value,score,pledge_amount,total_amount,operator_id,comment,from_value_card,pay_mode,pay_value from sales where member_id=@memberID 
                    and dt BETWEEN @dt and @et and order_id like @consumeID GROUP BY dt DESC",
                    new MySqlParameter("@memberID", memberID),
                    new MySqlParameter("@dt", st),
                    new MySqlParameter("@et", et),
                    new MySqlParameter("@consumeID", @"%" + consumeID + @"%")
                   );

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ConsumeOrderData data = BuildOrderData(dr);
                    if (null != data)
                    {
                        order_list.Add(data);
                    }
                }
            }

            return order_list;

        }

        public ConsumeInfo QueryOneById(string order_id)
        {
            ConsumeInfo consume_info = null;
            DataTable dt = ExecuteDataTable(@"select * from sales where order_id=@order_id",
                new MySqlParameter("@order_id", order_id));
            if (dt.Rows.Count > 0)
            {
                consume_info = new ConsumeInfo();
                consume_info.order_id = (string)dt.Rows[0]["order_id"];
                consume_info.dt = (DateTime)dt.Rows[0]["dt"];
                consume_info.discounted_value = Convert.ToDecimal(dt.Rows[0]["discounted_value"]);
                consume_info.score = (int)dt.Rows[0]["score"];
                consume_info.pledge_amount = (decimal)dt.Rows[0]["pledge_amount"];
                consume_info.total_amount = (decimal)dt.Rows[0]["total_amount"];
                consume_info.comment = (string)dt.Rows[0]["comment"];
                consume_info.optr_id = (int)dt.Rows[0]["operator_id"];
                consume_info.from_value_card = (decimal)dt.Rows[0]["from_value_card"];
             
                if (dt.Rows[0].Table.Columns.Contains("pay_mode"))
                {
                    consume_info.pay_mode = (int)dt.Rows[0]["pay_mode"]; 
                }
            }
            return consume_info;
        }

        /// <summary>
        /// 根据类别id、日期、商品名称查询消费明细记录
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <param name="sslb"></param>
        /// <param name="spmc"></param>
        /// <returns></returns>
        public List<DataReportMerchandiseData> QueryComsumeInfByLbGoodsInfo(string begin_date, string end_date, 
            string consume_type, string goods_name, string operator_name)
        {
            List<DataReportMerchandiseData> report_data_list = new List<DataReportMerchandiseData>();

            StringBuilder temp = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            temp.Append(@"select sales_goods.uuid,sales_goods.goods_id,sales_goods.price,sales_goods.discount,sales_goods.num,
                        sales_goods.total_amount, sales_goods.discounted_value, sales.dt, sales.operator_id, item.code, item.name,
                        item.units,item.category, goods_category.category_name, item.ean
                        from sales_goods left join sales on sales_goods.order_id = sales.order_id
                        left join goods item on sales_goods.goods_id = item.goods_id
                        left join goods_category on item.category = goods_category.id
                        where (sales_goods.goods_id like @goods_id or item.name like @goods_name)
                        and sales.dt between @begin_date and @end_date ");
            parameters.Add(new MySqlParameter("@goods_id", "%" + goods_name + "%"));
            parameters.Add(new MySqlParameter("@goods_name", "%" + goods_name + "%"));
            parameters.Add(new MySqlParameter("@begin_date", begin_date));
            parameters.Add(new MySqlParameter("@end_date", end_date));
            if (!("-1".Equals(consume_type) || string.IsNullOrEmpty(consume_type)))
            {
                temp.Append("  and item.category = @category");
                parameters.Add(new MySqlParameter("@category", consume_type));
            }
            if (operator_name != "全部" && !string.IsNullOrEmpty(operator_name))
            {
                temp.Append(" and sales.operator_id = @operator");
                UserDAL dal = new UserDAL();
                parameters.Add(new MySqlParameter("@operator", dal.QueryByUserName(operator_name).OptrID));
            }
            temp.Append(" order by sales.dt desc ");


            DataSet ds = ExecuteDataSet(temp.ToString(), parameters.ToArray());
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    DataReportMerchandiseData data = BuildConsumeData(dr);
                    report_data_list.Add(data);
                }
            }
            return report_data_list;
        }

        /// <summary>
        /// 统计消费信息
        /// </summary>
        /// <param name="begin_date"></param>
        /// <param name="end_date"></param>
        /// <param name="mem_type"></param>
        /// <param name="mem_id"></param>
        /// <param name="opt_name"></param>
        /// <returns></returns>

        public List<ConsumeOrderData> QueryConsumeInfByMemInfo(string begin_date, string end_date, string mem_type, string mem_id, string opt_name)
        {
            StringBuilder temp = new StringBuilder();
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            temp.Append(@"select member.mobile, member.`name`,
                        sales.member_id,  sales.order_id, sales.dt, sales.consume_value, 
                        sales.total_amount, sales.discounted_value, sales.pledge_amount, sales.score, 
                       sales.from_value_card, sales.pay_mode,
                      sales.operator_id, sales.comment
                        from sales left join member on sales.member_id = member.uuid
                        where sales.dt between @begin_date and @end_date");
            parameters.Add(new MySqlParameter("@begin_date", begin_date));
            parameters.Add(new MySqlParameter("@end_date", end_date));
            
            if (!"所有类型".Equals(mem_type))
            {
                temp.Append(" and member.card_name = @card_type");
                parameters.Add(new MySqlParameter("@card_type", mem_type));
            }
            if (opt_name != "所有操作员" && !string.IsNullOrEmpty(opt_name))
            {
                temp.Append(" and sales.operator_id = @operator");
                UserDAL userdal = new UserDAL();
                int optr_id = userdal.QueryByUserName(opt_name).OptrID;
                parameters.Add(new MySqlParameter("@operator", optr_id));
               
            }
            if ("" != mem_id)
            {
                temp.Append(" and (member.card_id like @mem_card_id or member.name like @mem_name or member.mobile like @mobile) ");
                parameters.Add(new MySqlParameter("@mem_card_id", "%" + mem_id + "%"));
                parameters.Add(new MySqlParameter("@mem_name", "%" + mem_id + "%"));
                parameters.Add(new MySqlParameter("@mobile", "%" + mem_id + "%"));
            }
            temp.Append(" order by sales.dt desc ");

            List<ConsumeOrderData> order_list = new List<ConsumeOrderData>();
            DataSet ds = ExecuteDataSet(temp.ToString(), parameters.ToArray());

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    ConsumeOrderData data = BuildOrderData2(dr);
                    if (null != data)
                    {
                        order_list.Add(data);
                    }
                }
            }
            return order_list;
        }
        
        public int CountMemberConsumeTimes(string memberID)
        {
            if (string.IsNullOrEmpty(memberID))
                return 0;
            return Convert.ToInt32(ExecuteScalar(@"select count(order_id) from sales where member_id=@memberID", new MySqlParameter("@memberID", memberID)));
        }
        private ConsumeOrderData BuildOrderData(DataRow dr)
        {
            ConsumeOrderData data = new ConsumeOrderData();

            data.OrderID = (string)dr["order_id"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                data.ConsumeDate = ((DateTime)dr["dt"]).ToString(@"yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(dr["total_amount"].ToString()))
                data.TotalMoney = (decimal)dr["total_amount"];
            if (!string.IsNullOrEmpty(dr["member_id"].ToString()))
                data.MemberID = (string)dr["member_id"];
            if (!string.IsNullOrEmpty(dr["score"].ToString()))
                data.Integral = (int)dr["score"];
            if (!string.IsNullOrEmpty(dr["pledge_amount"].ToString()))
                data.Deposit = (decimal)dr["pledge_amount"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL userdal = new UserDAL();
                int optr_id = (int)dr["operator_id"];
                data.Operator = userdal.QueryUserById(optr_id).name;
            }
                
            if (!string.IsNullOrEmpty(dr["comment"].ToString()))
                data.Remark = (string)dr["comment"];
                 int pay_value = 0;
                 if (!string.IsNullOrEmpty(dr["pay_mode"].ToString()))
                     pay_value = Convert.ToInt32(dr["pay_mode"].ToString());
                 
            data.PayMode = ui.PayMode.getStringbyMode(pay_value);

            return data;
        }

        private ConsumeOrderData BuildOrderData2(DataRow dr)
        {
            ConsumeOrderData data = new ConsumeOrderData();

            data.OrderID = (string)dr["order_id"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                data.ConsumeDate = ((DateTime)dr["dt"]).ToString(@"yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(dr["total_amount"].ToString()))
                data.TotalMoney = (decimal)dr["total_amount"];
            if (!string.IsNullOrEmpty(dr["member_id"].ToString()))
                data.MemberID = (string)dr["member_id"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MemberName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["mobile"].ToString()))
                data.Mobile = (string)dr["mobile"];
            if (!string.IsNullOrEmpty(dr["score"].ToString()))
                data.Integral = (int)dr["score"];
            if (!string.IsNullOrEmpty(dr["pledge_amount"].ToString()))
                data.Deposit = (decimal)dr["pledge_amount"];
            if (!string.IsNullOrEmpty(dr["from_value_card"].ToString()))
                data.CardPay = (decimal)dr["from_value_card"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                data.Operator = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }
            if (!string.IsNullOrEmpty(dr["comment"].ToString()))
                data.Remark = (string)dr["comment"];
            int pay_mode = 0;
            if(!string.IsNullOrEmpty(dr["pay_mode"].ToString()))
            {
                pay_mode = Convert.ToInt32(dr["pay_mode"].ToString());
            }
       

            data.PayMode = ui.PayMode.getStringbyMode(pay_mode);
     

            return data;
        }

        private DataReportMerchandiseData BuildConsumeData(DataRow dr)
        {
            DataReportMerchandiseData data = new DataReportMerchandiseData();

            data.ID = (string)dr["uuid"];
            if (!string.IsNullOrEmpty(dr["code"].ToString()))
                data.MerchID = (string)dr["code"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.MerchName = (string)dr["name"];
            if (!string.IsNullOrEmpty(dr["ean"].ToString()))
                data.Ean = (string)dr["ean"];
            if (!string.IsNullOrEmpty(dr["units"].ToString()))
                data.Uint = (string)dr["units"];
            if (!string.IsNullOrEmpty(dr["price"].ToString()))
                data.UintPrice = (decimal)dr["price"];
            if (!string.IsNullOrEmpty(dr["category_name"].ToString())) // todo 类别名称
                data.Category = (string)dr["category_name"];
            if (!string.IsNullOrEmpty(dr["dt"].ToString()))
                data.ConsumeDate = (DateTime)dr["dt"];
            if (!string.IsNullOrEmpty(dr["num"].ToString()))
                data.Amount = (float)dr["num"];
            if (!string.IsNullOrEmpty(dr["total_amount"].ToString()))
                data.TotalConsume = (decimal)dr["total_amount"];
            if (!string.IsNullOrEmpty(dr["operator_id"].ToString()))
            {
                UserDAL dal = new UserDAL();
                data.Operator = dal.QueryByOptrID((int)dr["operator_id"]).name;
            }

            return data;
        }
    }
    
}
