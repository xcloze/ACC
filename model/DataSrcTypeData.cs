using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuYuan.model
{

    public class OrderSrcData
    {

        public OrderSrcData()
        {
            SrcName = @"门店";
            SrcType = 0;
        }

        public void SetValue(OrderSrcData or)
        {
            this.SrcName = or.SrcName;
            this.SrcType = or.SrcType;
        }

     
        public string SrcName
        {
            get { return _srcName; }
            set { _srcName = value; }
        }
  
        public int SrcType
        {
            get { return _srcType; }
            set { _srcType = value; }
        }

        private string _srcName = "";

        private int _srcType = 0;
    }

    
    public class PayModeData
    {
        public void SetValue(PayModeData data)
        {
            this.PayModeName = data.PayModeName;
            this.PayModeType = data.PayModeType;
        }

        public string PayModeName
        {
            get { return _payModeName; }
            set { _payModeName = value; }
        }

        public int PayModeType
        {
            get { return _payModeType; }
            set { _payModeType = value; }
        }

        private string _payModeName = "";

        private int _payModeType;

    }

    public class ReturnModeData
    {
        public void SetValue(ReturnModeData data)
        {
            this.ModeName = data.ModeName;
            this.Mode = data.Mode;
        }

        public string ModeName
        {
            get { return _modeName; }
            set { _modeName = value; }
        }

        public int Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private string _modeName = "";
        private int _mode = 0;
    }

 
    public class TimeTypeData
    {
        public void SetValue(TimeTypeData data)
        {
            this.Name = data.Name;
            this.Type = data.Type;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

       
        private string _name = "";
        private int _type = 0;
    }

  
    public class ValidatePwdTypeData
    {
        public void SetValue(ValidatePwdTypeData data)
        {
            this.Name = data.Name;
            this.ValidateType = data.ValidateType;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int ValidateType
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _name = "";
        private int _type = 0;
    }

    
    public class CardStatusData
    {
        public void SetValue(CardStatusData data)
        {
            this.Name = data.Name;
            this.Status = data.Status;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _name = "";
        private int _status = 0;
    }

  
    public class RechargeActData
    {
        public void SetValue(RechargeActData data)
        {
            this.ID = data.ID;
            this.ActNo = data.ActNo;
            this.Name = data.Name;
            this.Instruction = data.Instruction;
            this.ActModeID = data.ActModeID;
            this.ActModeName = data.ActModeName;
            this.Recharge = data.Recharge;
            this.Receive = data.Receive;
            this.Status = data.Status;
            this.StartDate = data.StartDate;
            this.EndDate = data.EndDate;
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string ActNo
        {
            get { return _actNo; }
            set { _actNo = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Instruction
        {
            get { return _instruction; }
            set { _instruction = value; }
        }

        public int ActModeID
        {
            get { return _actModeID; }
            set { _actModeID = value; }
        }

        public string ActModeName
        {
            get { return _actModeName; }
            set { _actModeName = value; }
        }

        public decimal Recharge
        {
            get { return _recharge; }
            set { _recharge = value; }
        }

        public decimal Receive
        {
            get { return _receive; }
            set { _receive = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        
    }

  
    public class SexData
    {
        public void SetValue(SexData data)
        {
            this.Name = data.Name;
            this.SexType = data.SexType;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int SexType
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _name = "";
        private int _type = 0;
    }

   
    public class CredentialsData
    {
        public void SetValue(CredentialsData data)
        {
            this.Name = data.Name;
            this.CredentialsType = data.CredentialsType;
            this.No = data.No;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int CredentialsType
        {
            get { return _type; }
            set { _type = value; }
        }

        public string No
        {
            get { return _no; }
            set { _no = value; }
        }

        private string _name = "";
        private int _type = 0;
        private string _no = "";
    }

    public class OperatorData
    {
        public void SetValue(OperatorData data)
        {
            this.Name = data.Name;
            this.OptrID = data.OptrID;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int OptrID
        {
            get; set;
        }

        private string _name = "";
        private string _id = "";
    }
  
    public class OperatType
    {
        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }


    public class PrintMode
    {
        public void SetValue(PrintMode data)
        {
            this.Type = data.Type;
            this.Name = data.Name;
        }

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _type = 0;
        private string _name = "";
    }

    public class PrinterData
    {
        public void SetValue(PrinterData data)
        {
            this.ID = data.ID;
            this.Name = data.Name;
        }

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _id = "";
        private string _name = "";
    }

    class DataSrcTypeData
    {
    }
}
