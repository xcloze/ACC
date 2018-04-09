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
    /// Interaction logic for SplitPaging.xaml
    /// </summary>
    public partial class SplitPaging : UserControl
    {
        public delegate int ShowPage(int page);
        public ShowPage showpage_callback = null;

        /// <summary>
        /// 当前页!第 13 条记录（共 200 条）于第 1 页  
        /// </summary>
        private int page = 0;
        private int max_page = 1;
        private int max_record_onpage = 0;
        private int select_record = 0;


        public int SelectRecordIndex
        {
            set
            {
                select_record = value;
                SplitPaging_load();
            }
        }
       
        public SplitPaging()
        {
            InitializeComponent();
            this.hintTxt.Text = "";
            this.onPageNumTxt.Text = "";
        }

        private void SplitPaging_load()
        {
            string text = string.Format("第 {0} 条记录（共 {1} 条）于第 {2} 页  ", select_record, max_record_onpage, page);
            this.hintTxt.Text = text;
            this.onPageNumTxt.Text = page.ToString();
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="max_page"></param>
        /// <param name="max_record"></param>
        public void PageDataInit(int maxPage, int maxRecord)
        {
            page = 1;
            max_page = maxPage;
            max_record_onpage = maxRecord;
            SplitPaging_load();
        }

        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (showpage_callback != null)
            {
                page = 1;
                max_record_onpage = showpage_callback(page);
                SplitPaging_load();
            }
        }

        private void BeforePageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (showpage_callback != null && page -1 > 0)
            {
                page--;
                max_record_onpage = showpage_callback(page);
                SplitPaging_load();
            }
        }

        private void FristNextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (showpage_callback != null && page + 1 <= max_page)
            {
                page++;
                max_record_onpage = showpage_callback(page);
                SplitPaging_load();
            }
        }

        private void EndPageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (showpage_callback != null)
            {
                page = max_page;
                max_record_onpage = showpage_callback(page);
                SplitPaging_load();
            }
        }

        private void onPageNumTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //todo
                if(Convert.ToInt32(onPageNumTxt.Text) <= max_page)
                {
                    page = Convert.ToInt32(onPageNumTxt.Text);
                    if(showpage_callback != null)
                    {
                        max_record_onpage = showpage_callback(page);
                        SplitPaging_load();
                    }
                }
                else
                {
                    onPageNumTxt.Text = page.ToString();
                }
            }
            else
            {
                utils.SystemManager.TxtOnlyNumbersNotPoint_KeyDown(sender, e);
            }
        }
    }
}
