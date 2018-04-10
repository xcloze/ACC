using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
 
    public class MemberCard
    {
        public string MemberID { get; set; }
        
        public string CardID { get; set; }
        
      
        public string CardState { get; set; }
        
        public DateTime JoinDate { get; set; }
        
        public string Password { get; set; }
        
   
        public decimal CardRemain { get; set; }
        
        public string Comment { get; set; }
       
      
        public int ClosingState { get; set; }
    
        public DateTime? ClosingDate { get; set; }
        
    }
}
