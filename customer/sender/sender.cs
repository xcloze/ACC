using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using copydata;
using System.Diagnostics;

namespace sender_process.sender
{
    
    public class sender
    {
        public struct tag_send_msg
        {
            public string m_processName;
            public int m_type;
            public string m_msg;
        };
        
        const int WM_COPYDATA = 0x004A;

        public static bool send_msg(string process_name, int type, string msg)
        {
            IntPtr receiver_handler = FindWindow(null, process_name);
            if (IntPtr.Zero == receiver_handler)
            {
                return false;
            }

            byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
            int len = sarr.Length;
            COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)Convert.ToInt16(type);
            cds.cbData = len + 1;
            cds.lpData = msg;

            SendMessage(receiver_handler, WM_COPYDATA, 0, ref cds);
            
            return true;
        }

        public static bool send_msg2(string process_name, int type, string msg)
        {
            IntPtr receiver_handler = FindWindow(null, process_name);
            if (IntPtr.Zero == receiver_handler)
            {
                return false;
            }

            byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
            int len = sarr.Length;
            COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)Convert.ToInt16(type);
            cds.cbData = len + 1;
            cds.lpData = msg;

            SendMessage(receiver_handler, WM_COPYDATA, 0, ref cds);

            return true;
        }

        public static bool send_thread(string process_name, int type, string msg)
        {
            tag_send_msg str_msg = new tag_send_msg();
            str_msg.m_processName = process_name;
            str_msg.m_type = type;
            str_msg.m_msg = msg;

            Thread thread = new Thread(new ParameterizedThreadStart(ThreadFunc));
            thread.IsBackground = true;
            thread.Start(str_msg);

            return true;
        }

        protected static void ThreadFunc(object obj)
        {
            tag_send_msg str_msg = (tag_send_msg)obj;

            IntPtr receiver_handler = FindWindow(null, str_msg.m_processName);
            if (IntPtr.Zero == receiver_handler)
            {
                return;
            }

            byte[] sarr = System.Text.Encoding.Default.GetBytes(str_msg.m_msg);
            int len = sarr.Length;
            COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)Convert.ToInt16(str_msg.m_type);
            cds.cbData = len + 1;
            cds.lpData = str_msg.m_msg;

            SendMessage(receiver_handler, WM_COPYDATA, 0, ref cds);
        }

        //Win32 API函数
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]

        private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "PostThreadMessage")]
        public static extern bool PostThreadMessage(int threadId, int msg, int wParam, ref COPYDATASTRUCT lParam);
       
    }
}
