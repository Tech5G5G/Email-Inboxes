using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes.Controls
{
    public sealed partial class InboxButton : UserControl
    {
        public InboxButton()
        {
            this.InitializeComponent();

            this.DefaultStyleKey = typeof(InboxButton);
        }

        public event RoutedEventHandler Click;

        //private Button ButtonControl { get; set; }

        //private FontIcon HeaderIcon { get; set; }

        //protected override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();

        //    if (ButtonControl is not null)
        //    {
        //        ButtonControl.Click -= ButtonControl_Click;
        //    }

        //    if (GetTemplateChild(nameof(ButtonControl)) is Button button)
        //    {
        //        ButtonControl = button;
        //        ButtonControl.Click += ButtonControl_Click;
        //    }

        //    if (GetTemplateChild(nameof(HeaderIcon)) is FontIcon fontIcon)
        //    {
        //        HeaderIcon = fontIcon;
        //        HeaderIcon.FontFamily = this.IconFontFamily;
        //    }
        //}

        //private void ButtonControl_Click(object sender, RoutedEventArgs e)
        //{
        //    Click?.Invoke(this, e);
        //}

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public FontFamily IconFontFamily
        {
            get { return (FontFamily)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(InboxButton), new PropertyMetadata((FontFamily)App.Current.Resources["SymbolThemeFontFamily"]));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public string ActionIcon
        {
            get { return (string)GetValue(ActionIconProperty); }
            set { SetValue(ActionIconProperty, value); }
        }

        public static readonly DependencyProperty ActionIconProperty = DependencyProperty.Register("ActionIcon", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));
    }
}
