using System.Runtime.InteropServices;
using System.Windows.Web;

namespace Email_Inboxes.Controls
{
    /// <summary>
    /// Provides a version of <see cref="System.Windows.Web.WebViewHost"/> to use as a control.
    /// </summary>
    public sealed partial class WebView : UserControl
    {
        #region PInvoke

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        #endregion

        public WebViewHost WebViewHost => host;
        WebViewHost host;

        public WebView(WebViewEnvironment env = null, WebViewControllerOptions options = null)
        {
            host = new WebViewHost();
            host.GenerateWebView(env, options);

            Loaded += View_Loaded;
            SizeChanged += View_SizeChanged;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            host.Show();
            host.Top = host.Left = 0;
            SetParent(host.Handle, (nint)XamlRoot.ContentIslandEnvironment.AppWindowId.Value);
            host.Width = ActualWidth;
            host.Height = ActualHeight;
            host.CurveCorners(radius);
        }

        private void View_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            host.Width = e.NewSize.Width;
            host.Height = e.NewSize.Height;
            host.Top = host.Left = 0;
            host.CurveCorners(radius);
        }

        public new int CornerRadius
        {
            get => radius;
            set => host.CurveCorners(radius = value);
        }
        private int radius = 0;
    }
}
