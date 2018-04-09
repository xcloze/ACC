using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using JuYuan.utils;
using copydata;

namespace JuYuan.customer
{
    class StartCustomer
    {
        public static void Start()
        {
            if (JuYuan.utils.GlobalConst.showCustomer)
            {
                //Customer.DisplayClient.GetInst().ShowView();
            }
        }

        public static void Start(string exePath)
        {
            if (0 == exePath.Length)
            {
                return;
            }

            string path = Application.StartupPath;
            string filePath = path;
            string fileName = exePath;

            Process proc = new Process();
            proc.StartInfo.UseShellExecute = true;//是否使用操作系统外壳程序启动进程

            proc.StartInfo.WorkingDirectory = filePath;//启动进程的初始目录
            proc.StartInfo.FileName = fileName + ".exe";
            String f1 = filePath + "\\" + fileName + ".exe";
            if (File.Exists(f1))
            {
                proc.Start();
            }
        }

        public static void CloseShowProcess()
        {
            if (true == GlobalConst.showCustomer)
            {
                // close the show window process
                string process_name = sender_process.sender.DisplayClientInfo.DisplayClientShowName;
                string msg = "";
                int type = (int)InfoType.InfoType_Close;
                sender_process.sender.sender.send_msg(process_name, type, msg);

                process_name = sender_process.sender.DisplayClientInfo.DisplayClientSetName;
                sender_process.sender.sender.send_msg(process_name, type, msg);
            }
        }
    }
}
