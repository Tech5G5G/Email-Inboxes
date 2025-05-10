using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace System.Windows.Web
{
    /// <summary>
    /// A WPF window whose content is set to a WebView2.
    /// </summary>
    public partial class WebViewHost : Window
    {
        #region PInvoke

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

        [DllImport("user32.dll")]
        private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetDpiForWindow(IntPtr hWnd);

        #endregion

        public nint Handle { get; private set; } /*=> webView.Handle;*/

        internal CoreWebView2 Core => webView?.CoreWebView2;

        private WebView2 webView;

        /// <summary>
        /// Initializes a new instance of <see cref="WebViewHost"/>.
        /// </summary>
        public WebViewHost()
        {
            Height = 450;
            Width = 800;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;

            var interop = new WindowInteropHelper(this);
            interop.EnsureHandle();
            Handle = interop.Handle;
        }

        public async void GenerateWebView(WebViewEnvironment env = null, WebViewControllerOptions options = null)
        {
            Content = webView = new() { DefaultBackgroundColor = Drawing.Color.Transparent, Source = new Uri("https://www.google.com") };
            await webView.EnsureCoreWebView2Async(env?.Environment, options?.Options);

            SetupEvents();
        }

        public void CurveCorners(int radius)
        {
            double scale = GetDpiForWindow(Handle) / 96.0;
            var hrgn = CreateRoundRectRgn(0, 0, (int)(ActualWidth * scale), (int)(ActualHeight * scale), (int)(radius * scale), (int)(radius * scale));
            _ = SetWindowRgn(Handle, hrgn, true);
        }
    }
}