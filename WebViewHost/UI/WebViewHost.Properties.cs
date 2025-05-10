namespace System.Windows.Web
{
    public partial class WebViewHost
    {
        public Uri Source
        {
            get => Core is not null && string.IsNullOrWhiteSpace(Core.Source) ? new(Core.Source) : null;
            set => Core?.Navigate(value.OriginalString);
        }
    }
}