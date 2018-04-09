using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JuYuan.model
{
    /// <summary>
    /// 制卡记录
    /// </summary>
    class ChangeCardRecord
    {
        public string member_id { get; set; }
        /// <summary>
        /// 新卡号
        /// </summary>
        public string new_cardID { get; set; }
        /// <summary>
        /// 旧卡号
        /// </summary>
        public string old_cardID { get; set; }
        /// <summary>
        /// 状态 1正常
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passwd { get; set; }
        /// <summary>
        /// 换卡时间
        /// </summary>
        public DateTime date { get; set; }

    }
}
