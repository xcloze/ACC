using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mysqlApp.utils;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;

namespace JuYuan.dal
{
    class TJCodeDAL : MySqlUtils
    {
        public int InsertTraceCode(TJCode new_tarce)
        {
            return ExecuteNonQuery(@"insert into trace_code(goods_id,qr_code,state,code_count,per_batch,operator_id) values(@goods_id,@qr_code,@state,@code_count,@per_batch,@oper)",
                new MySqlParameter("@goods_id", new_tarce.GoodsID), 
                new MySqlParameter("@qr_code", new_tarce.TraceCode),
                new MySqlParameter("@state", new_tarce.Status),
                new MySqlParameter("@code_count", new_tarce.CodeCount),
                new MySqlParameter("@per_batch", new_tarce.ypym),
                new MySqlParameter("@oper", new_tarce.oper_id)
            );
        }

        public TJCode FindTraceInfoByCode(string qr_code)
        {
            TJCode tjcode = null;
            DataTable dt = ExecuteDataTable(@"select code,trace_code.* from trace_code,goods where goods.goods_id=trace_code.goods_id and qr_code=@qr_code", 
                new MySqlParameter("@qr_code", qr_code));
            if (dt.Rows.Count > 0)
            {
                tjcode = new TJCode();
                tjcode.Code = (string)dt.Rows[0]["code"];
                tjcode.GoodsID = (string)dt.Rows[0]["goods_id"];
                tjcode.TraceCode = (string)dt.Rows[0]["qr_code"];
                tjcode.Status = (int)dt.Rows[0]["state"];
                tjcode.CodeCount = (int)dt.Rows[0]["code_count"];
                tjcode.ypym = (int)dt.Rows[0]["per_batch"];
                tjcode.dnxf = (int)dt.Rows[0]["deal_in_store"];
            }
            return tjcode;
        }
        
     
        public int UpdateTraceCodeInfo(TJCode trace_info)
        {
            return ExecuteNonQuery(@"update trace_code set state = @status,goods_id = @goods_id,status = @status,code_count = @code_count,per_batch = @per_batch,deal_in_store = @deal_in_store where qr_code = @qr_code",
                  new MySqlParameter("@qr_code", trace_info.TraceCode),
                  new MySqlParameter("@goods_id", trace_info.GoodsID),
                  new MySqlParameter("@status", trace_info.Status),
                  new MySqlParameter("@code_count", trace_info.CodeCount),
                  new MySqlParameter("@per_batch", trace_info.ypym),
                  new MySqlParameter("@deal_in_store", trace_info.dnxf));
        }

        public int deleteByTjm(string qr_code)
        {
            return ExecuteNonQuery(@"delete from trace_code where qr_code=@qr_code",
                  new MySqlParameter("@qr_code", qr_code));
        }
    }
}
