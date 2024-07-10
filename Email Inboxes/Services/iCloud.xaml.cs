using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class iCloud : Page
    {
        MainWindow mw = (MainWindow)((App)Application.Current).m_window;

        public iCloud()
        {
            this.InitializeComponent();

            //Runs method CoreWebView2Intitialized once the title suggests is done 
            IcloudWebView.CoreWebView2Initialized += CoreWebView2Initialized;
        }

        private void CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            //Allows links to be opened in the default browser
            IcloudWebView.CoreWebView2.NewWindowRequested += NewWindowRequested;

            //Enables navigation buttons based on page navigation status
            IcloudWebView.CoreWebView2.SourceChanged += SourceChanged;
        }

        private void SourceChanged(CoreWebView2 sender, CoreWebView2SourceChangedEventArgs args)
        {
            mw.BackButton.IsEnabled = IcloudWebView.CanGoBack;
            mw.ForwardButton.IsEnabled = IcloudWebView.CanGoForward;
        }

        private void NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
        {
            //If link isn't a Outlook URL, it opens it in an external browser
            if (args.Uri.Contains("https://www.icloud.com/mail") || args.Uri.Contains("https://icloud.com/mail"))
            {
                args.Handled = true;
                IcloudWebView.Source = new Uri(args.Uri);
            }
            else
            {
                args.Handled = true;
                Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true });
            }
        }
    }
}
