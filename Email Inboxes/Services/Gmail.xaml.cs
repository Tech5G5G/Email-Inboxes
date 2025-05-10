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
using System.Diagnostics;
using Microsoft.Web.WebView2.Core;
using static Email_Inboxes.MainWindow;
using Email_Inboxes.Settings;
using Windows.UI.WebUI;

namespace Email_Inboxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Gmail : Page
    {
        MainWindow mw = (MainWindow)((App)Application.Current).m_window;

        public Gmail()
        {
            ConstructWebView();

            this.InitializeComponent();
        }

        private async void ConstructWebView()
        {
            //Creates GmailWebView and sets the content of the page to it
            if (WebViews.GmailWebView == null)
            {
                WebViews.GmailWebView = new WebView2();
                var env = await CoreWebView2Environment.CreateAsync();
                var options = env.CreateCoreWebView2ControllerOptions();
                options.ProfileName = "gmailhaha";
                await WebViews.GmailWebView.EnsureCoreWebView2Async(env, options);
                WebViews.GmailWebView.Source = new Uri("https://mail.google.com/mail/u/0/");
            }
            this.Content = WebViews.GmailWebView;

            //Runs method CoreWebView2Intitialized once the title suggests is done 
            WebViews.GmailWebView.CoreWebView2Initialized += CoreWebView2Initialized;
        }

        private void CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            //Allows links to be opened in the default browser
            WebViews.GmailWebView.CoreWebView2.NewWindowRequested += NewWindowRequested;

            //Enables navigation buttons based on page navigation status
            WebViews.GmailWebView.CoreWebView2.SourceChanged += SourceChanged;
        }

        private void SourceChanged(CoreWebView2 sender, CoreWebView2SourceChangedEventArgs args)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)mw.nvSample.SelectedItem;
            if (selectedItem.Name == "NavItem_Gmail")
            {
                mw.BackButton.IsEnabled = WebViews.GmailWebView.CanGoBack;
                mw.ForwardButton.IsEnabled = WebViews.GmailWebView.CanGoForward;
            }
        }

        private void NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
        {
            //If link isn't a Outlook URL, it opens it in an external browser
            if (args.Uri.Contains("https://mail.google.com") || args.Uri.Contains("https://www.gmail.com"))
            {
                args.Handled = true;
                WebViews.GmailWebView.Source = new Uri(args.Uri);
            }
            else
            {
                args.Handled = true;
                Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true });
            }
        }
    }
}
