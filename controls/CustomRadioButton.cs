using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;


namespace CustomRadioButton.Controls
{
    public class CustomRadioButton : RadioButton
    {
        static CustomRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomRadioButton),
                new FrameworkPropertyMetadata(typeof(CustomRadioButton)));
        }

        #region Dependency Properties

        public string IconImg
        {
            get { return (string)GetValue(IconImgProperty); }
            set { SetValue(IconImgProperty, value); }
        }

        public static readonly DependencyProperty IconImgProperty =
            DependencyProperty.Register("IconImg", typeof(string), typeof(CustomRadioButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));


        public string NormalImage
        {
            get { return (string)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }

        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(string), typeof(CustomRadioButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public Brush NormalForeground
        {
            get { return (Brush)GetValue(NormalForegroundProperty); }
            set { SetValue(NormalForegroundProperty, value); }
        }

        public static readonly DependencyProperty NormalForegroundProperty =
            DependencyProperty.Register("NormalForeground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));

        public Brush NormalBackground
        {
            get { return (Brush)GetValue(NormalBackgroundProperty); }
            set { SetValue(NormalBackgroundProperty, value); }
        }

        public static readonly DependencyProperty NormalBackgroundProperty =
            DependencyProperty.Register("NormalBackground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));

        public string HoverImage
        {
            get { return (string)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register("HoverImage", typeof(string), typeof(CustomRadioButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public Brush HoverForeground
        {
            get { return (Brush)GetValue(HoverForegroundProperty); }
            set { SetValue(HoverForegroundProperty, value); }
        }

        public static readonly DependencyProperty HoverForegroundProperty =
            DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));

        public Brush HoverBackground
        {
            get { return (Brush)GetValue(HoverBackgroundProperty); }
            set { SetValue(HoverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));


        public string PressedImage
        {
            get { return (string)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register("PressedImage", typeof(string), typeof(CustomRadioButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        public static readonly DependencyProperty PressedForegroundProperty =
          DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));


        public string DisabledImage
        {
            get { return (string)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        public static readonly DependencyProperty DisabledImageProperty =
            DependencyProperty.Register("DisabledImage", typeof(string), typeof(CustomRadioButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, ImageSourceChanged));

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        public static readonly DependencyProperty DisabledForegroundProperty =
           DependencyProperty.Register("DisabledForeground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));

        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        public static readonly DependencyProperty DisabledBackgroundProperty =
            DependencyProperty.Register("DisabledBackground", typeof(Brush), typeof(CustomRadioButton), new PropertyMetadata(Brushes.Transparent));


        private static void ImageSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Application.GetResourceStream(new Uri("pack://application:,,," + (string)e.NewValue));
        }

        public Visibility BorderVisibility
        {
            get { return (Visibility)GetValue(BorderVisibilityProperty); }
            set { SetValue(BorderVisibilityProperty, value); }
        }

        public static readonly DependencyProperty BorderVisibilityProperty =
            DependencyProperty.Register("BorderVisibility", typeof(Visibility), typeof(CustomRadioButton),
            new FrameworkPropertyMetadata(Visibility.Hidden, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region 自定义事件

        #endregion

    }

}
