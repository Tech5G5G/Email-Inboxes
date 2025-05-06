using Microsoft.UI.Xaml.Markup;

namespace Email_Inboxes.Markup
{
    public class BackdropType : MarkupExtension
    {
        public Settings.BackdropType Backdrop { get; set; }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Backdrop;
    }

    public class PaneDisplayMode : MarkupExtension
    {
        public NavigationViewPaneDisplayMode Mode { get; set; }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Mode;
    }

    public class OutlookType : MarkupExtension
    {
        public Settings.OutlookType Type { get; set; }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Type;
    }

    public class PageType : MarkupExtension
    {
        public Settings.PageType Page { get; set; }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Page;
    }

    public class CalendarService : MarkupExtension
    {
        public Settings.CalendarService Service { get; set; }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Service;
    }

    public class ToDoService : MarkupExtension
    {
        public Settings.ToDoService Service { get; set; }

        protected override object ProvideValue(IXamlServiceProvider serviceProvider) => Service;
    }
}
