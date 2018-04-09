using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProcessCommunication.SharedMemory
{
    public class SharedMemorySend
    {
        public delegate void recv_data(string data);
        public recv_data RecvData = null;

        private Semaphore m_write;
        private Semaphore m_read;
        private IntPtr m_handle;
        private IntPtr m_addr;
        private uint m_mapLength;
        private string m_data = "";

        // 线程用来发送数据
        Thread threadRead;

        private static SharedMemorySend _shareSend = null;

        public static SharedMemorySend GetInst()
        {
            if (null == _shareSend)
                _shareSend = new SharedMemorySend();
            return _shareSend;
        }

        /// <summary>
        /// 初始化共享内存数据，创建一个共享内存
        /// </summary>
        public void Init(string writeSemaphore = @"WriteMap", string readSemaphore = @"ReadMap", string sharedMemoryName = @"shareMemory", uint dataSize = DATA_SIZE)
        {
            m_write = new Semaphore(1, 1, "WriteMap");
            m_read = new Semaphore(0, 1, "ReadMap");
            m_mapLength = dataSize;
            IntPtr hFile = new IntPtr(INVALID_HANDLE_VALUE);
            m_handle = CreateFileMapping(hFile, 0, PAGE_READWRITE, 0, m_mapLength, "shareMemory");
            m_addr = MapViewOfFile(m_handle, FILE_MAP_ALL_ACCESS, 0, 0, 0);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void UnInit()
        {
            if (null != m_write)
            {
                m_write.Release();
                m_write.Dispose();
            }
            if (null != m_read)
            {
                m_read.Release();
                m_read.Dispose();
            }

            if (null != m_handle)
            {
                CloseHandle(m_handle);
                m_handle = new IntPtr(INVALID_HANDLE_VALUE);
            }

            if (null != m_addr)
            {
                UnmapViewOfFile(m_addr);
                m_addr = new IntPtr(INVALID_HANDLE_VALUE);
            }
        }

        public bool StartSendData(string data)
        {
            if (null == m_write || null == m_read ||
                null == m_handle || null == m_addr)
                return false;

            m_data = data;
            threadRead = new Thread(new ThreadStart(SendDataFunc));
            threadRead.Start();
            return true;
        }

        protected void SendDataFunc()
        {
            SendData(m_data);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SendData(string data)
        {
            try
            {
                m_write.WaitOne();
                byte[] sendStr = Encoding.Default.GetBytes(data + '\0');

                // 如果要是超长的话，应里外处理，最好是分配足够的内存
                System.Diagnostics.Debug.WriteLine(data);
                if (sendStr.Length < m_mapLength)
                    Copy(sendStr, m_addr);

                m_read.Release();
                return true;
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                return false;
            }
        }
        
        static unsafe void Copy(byte[] byteSrc, IntPtr dst)
        {
            fixed (byte* pSrc = byteSrc)
            {
                byte* pDst = (byte*)dst;
                byte* psrc = pSrc;
                for (int i = 0; i < byteSrc.Length; i++)
                {
                    *pDst = *psrc;
                    pDst++;
                    psrc++;
                }
            }
        }

        const int INVALID_HANDLE_VALUE = -1;
        const int PAGE_READWRITE = 0x04;
        const int FILE_MAP_ALL_ACCESS = 0x0002;
        const int FILE_MAP_WRITE = 0x0002;
        const uint DATA_SIZE = 1024 * 10;

        #region import dll func

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hwnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindows(IntPtr hwnd);

        // shared memory
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateFileMapping(IntPtr hFile,    // HANDLE hFile,
            UInt32 lpAttributes, //LPSECURITY_ATTRIBUTES lpAttributes,  //0
            UInt32 flProtect,//DWORD flProtect
            UInt32 dwMaximumSizeHigh,//DWORD dwMaximumSizeHigh,
            UInt32 dwMaximumSizeLow,//DWORD dwMaximumSizeLow,
            string lpName//LPCTSTR lpName
           );
        [DllImport("Kernel32.dll", EntryPoint = "OpenFileMapping")]
        private static extern IntPtr OpenFileMapping(
            UInt32 dwDesiredAccess,//DWORD dwDesiredAccess,
            int bInheritHandle,//BOOL bInheritHandle,
            string lpName//LPCTSTR lpName
           );

        [DllImport("Kernel32.dll", EntryPoint = "MapViewOfFile")]
        private static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,//HANDLE hFileMappingObject,
            UInt32 dwDesiredAccess,//DWORD dwDesiredAccess
            UInt32 dwFileOffsetHight,//DWORD dwFileOffsetHigh,
            UInt32 dwFileOffsetLow,//DWORD dwFileOffsetLow,
            UInt32 dwNumberOfBytesToMap//SIZE_T dwNumberOfBytesToMap
           );

        [DllImport("Kernel32.dll", EntryPoint = "UnmapViewOfFile")]
        private static extern int UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("Kernel32.dll", EntryPoint = "CloseHandle")]
        private static extern int CloseHandle(IntPtr hObject);

        #endregion
    }
}
