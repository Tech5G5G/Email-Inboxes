namespace Email_Inboxes.Controls
{
    public sealed partial class ButtonedScrollViewer : UserControl
    {
        public new object Content
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        public new static DependencyProperty ContentProperty { get; } = DependencyProperty.Register("Content", typeof(object), typeof(ButtonedScrollViewer), new PropertyMetadata(null));

        public int ScrollOffset
        {
            get => (int)GetValue(ScrollOffsetProperty);
            set => SetValue(ScrollOffsetProperty, value);
        }
        public static DependencyProperty ScrollOffsetProperty { get; } = DependencyProperty.Register("ScrollOffset", typeof(int), typeof(ButtonedScrollViewer), new PropertyMetadata(200));

        public ButtonedScrollViewer()
        {
            this.InitializeComponent();
        }

        private void SetButtonOpacity()
        {
            leftButton.Opacity = view.HorizontalOffset == 0 ? 0 : 1;
            rightButton.Opacity = view.HorizontalOffset == view.ScrollableWidth ? 0 : 1;
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs args) => SetButtonOpacity();

        private void OnViewChanged(object sender, ScrollViewerViewChangedEventArgs args) => SetButtonOpacity();

        private void OnPointerExited(object sender, PointerRoutedEventArgs args) => leftButton.Opacity = rightButton.Opacity = 0;

        private void ScrollLeft(object sender, RoutedEventArgs args) => view.ChangeView(view.HorizontalOffset + -ScrollOffset, null, null);

        private void ScrollRight(object sender, RoutedEventArgs args) => view.ChangeView(view.HorizontalOffset + ScrollOffset, null, null);
    }
}
