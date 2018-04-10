using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JuYuan.model;

namespace JuYuan.controls
{
    /// <summary>
    /// StartEndDatePicker.xaml 
    /// </summary>
    public partial class StartEndDatePicker : UserControl
    {

        public delegate void DataChangedEventManager();
        public DataChangedEventManager DateChangedHandler;


        private DateTime ori_time = Convert.ToDateTime("1970-01-01 00:00:00");
        private DateTime rmt_time = Convert.ToDateTime("2099-12-31 23:59:59");

        public string TitleName
        {
            set
            {
                SetStartTitleText(value);
            }
        }
        public StartEndDatePicker()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(StartEndDatePicker_Loaded);
        }

        private void StartEndDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            this.StartDatePicker.SelectedDate = ori_time;
            this.StartDatePicker.DisplayDate = ori_time;

            this.EndDatePicker.SelectedDate = DateTime.Now;
            this.EndDatePicker.DisplayDate = DateTime.Now;

            this.QueryTimeCbx.ItemsSource = null;
            this.QueryTimeCbx.ItemsSource = JuYuan.ui.TimeType.GetInst().SelectMemberTimeTypeList;
            this.QueryTimeCbx.DisplayMemberPath = @"Name";
            this.QueryTimeCbx.SelectedValuePath = @"Type";
            
            try
            {
                this.QueryTimeCbx.SelectedIndex = 1;
            }
            catch (Exception ex)
            {

            }
        }

        private void QueryTimeCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TimeTypeData td = (TimeTypeData)this.QueryTimeCbx.SelectedItem;
                DateTime st, et;
                FlushStartAndEndTime(td, out st, out et);
                SetDisplayDate(st, et);
            }
            catch (Exception ex){ }
        }

        private void FlushStartAndEndTime(TimeTypeData data, out DateTime st, out DateTime et)
        {
            st = new DateTime(ori_time.Year, ori_time.Month, ori_time.Day);
            et = DateTime.Now;
            switch (data.Type)
            {
                case (int)JuYuan.ui.TIMETYPE.TIMETYPE_ALL:
                    {
                        st = new DateTime(ori_time.Year, ori_time.Month, ori_time.Day);
                        et = DateTime.Now;
                    }
                    break;

                case (int)JuYuan.ui.TIMETYPE.TIMETYPE_TODAY:
                    {
                        st = DateTime.Now;
                        et = DateTime.Now;
                    }
                    break;

                case (int)JuYuan.ui.TIMETYPE.TIMETYPE_YESTDAY:
                    {
                        DateTime dt = DateTime.Now;
                        st = et = dt.AddDays(-1);
                    }
                    break;

                case (int)JuYuan.ui.TIMETYPE.TIMETYPE_TOMONTH:
                    {
                        et = DateTime.Now;
                        st = new DateTime(et.Year, et.Month, 1);
                    }
                    break;

                case (int)JuYuan.ui.TIMETYPE.TIMETYPE_YESTMONTH:
                    {
                        DateTime dt = DateTime.Now;
                        int beforeY = 0;
                        int beforeM = 0;
                        if (dt.Month <= 1)
                        {
                            beforeY = dt.Year - 1;
                            beforeM = 12;
                        }
                        else
                        {
                            beforeY = dt.Year;
                            beforeM = dt.Month - 1;
                        }
                        st = new DateTime(beforeY, beforeM, 1);
                        et = new DateTime(beforeY, beforeM, DateTime.DaysInMonth(beforeY, beforeM));
                    }
                    break;

                case (int)JuYuan.ui.TIMETYPE.TIMETYPE_TOYEAR:
                    {
                        et = DateTime.Now;
                        st = new DateTime(et.Year, 1, 1);
                    }
                    break;

                case (int)JuYuan.ui.TIMETYPE.TIMETYPE_YESTYEAR:
                    {
                        DateTime dt = DateTime.Now;
                        st = new DateTime(dt.Year - 1, 1, 1);
                        et = new DateTime(dt.Year - 1, 12, 31);
                    }
                    break;
            }
        }

        private void SetDisplayDate(DateTime start, DateTime end)
        {
            string st = start.ToString("yyyy-MM-dd");
            DateTime s = DateTime.Parse(st);

            string et = end.ToString("yyyy-MM-dd");
            DateTime e = DateTime.Parse(et).AddDays(1).AddSeconds(-1);

            
            this.StartDatePicker.BlackoutDates.Clear();
            this.EndDatePicker.BlackoutDates.Clear();

            this.StartDatePicker.SelectedDate = s;
            this.StartDatePicker.DisplayDate = s;

            this.EndDatePicker.SelectedDate = e;
            this.EndDatePicker.DisplayDate = e;

            if (DateChangedHandler != null) 
                DateChangedHandler();

            SelectedDateChanged(s, e);
        }

        public void GetDisplayDateTime(out DateTime start, out DateTime end)
        {
            start = this.StartDatePicker.SelectedDate == null ? this.StartDatePicker.DisplayDate : (DateTime)this.StartDatePicker.SelectedDate;
            end = this.EndDatePicker.SelectedDate == null ? this.EndDatePicker.DisplayDate : (DateTime)this.EndDatePicker.SelectedDate;

            start = Convert.ToDateTime(start.ToShortDateString() + " 0:00:00");
            end = Convert.ToDateTime(end.ToShortDateString() + " 23:59:59");
        }

        public void GetDisplayDateShortString( out string start, out string end)
        {
            start = this.StartDatePicker.Text;
            end = this.EndDatePicker.Text;
        }

        public void GetDisplayDateLongString(out string start, out string end)
        {
            start = this.StartDatePicker.Text + " 00:00:00";
            end = this.EndDatePicker.Text + " 23:59:59";
        }

        public void SetStartTitleText(string title )
        {
            this.TitleText.Text = title;
        }
        
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedDateChanged(DateTime st, DateTime et)
        {  
            this.EndDatePicker.BlackoutDates.Add(new CalendarDateRange(ori_time, st.AddSeconds(-1)));
            this.StartDatePicker.BlackoutDates.Add(new CalendarDateRange(et.AddSeconds(1), rmt_time));
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.StartDatePicker.BlackoutDates.Clear();
                DateTime end = this.EndDatePicker.SelectedDate == null ? this.EndDatePicker.DisplayDate : (DateTime)this.EndDatePicker.SelectedDate;
                this.StartDatePicker.BlackoutDates.Add(new CalendarDateRange(end.AddDays(1), rmt_time));

                if (DateChangedHandler != null) 
                    DateChangedHandler();
            }
            catch (Exception ex) { }
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.EndDatePicker.BlackoutDates.Clear();
                DateTime start = this.StartDatePicker.SelectedDate == null ? this.StartDatePicker.DisplayDate : (DateTime)this.StartDatePicker.SelectedDate;
                this.EndDatePicker.BlackoutDates.Add(new CalendarDateRange(ori_time, start.AddDays(-1)));

                if (DateChangedHandler != null) 
                    DateChangedHandler();
            }
            catch (Exception ex) { }
            
        }
    }
}
