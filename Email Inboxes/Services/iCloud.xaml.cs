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
using static Email_Inboxes.MainWindow;

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

            //Creates IcloudWebView and sets the content of the page to it
            if (WebViews.IcloudWebView == null)
            {
                WebViews.IcloudWebView = new WebView2() { Source = new Uri("https://www.icloud.com/mail") };
            }
            this.Content = WebViews.IcloudWebView;

            //Runs method CoreWebView2Intitialized once the title suggests is done 
            WebViews.IcloudWebView.CoreWebView2Initialized += CoreWebView2Initialized;
        }

        private void CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            //Allows links to be opened in the default browser
            WebViews.IcloudWebView.CoreWebView2.NewWindowRequested += NewWindowRequested;

            //Enables navigation buttons based on page navigation status
            WebViews.IcloudWebView.CoreWebView2.SourceChanged += SourceChanged;
        }

        private void SourceChanged(CoreWebView2 sender, CoreWebView2SourceChangedEventArgs args)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)mw.nvSample.SelectedItem;
            if (selectedItem.Name == "NavItem_iCloud")
            {
                mw.BackButton.IsEnabled = WebViews.IcloudWebView.CanGoBack;
                mw.ForwardButton.IsEnabled = WebViews.IcloudWebView.CanGoForward;
            }
        }

        private void NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
        {
            //If link isn't a Outlook URL, it opens it in an external browser
            if (args.Uri.Contains("https://www.icloud.com/mail") || args.Uri.Contains("https://icloud.com/mail"))
            {
                args.Handled = true;
                WebViews.IcloudWebView.Source = new Uri(args.Uri);
            }
            else
            {
                args.Handled = true;
                Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true });
            }
        }
    }
}
