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
    /// 会员信息数据操作
    /// </summary>
    class MemberDAL:MySqlUtils
    {
        /// <summary>
        /// 根据会员卡号|姓名|简名|电话号码查询会员信息
        /// </summary>
        /// <param name="hykh">会员卡号</param>
        /// <returns></returns>
        public List<MemberInfo> QueryBySearch(String content)
        {
            List<MemberInfo> meminfo_list = new List<MemberInfo>();

            DataSet ds;
            if (string.IsNullOrEmpty(content))
            {
                ds = ExecuteDataSet(@"select member.*,value_card.state ,value_card.remain_value from member left 
                        JOIN value_card on value_card.card_id = member.card_id order by member.mobile");
            }
            else
            {
                content = content.ToUpper().Trim();

                ds = ExecuteDataSet(@"select member.*,value_card.state  ,value_card.remain_value from member left 
                        JOIN value_card on value_card.card_id = member.card_id where member.card_id like @card_id or member.name like @name 
                        or member.mobile like @mobile or member.vip_card_id like @vip_card_id order by member.mobile",
                        new MySqlParameter("@card_id", "%" + content + "%"), 
                        new MySqlParameter("@name", "%" + content + "%"),
                        new MySqlParameter("@mobile", "%" + content + "%"),
                        new MySqlParameter("@vip_card_id", "%" + content + "%")
                    );
            }
            
            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    MemberInfo member = BuildMemberInfoData(dr);
                    if (null != member)
                    {
                        meminfo_list.Add(member);
                    }
                }
            }

            return meminfo_list;

        }
       
        /// <summary>
        /// 根据会员卡号|姓名|简名|电话号码 以及 会员加入时间 查询会员信息 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public List<MemberData> QueryBySearchAandTime(string content, bool isHasCard, string st, string et)
        {

            List<MemberData> mem_list = new List<MemberData>();
            DataSet ds;

            if (!isHasCard)
            {
                if (string.IsNullOrEmpty(content))
                {
                    ds = ExecuteDataSet(@"select  member.* from member where (member.card_id<>'0000' or card_id is NULL) and 
                        member.join_dt BETWEEN @st and @et order by member.mobile, member.name desc",
                        new MySqlParameter("@st", st),
                        new MySqlParameter("@et", et)
                       );

                }
                else
                {
                    content = content.ToUpper().Trim();
                    ds = ExecuteDataSet(@"select member.* from member where (member.card_id like @Hykh or member.name like @Hyxm 
                        or member.mobile like @Yddh or member.vip_card_id like @vip) and (member.card_id<>'0000' or card_id is NULL) and 
                        member.join_dt BETWEEN @st and @et order by member.mobile, member.name desc",
                        new MySqlParameter("@Hykh", "%" + content + "%"),
                        new MySqlParameter("@Hyxm", "%" + content + "%"),
                        new MySqlParameter("@Yddh", "%" + content + "%"),
                        new MySqlParameter("@vip", "%" + content + "%"),
                        new MySqlParameter("@st", st),
                        new MySqlParameter("@et", et)
                       );
                }
                
            }
            else
            {
                if (string.IsNullOrEmpty(content))
                {
                    ds = ExecuteDataSet(@"select member.*,value_card.is_closing,value_card.state,value_card.closing_dt from member left 
                        JOIN value_card on value_card.card_id = member.card_id where member.card_id<>'0000' and member.card_id<>'' and 
                        member.join_dt BETWEEN @st and @et order by member.mobile desc, member.name desc",
                        new MySqlParameter("@st", st),
                        new MySqlParameter("@et", et)
                       );

                }
                else
                {
                    content = content.ToUpper().Trim();
                    ds = ExecuteDataSet(@"select member.*,value_card.is_closing,value_card.state,value_card.closing_dt from member left 
                    JOIN value_card on value_card.card_id = member.card_id where (member.card_id like @Hykh or member.name like @Hyxm 
                    or member.mobile like @Yddh or memeber.vip_card_id like @vip) and member.card_id<>'0000' and member.card_id<>'' and 
                    member.join_dt BETWEEN @st and @et order by member.mobile desc, member.name desc",
                        new MySqlParameter("@Hykh", "%" + content + "%"),
                        new MySqlParameter("@Hyxm", "%" + content + "%"),
                        new MySqlParameter("@Yddh", "%" + content + "%"),
                        new MySqlParameter("@vip", "%" + content + "%"),
                        new MySqlParameter("@st", st),
                        new MySqlParameter("@et", et)
                       );
                }
               
            }

            if (null != ds && ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    MemberData data = BuildMemberData(dr);
                    if (null != data)
                    {
                        mem_list.Add(data);
                    }
                }
            }

            return mem_list;
        }

        private MemberData BuildMemberData(DataRow dr)
        {
            MemberData data = new MemberData();
            data.ID = (string)dr["uuid"];
            if (!string.IsNullOrEmpty(dr["person_code"].ToString()))
                data.Code = (string)dr["person_code"];
            if (!string.IsNullOrEmpty(dr["passwd"].ToString()))
                data.Password = (string)dr["passwd"];
            if (!string.IsNullOrEmpty(dr["name"].ToString()))
                data.Name = (string)dr["name"];

            data.Sex = "未知";
            if (!string.IsNullOrEmpty(dr["sex"].ToString()))
            {
                if((int)dr["sex"] == (int)SEX.Male)
                    data.Sex = "男";
                else if((int)dr["sex"] == (int)SEX.Female)
                    data.Sex = "女";
            }

            if (!string.IsNullOrEmpty(dr["mobile"].ToString()))
                data.Phone = (string)dr["mobile"];
            if (!string.IsNullOrEmpty(dr["birthday"].ToString()))
                data.Birthday = (DateTime)dr["birthday"];
            if (!string.IsNullOrEmpty(dr["join_dt"].ToString()))
                data.JoinDate = (DateTime)dr["join_dt"];
            if (!string.IsNullOrEmpty(dr["identification_type"].ToString()))
                data.CredentialsType = (string)dr["identification_type"];
            if (!string.IsNullOrEmpty(dr["identification_id"].ToString()))
                data.CredentialsID = (string)dr["identification_id"];
            if (!string.IsNullOrEmpty(dr["address"].ToString()))
                data.Address = (string)dr["address"];
            if (!string.IsNullOrEmpty(dr["comment"].ToString()))
                data.Remark = (string)dr["comment"];
            if (!string.IsNullOrEmpty(dr["score"].ToString()))
                data.Integral = (int)dr["score"];
         
            if (!string.IsNullOrEmpty(dr["vip_card_id"].ToString()))
                data.VipCardId = (string)dr["vip_card_id"];

            if (!string.IsNullOrEmpty(dr["card_id"].ToString()) && dr["card_id"].ToString() != "--")
            {
                data.CardID = dr["card_id"].ToString();
                data.OpenEffective = false;
                data.Status = "0";
                
            }
            else // 无卡用户
            {
                data.CardID = "";
                data.OpenEffective = false;
                data.Status = "0";
            }

            return data;
        }

        
        /// <summary>
        /// 构建会员信息
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Member BuildMemberInfo(DataRow row)
        {
            Member mem_info = new Member();
            mem_info.uuid = (string)row["uuid"];
            mem_info.birthday = (DateTime?)FromDBValue(row["birthday"]);
            if (!string.IsNullOrEmpty(row["person_code"].ToString()))
                mem_info.person_code = (string)row["person_code"];
            if (!string.IsNullOrEmpty(row["card_id"].ToString()))
                mem_info.card_id = (string)row["card_id"];
            else
                mem_info.card_id = "";

            mem_info.name = (string)row["name"];
        
            mem_info.passwd = (string)row["passwd"];
            mem_info.sex.sex_type = (SEX)row["sex"];

         
            mem_info.mobile = (string)row["mobile"];

            if (!string.IsNullOrEmpty(row["phone"].ToString()))
                mem_info.phone = (string)row["phone"];
            else
                mem_info.phone = "";
         
            if (!string.IsNullOrEmpty(row["identification_type"].ToString()))
                mem_info.identification_type = (string)row["identification_type"];
            if (!string.IsNullOrEmpty(row["identification_id"].ToString()))
                mem_info.identification_id = (string)row["identification_id"];
            if (!string.IsNullOrEmpty(row["address"].ToString()))
                mem_info.address = (string)row["address"];
          
            if (!string.IsNullOrEmpty(row["join_dt"].ToString()))
                mem_info.join_dt = (DateTime)row["join_dt"];
        
            if (!string.IsNullOrEmpty(row["comment"].ToString()))
                mem_info.comment = (string)row["comment"];
            if (!string.IsNullOrEmpty(row["score"].ToString()))
                mem_info.score = (int)row["score"];
       
            if (!string.IsNullOrEmpty(row["vip_card_id"].ToString()))
                mem_info.vip_card_id = row["vip_card_id"].ToString();
            return mem_info;
        }

        /// <summary>
        /// 根据会员卡号查询会员信息数据
        /// </summary>
        /// <param name="card_id">会员卡号</param>
        /// <returns></returns>
        public Member QueryByCardID(string card_id)
        {
            DataTable table = ExecuteDataTable(@"select * from member where card_id =@card_id or uuid=@id",
                new MySqlParameter("@card_id", card_id),
                new MySqlParameter("@id", card_id)
            );
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                return BuildMemberInfo(row);
            }
            return null;
        }

       
        public int ExchangeCard(string id, string newCard)
        {
            return ExecuteNonQuery(@"update member set card_id=@newCard where uuid=@id",
                new MySqlParameter("@newCard", newCard),
                new MySqlParameter("@id", id)
               );
        }
        
        /// <summary>
        /// 更新积分
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public int UpdateMemberScore(string memberId, int score)
        {
            return ExecuteNonQuery(@"update member set score=score+@integral where uuid=@id",
                new MySqlParameter("@integral", score),
                new MySqlParameter("@id", memberId)
               );
        }

        private MemberInfo BuildMemberInfoData(DataRow row)
        {
            if (null == row)
                return null;

            MemberInfo member = new MemberInfo();

            member.ID = (string)row["uuid"];
            if (!string.IsNullOrEmpty(row["card_id"].ToString()))
                member.CardID = (string)row["card_id"];
           
            if (!string.IsNullOrEmpty(row["person_code"].ToString()))
                member.MemberCode = (string)row["person_code"];
            if (!string.IsNullOrEmpty(row["name"].ToString()))
                member.MemberName = (string)row["name"];
            if (!string.IsNullOrEmpty(row["score"].ToString()))
                member.Integral = (int)row["score"];
            if (!string.IsNullOrEmpty(row["mobile"].ToString()))
                member.Phone = (string)row["mobile"];
            if (!string.IsNullOrEmpty(row["vip_card_id"].ToString()))
                member.VipCardID = (string)row["vip_card_id"];

            if(row.Table.Columns.Contains("remain_value") && !string.IsNullOrEmpty(row["remain_value"].ToString()))
            {
                member.remain_value = Convert.ToDecimal(row["remain_value"].ToString());
            }

            if (row.Table.Columns.Contains("state") && !string.IsNullOrEmpty(row["remain_value"].ToString()))
            {
                string state = row["state"].ToString();
                if (state == "可用")
                {
                    member.isHaveValueCard = true;
                }
            }
            else
                member.isHaveValueCard = false;

            if (member.remain_value <= 0)
            {
                member.isHaveValueCard = false;
            }


            return member;
        }

    }
}
