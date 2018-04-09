using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using MySql.Data.MySqlClient;
using System.Data;
using mysqlApp.utils;
using Model.model;
/*
* 用户数据库操作累
*/
namespace JuYuan.dal
{
    class UserDAL : MySqlUtils
    {
        /// <summary>
        /// 查询原密码是否正确
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int QueryPasswordByID(Optr u)
        {
             return Convert.ToInt32(
                 ExecuteScalar(@"select count(id) from operator where passwd=@pwd and id = @id", new MySqlParameter("@pwd", u.password), new MySqlParameter("@id", u.OptrID)));

        }


        /// <summary>
        /// 查询用户名密码是否正确
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int ValidateLogin(Optr u)
        {
            return Convert.ToInt32(
                 ExecuteScalar(@"select count(id) from operator where user_name=@name and passwd = @pwd", new MySqlParameter("@name", u.username), new MySqlParameter("@pwd", u.password)));
        }

        public void ValidateUser(Optr u, out bool name, out bool pwd)
        {
            name = pwd = false;

            if (Convert.ToInt32(ExecuteScalar(@"select count(id) from operator where user_name=@name", new MySqlParameter("@name", u.username))) > 0)
            {
                name = true;
            }
            if (Convert.ToInt32(ExecuteScalar(@"select count(id) from operator where user_name=@name and passwd=@pwd", new MySqlParameter("@name", u.username), new MySqlParameter("@pwd", u.password))) > 0)
            {
                pwd = true;
                return;
            }
        }


        /// <summary>
        /// 查询全部类型
        /// </summary>
        /// <returns></returns>
        public List<Optr> SelectAll()
        {
            List<Optr> list = new List<Optr>();

            DataTable table = ExecuteDataTable(@"select * from operator where state=1");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Optr rt = new Optr();
                DataRow row = table.Rows[i];

                rt = BuildUserData(row);

                if (string.IsNullOrEmpty(rt.name))
                    continue;

                list.Add(rt);
            }
            return list;
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public Optr QueryByUserName(string name)
        {
            Optr s = null;
            DataTable table = ExecuteDataTable(@"select * from operator where user_name = @name", new MySqlParameter("@name", name));
            for (int i = 0; i < table.Rows.Count; i++)
            {
                try
                {
                    s = new Optr();
                    DataRow row = table.Rows[i];
                    s = BuildUserData(row);
                }
               catch(Exception ex) 
                {
               
                   JuYuan.utils.ErrorCatchUtil.showErrorMsg(ex);
               }
             }
            return s;
        }

        public Optr QueryByOptrID(int id)
        {
            Optr s = null;
            DataTable table = ExecuteDataTable(@"select * from operator where id = @id", new MySqlParameter("@id", id));
            for (int i = 0; i < table.Rows.Count; i++)
            {
                try
                {
                    s = new Optr();
                    DataRow row = table.Rows[i];
                    s = BuildUserData(row);
                }
                catch (Exception ex)
                {
                    JuYuan.utils.ErrorCatchUtil.showErrorMsg(ex);
                }
            }
            return s;
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        public Optr QueryUserById(int id)
        {
            Optr s = null;
            DataTable table = ExecuteDataTable(@"select * from operator where id = @Id", new MySqlParameter("@Id", id));
            for (int i = 0; i < table.Rows.Count; i++)
            {
                try
                {
                    s = new Optr();
                    DataRow row = table.Rows[i];

                    s = BuildUserData(row);
                }
                catch (Exception ex)
                {
                    JuYuan.utils.ErrorCatchUtil.showErrorMsg(ex);
                }
            }
            return s;
        }

        private Optr BuildUserData(DataRow row)
        {
            Optr usr = new Optr();
            usr.OptrID = Convert.ToInt32(row["id"]);
            
            if (!string.IsNullOrEmpty(row["user_name"].ToString()))
                usr.username = (string)row["user_name"];

            if (!string.IsNullOrEmpty(row["name"].ToString()))
                usr.name = (string)row["name"];

            if (!string.IsNullOrEmpty(row["state"].ToString()))
                usr.status = Convert.ToInt32(row["state"]);
            
            if(!string.IsNullOrEmpty(row["passwd"].ToString()))
                usr.password = row["passwd"].ToString();

            return usr;
        }
    }
}
