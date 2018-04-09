using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ProcessCommunication.SharedMemory;

namespace JuYuan.sharedmemory
{
    struct send_msg_struct
    {
        public int m_type;
        public object m_obj;
    };

    class sender
    {
        private static sender _sender = null;

        public static sender GetInst()
        {
            if (null == _sender)
            {
                _sender = new sender();
                SharedMemorySend.GetInst().Init();
            }
            return _sender;
        }

        public void Release()
        {
            SharedMemorySend.GetInst().UnInit();
        }

        public bool SendSignl(int type, object obj)
        {
            if (null == obj)
                return false;

            send_msg_struct Data = new send_msg_struct();
            Data.m_type = type;
            Data.m_obj = obj;

            string msg = JsonConvert.SerializeObject(Data);

            return SharedMemorySend.GetInst().StartSendData(msg);
        }
    }
}
