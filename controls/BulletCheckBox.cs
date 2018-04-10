using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CustomCheckBox.controls
{
    /// <summary>
    /// BulletCheckBox
    /// </summary>
    public class BulletCheckBox : CheckBox
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(BulletCheckBox), new PropertyMetadata("Off"));
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty CheckedTextProperty = DependencyProperty.Register(
            "CheckedText", typeof(string), typeof(BulletCheckBox), new PropertyMetadata("On"));
        /// <summary>
        /// 
        /// </summary>
        public string CheckedText
        {
            get { return (string)GetValue(CheckedTextProperty); }
            set { SetValue(CheckedTextProperty, value); }
        }

        public static readonly DependencyProperty CheckedForegroundProperty =
            DependencyProperty.Register("CheckedForeground", typeof(Brush), typeof(BulletCheckBox), new PropertyMetadata(Brushes.WhiteSmoke));
        /// <summary>
        /// 
        /// </summary>
        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty CheckedBackgroundProperty =
            DependencyProperty.Register("CheckedBackground", typeof(Brush), typeof(BulletCheckBox), new PropertyMetadata(Brushes.LimeGreen));
        /// <summary>
        /// 
        /// </summary>
        public Brush CheckedBackground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }

        static BulletCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BulletCheckBox), new FrameworkPropertyMetadata(typeof(BulletCheckBox)));
        }
    }
}
