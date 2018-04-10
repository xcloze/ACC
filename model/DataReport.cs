using JuYuan.utils;

namespace JuYuan.model
{
    class DataReportConsumeGoods : IExportRow
    {
        private string m_id;
        private string m_date;
        private string m_goods_id;
        private string m_goods_name;
        private int m_num;
        private decimal m_amount;
        private string m_pay_type;
        private string m_optr;
        private string m_units;
        public string ID
        {
            get { return m_id; }
            set { m_id = value; }
        }
        public string GoodsID
        {
            get { return m_goods_id; }
            set { m_goods_id = value; }
        }
        public string GoodsName
        {
            get { return m_goods_name; }
            set { m_goods_name = value; }
        }
		public string Ean { get; set; }
		public int Num
        {
            get { return m_num; }
            set { m_num = value; }
        }
        public decimal Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }
        public string PayType
        {
            get { return m_pay_type; }
            set { m_pay_type = value; }
        }
        public string Opter
        {
            get { return m_optr; }
            set { m_optr = value; }
        }

        public string Units
        {
            get { return m_units; }
            set { m_units = value; }
        }

        public string Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        public enum ColumnIndex
        {
            COLUMN_DATE = 0,
            COLUMN_ORDER_ID,
            COLUMN_GOODS_CODE,
            COLUMN_GOODS_NAME,
			COLUNM_GOODS_EAN,
			COLUMN_NUM,
            COLUMN_TOTAL_MONEY,
            COLUMN_PAY_MODE,

            // ========================
            COLUMN_END,
        }

        public int ColumnCount()
        {
            return (int)ColumnIndex.COLUMN_END;
        }

        public string GetColumn(int index, ExportColumnType type)
        {
            bool isHeader = type == ExportColumnType.ColumnHeader;
            string text = "";
            switch (index)
            {
                case (int)ColumnIndex.COLUMN_DATE: text = isHeader ? "日期" : Date; break;
                case (int)ColumnIndex.COLUMN_ORDER_ID: text = isHeader ? @"订单\退货号" : ID; break;
                case (int)ColumnIndex.COLUMN_GOODS_CODE: text = isHeader ? "商品编号" : GoodsID; break;
                case (int)ColumnIndex.COLUMN_GOODS_NAME: text = isHeader ? "商品名称" : GoodsName; break;
				case (int)ColumnIndex.COLUNM_GOODS_EAN: text = isHeader ? "条形码" : Ean; break;
				case (int)ColumnIndex.COLUMN_NUM: text = isHeader ? "数量" : Num.ToString(); break;
                case (int)ColumnIndex.COLUMN_TOTAL_MONEY: text = isHeader ? "金额(元)" : Amount.ToString(); break;
                case (int)ColumnIndex.COLUMN_PAY_MODE: text = isHeader ? "支付方式" : PayType; break;
            }
            return text;
        }
    }

    class DataReportPayType : IExportRow
    {
        private string m_payType;
        private decimal m_mount;
        private int m_count;
        private int m_ret_count;
        private decimal m_pay_value;
        private decimal m_ret_value;
        private string m_optr;

        public string PayType
        {
            get { return m_payType; }
            set { m_payType = value; }
        }
        public string Opter
        {
            get { return m_optr; }
            set { m_optr = value; }
        }
        public decimal Amount
        {
            get { return m_mount; }
            set { m_mount = value; }
        }
        public int Num
        {
            get { return m_count; }
            set { m_count = value; }
        }

        public int ReturnNum
        {
            get { return m_ret_count; }
            set { m_ret_count = value; }
        }

        public decimal PayValue
        {
            get { return m_pay_value; }
            set { m_pay_value = value; }
        }

        public decimal ReturnValue
        {
            get { return m_ret_value; }
            set { m_ret_value = value; }
        }

        public enum ColumnIndex
        {
            COLUMN_PAY_MODE = 0,
            COLUMN_SALE_TIMES,
            COLUMN_SALE_MONEY,
            COLUMN_RETURN_TIMES,
            COLUMN_RETURN_MONEY,
            COLUMN_NET_INCOME,

            // ========================
            COLUMN_END,
        }

        public int ColumnCount()
        {
            return (int)ColumnIndex.COLUMN_END;
        }

        public string GetColumn(int index, ExportColumnType type)
        {
            bool isHeader = type == ExportColumnType.ColumnHeader;
            string text = "";
            switch (index)
            {
                case (int)ColumnIndex.COLUMN_PAY_MODE: text = isHeader ? "支付方式" : PayType; break;
                case (int)ColumnIndex.COLUMN_SALE_TIMES: text = isHeader ? "支付次数" : Num.ToString(); break;
                case (int)ColumnIndex.COLUMN_SALE_MONEY: text = isHeader ? "支付金额" : PayValue.ToString(); break;
                case (int)ColumnIndex.COLUMN_RETURN_TIMES: text = isHeader ? "退货次数" : ReturnNum.ToString(); break;
                case (int)ColumnIndex.COLUMN_RETURN_MONEY: text = isHeader ? "退货金额" : ReturnValue.ToString(); break;
                case (int)ColumnIndex.COLUMN_NET_INCOME: text = isHeader ? "合计金额" : Amount.ToString(); break;
            }
            return text;
        }
    }
    class VIPConsumeGoods : DataReportConsumeGoods
    {
       
        public string VIPName
        {
            get;
            set;
        }

        
        public string Ean
        {
            get;
            set;
        }

       
        public decimal Price
        {
            get;
            set;
        }

       
        public decimal VIPPrice
        {
            get;
            set;
        }

        
        public decimal SurrenderProfits
        {
            get;
            set;
        }
    }

    public class StockSellInventory
    {
       
        public string GoodsID
        {
            get;
            set;
        }

        public string GoodsName
        {
            get;
            set;
        }

        public string Ean
        {
            get;
            set;
        }

        public string GoodsCode
        {
            get;
            set;
        }

        public string Units
        {
            get;
            set;
        }

        public string CategoryName { get; set; }

      
        public decimal BeginNum
        {
            get;
            set;
        }

        public decimal BeginAmount
        {
            get;
            set;
        }

        public decimal BeginExpectAmount
        {
            get;
            set;
        }

        
        public decimal InNum
        {
            get;
            set;
        }

        public decimal InAmount
        {
            get;
            set;
        }

        public decimal NotTaxInAmount
        {
            get;
            set;
        }
        public decimal InTaxAmount
        {
            get;
            set;
        }

        public decimal InExpectAmount
        {
            get;
            set;
        }

   
        public decimal SellNum
        {
            get;
            set;
        }

        public decimal SellPrice
        {
            get;
            set;
        }
        public decimal SellAmount
        {
            get;
            set;
        }
        public decimal SellNotTax
        {
            get;
            set;
        }

        public decimal SellTax
        {
            get;
            set;
        }
        public decimal SellCost
        {
            get;
            set;
        }
        public decimal SellGrossMargin
        {
            get;
            set;
        }
        public decimal RateOfMargin
        {
            get;
            set;
        }

     
        public decimal CallInNum
        {
            get;
            set;
        }

        public decimal CallInAmount
        {
            get;
            set;
        }

      
        public decimal CallOutNum
        {
            get;
            set;
        }

        public decimal CallOutAmount
        {
            get;
            set;
        }

        
        public decimal ProfitLossNum
        {
            get;
            set;
        }

        public decimal ProfitLossAmount
        {
            get;
            set;
        }

        public decimal ReceptionNum
        {
            get;
            set;
        }

        public decimal ReceptionAmount
        {
            get;
            set;
        }

        public decimal EndPeriodNum
        {
            get;
            set;
        }

        public decimal EndPeriodAmount
        {
            get;
            set;
        }

        public decimal EndExpectAmount
        {
            get;
            set;
        }
    }
}

