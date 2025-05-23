namespace Email_Inboxes.Controls
{
    public sealed partial class InboxButton : UserControl
    {
        public InboxButton()
        {
            this.InitializeComponent();
            this.DefaultStyleKey = typeof(InboxButton);

            ButtonControl.Click += (sender, e) => Click?.Invoke(sender, e);
        }

        public event RoutedEventHandler Click;

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public static DependencyProperty HeaderProperty { get; } = DependencyProperty.Register("Header", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public static DependencyProperty IconProperty { get; } = DependencyProperty.Register("Icon", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static DependencyProperty IconFontFamilyProperty { get; } = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(InboxButton), new PropertyMetadata((FontFamily)App.Current.Resources["SymbolThemeFontFamily"]));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        public static DependencyProperty DescriptionProperty { get; } = DependencyProperty.Register("Description", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public string ActionIcon
        {
            get => (string)GetValue(ActionIconProperty);
            set => SetValue(ActionIconProperty, value);
        }
        public static DependencyProperty ActionIconProperty { get; } = DependencyProperty.Register("ActionIcon", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));
    }
}
