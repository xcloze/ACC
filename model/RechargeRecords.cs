using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
   
    public class RechargeRecords
    {
      
        public string UUID { get; set; }
      
        public string CardID { get; set; }

        public string MemberID { get; set; }

        public string ActiveNO { get; set; }
       
        public decimal PayMoney { get; set; }
      
        public DateTime Date { get; set; }
        
        public decimal DiscountMoney { get; set; }
    
        public int OpetrID { get; set; }
     
        public string Comment { get; set; }
      
        public decimal ActualPayMoney { get; set; }
      
        public decimal Cash { get; set; }
        
        public decimal ValueCard { get; set; }
       
        public decimal BankCard { get; set; }
        
        public decimal Voucher { get; set; }
    }
}
