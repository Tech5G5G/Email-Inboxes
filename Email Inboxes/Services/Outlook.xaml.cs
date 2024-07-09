using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    }
}
