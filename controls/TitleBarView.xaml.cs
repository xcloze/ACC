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

namespace JuYuan.controls
{
    /// <summary>
    /// Interaction logic for TitleBarView.xaml
    /// </summary>
    public partial class TitleBarView : UserControl
    {
        public TitleBarView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(TitleBarView_Loaded);
        }
        void TitleBarView_Loaded(object sender, RoutedEventArgs e)
        {
            this.TitleTime.Text = DateTime.Now.ToString("dddd ", new System.Globalization.CultureInfo("zh-cn")) +
                                  DateTime.Now.ToString("yyyy-MM-dd ") + DateTime.Now.ToLongTimeString();
            InitTimer();
        }

        private System.Windows.Threading.DispatcherTimer ShowTimer;

        private void InitTimer()
        {
            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);//起个Timer一直获取当前时间
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();
        }

        private void ShowCurTimer(object sender, EventArgs e)
        {
            this.TitleTime.Text = DateTime.Now.ToString("dddd ", new System.Globalization.CultureInfo("zh-cn")) +
                                  DateTime.Now.ToString("yyyy-MM-dd ") + DateTime.Now.ToLongTimeString();
        }

    }
}
