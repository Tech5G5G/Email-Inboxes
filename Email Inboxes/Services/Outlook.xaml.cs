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
using Windows.ApplicationModel.Appointments.DataProvider;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Search;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using static Email_Inboxes.MainWindow;
using static System.Net.WebRequestMethods;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Outlook : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public Outlook()
        {
            this.InitializeComponent();

            //Runs method CoreWebView2Intitialized once the title suggests is done 
            OutlookWebView.CoreWebView2Initialized += CoreWebView2Initialized;

            //Sets the source of OutlookWebView depending on the user's setting
            string outlookWebViewSource = "https://https://outlook.live.com";
            if ((string)localSettings.Values[App.Settings.OutlookAppType] == "Website")
                outlookWebViewSource = "https://outlook.live.com";
            else
                outlookWebViewSource = "https://outlook.office.com";

            OutlookWebView.Source = new Uri(outlookWebViewSource);
        }

        private void CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            //Allows links to be opened in the default browser
            OutlookWebView.CoreWebView2.NewWindowRequested += NewWindowRequested;

            //Enables navigation buttons based on page navigation status
            OutlookWebView.CoreWebView2.SourceChanged += SourceChanged;
        }

        private void SourceChanged(CoreWebView2 sender, CoreWebView2SourceChangedEventArgs args)
        {
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            mw.BackButton.IsEnabled = OutlookWebView.CanGoBack;
            mw.ForwardButton.IsEnabled = OutlookWebView.CanGoForward;
        }

        private void NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
        {
            //If link isn't a Outlook URL, it opens it in an external browser
            if (args.Uri.Contains("https://outlook.live.com") || args.Uri.Contains("https://outlook.office.com"))
            {
                args.Handled = true;
                OutlookWebView.Source = new Uri(args.Uri);
            }
            else
            {
                args.Handled = true;
                Process.Start(new ProcessStartInfo(args.Uri) { UseShellExecute = true });
            }
        }
    }
}
