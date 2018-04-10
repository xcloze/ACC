using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuYuan.model
{
    class GoodsBase
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

        public string Code
        {
            get;
            set;
        }

        public string CategoryID
        {
            get;
            set;
        }

        public string CategoryName
        {
            get;
            set;
        }

        public string Units
        {
            get;
            set;
        }

        public decimal SellPrice
        {
            get;
            set;
        }

        public decimal PurchasingPrice
        {
            get;
            set;
        }

        public decimal VIPPrice
        {
            get;
            set;
        }

        public int SellState
        {
            get;
            set;
        }

        public string ParentID
        {
            get;
            set;
        }

        public decimal SplitNum
        {
            get;
            set;
        }
    }

    class GoodsStock:GoodsBase
    {

        public void SetValue(GoodsBase goods)
        {
            this.GoodsID = goods.GoodsID;
            this.GoodsName = goods.GoodsName;
            this.Ean = goods.Ean;
            this.Code = goods.Code;
            this.CategoryID = goods.CategoryID;
            this.CategoryName = goods.CategoryName;
            this.ParentID = goods.ParentID;
            this.Units = goods.Units;

            this.SellPrice = goods.SellPrice;
            this.PurchasingPrice = goods.PurchasingPrice;
            this.VIPPrice = goods.VIPPrice;
            this.SplitNum = goods.SplitNum;
            this.SellState = goods.SellState;
        }

        public decimal Num
        {
            get;
            set;
        }

      
        public decimal CostAmount
        {
            get;
            set;
        }

       
        public decimal ExpectSellAmount
        {
            get
            {
                return Num * SellPrice;
            }
        }

        public decimal ActulSaleAmount
        {
            get;
            set;
        }
    }

}
