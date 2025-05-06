namespace Email_Inboxes.Controls
{
    public sealed partial class InboxButton : UserControl
    {
        public InboxButton()
        {
            this.InitializeComponent();
            this.DefaultStyleKey = typeof(InboxButton);

            ButtonControl.Click += (sender, e) => this.Click?.Invoke(sender, e);
        }

        public event RoutedEventHandler Click;

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
