using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JuYuan.model;
using System.Data;
using MySql.Data.MySqlClient;
using mysqlApp.utils;
using Model.model;
using JuYuan.utils;

namespace JuYuan.dal
{
    /// <summary>
    /// 系统设置项
    /// </summary>
    class SystemSettingDAL : MySqlUtils
    {
        /// <summary>
        /// 打印设置
        /// </summary>
        /// <returns></returns>
        public ClientPrintSetting GetPrintSetting()
        {
            ClientPrintSetting setting = new ClientPrintSetting();

            DataTable dt = ExecuteDataTable(@"SELECT
                                              	`key`,
                                              	`value`
                                              FROM
                                              	system_setting
                                              WHERE
                                              	`key` IN (
                                              		'is_print',
                                              		'print_mode',
                                              		'printer',
                                              		'print_title1',
                                              		'print_title2',
                                              		'print_foot1',
                                              		'print_foot2',
                                              		'print_foot3',
                                              		'print_amount'
                                              	)
                                              AND client_id =@client_id",
                            new MySqlParameter("@client_id", GlobalConst.CurrentClientID));

            for(int i=0; i < dt.Rows.Count; i++)
            {
                switch(dt.Rows[i]["key"].ToString())
                {
                    case "is_print":
                        setting.IsPrint = Convert.ToInt32(dt.Rows[i]["value"].ToString()) == 1;
                        break;
                    case "print_mode":
                        setting.PrintMode = dt.Rows[i]["value"].ToString();
                        break;
                    case "printer":
                        setting.Printer = dt.Rows[i]["value"].ToString();
                        break;
                    case "print_title1":
                        setting.PrinTitle1 = dt.Rows[i]["value"].ToString();
                        break;
                    case "print_title2":
                        setting.PrinTitle2 = dt.Rows[i]["value"].ToString();
                        break;
                    case "print_foot1":
                        setting.PrintFoot1 = dt.Rows[i]["value"].ToString();
                        break;
                    case "print_foot2":
                        setting.PrintFoot2 = dt.Rows[i]["value"].ToString();
                        break;
                    case "print_foot3":
                        setting.PrintFoot3 = dt.Rows[i]["value"].ToString();
                        break;
                    case "print_amount":
                        setting.Number = Convert.ToInt32(dt.Rows[i]["value"].ToString());
                        break;
                    default:
                        break;
                }
            }
            return setting;
        }

        /// <summary>
        /// 获取单项配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSystemSettingByKey(string key)
        {
            DataTable dt = ExecuteDataTable(@"select `value` from system_setting where `key`=@key and client_id=@client_id",
                new MySqlParameter("@key", key),
                new MySqlParameter("@client_id", GlobalConst.CurrentClientID));

            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows[0]["value"].ToString();
        }

        public Dictionary<string, string> GetSystemSetting(List<string> key_list)
        {
            if(key_list == null || key_list.Count == 0)
                return null;

            Dictionary<string, string> value_map = new Dictionary<string,string>();
            string key_str = "";    // 拼接字符串

            foreach(var k in key_list)
            {
                key_str +="'" + k + "',";
            }
            key_str = key_str.Substring(0, key_str.Length - 1); // 去掉最后的逗号

            DataTable dt = ExecuteDataTable(@"select `key`,`value` from system_setting where `key` in (" + key_str + ") and client_id=@client_id",
                   new MySqlParameter("@client_id", GlobalConst.CurrentClientID));

            for (int i=0; i<dt.Rows.Count; i++)
            {
                foreach(var k in key_list)
                {
                    if(dt.Rows[i]["key"].ToString() == k )
                    {
                        value_map.Add(k, dt.Rows[i]["value"].ToString());
                        break;
                    }
                }
            }

            return value_map;
        }
    }
}
