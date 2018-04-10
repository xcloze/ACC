using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
   
    public class ExchangeGoodsRecord
    {
       
        public string UUID { get; set; }
       
        public string CardID { get; set; }

        public string MemberID { get; set; }

       
        public string GoodsID { get; set; }
       
        public DateTime Date { get; set; }
      
        public int Num { get; set; }
       
        public int ExchangeScore { get; set; }
      
        public int RemainScore { get; set; }
      
        public string Opter { get; set; }
    }
}
