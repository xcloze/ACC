using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    
    class ChangeCardRecord
    {
        public string member_id { get; set; }
       
        public string new_cardID { get; set; }
        
        public string old_cardID { get; set; }
        
        public int state { get; set; }
        
        public string passwd { get; set; }
       
        public DateTime date { get; set; }

    }
}
