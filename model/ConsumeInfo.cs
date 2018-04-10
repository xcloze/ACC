using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
  
    public class ConsumeInfo
    {
        public string order_id { get; set; }  

        public string member_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal consume_value { get; set; }   
       
        public DateTime dt { get; set; }  
        
        public decimal discounted_value { get; set; }  


        public int score { get; set; }   

        public decimal pledge_amount { get; set; }    
        public decimal total_amount { get; set; }     
      
        public string comment { get; set; } 
    
        public decimal from_value_card { get; set; }    

        public int pay_mode { get; set; } 

        public decimal pay_value { get; set; } 
       
    }
}
