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
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Automation.Peers;
using static System.Net.WebRequestMethods;
using Microsoft.UI.Input;
using Windows.Storage;
using Windows.UI.Notifications;
using Microsoft.Web.WebView2.Core;
using WinRT;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public Home()
        {
            this.InitializeComponent();

            var settingsValues = localSettings.Values;

            string ToDoServiceUrl = "disabled";
            if(localSettings.Values.ContainsKey("ToDoServiceUrl"))
            {
                ToDoServiceUrl = localSettings.Values["ToDoServiceUrl"].ToString();
            }
            else
            {
                localSettings.Values["ToDoServiceUrl"] = "disabled";
            }

            switch (ToDoServiceUrl)
            {
                case "disabled":
                    HomeWebView.Visibility = Visibility.Collapsed;
                    break;
                default:
                    HomeWebView.Source = new Uri(ToDoServiceUrl);
                    break;
            }


            string CalendarServiceUrl = "disabled";
            if (localSettings.Values.ContainsKey("CalendarServiceUrl"))
            {
                CalendarServiceUrl = localSettings.Values["CalendarServiceUrl"].ToString();
            }
            else
            {
                localSettings.Values["CalendarServiceUrl"] = "disabled";
            }

            switch (CalendarServiceUrl)
            {
                case "disabled":
                    CalendarWebView.Visibility = Visibility.Collapsed;
                    break;
                default:
                    CalendarWebView.Source = new Uri(CalendarServiceUrl);
                    break;
            }

            string OutlookEnabled = localSettings.Values["OutlookEnabled"].ToString();
            string OutlookAppType = localSettings.Values["OutlookAppType"].ToString();

            if (OutlookEnabled is "True")
            {
                if (OutlookAppType == "Website")
                {
                    OutlookPageButton.Visibility = Visibility.Visible;
                    OutlookAppButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    OutlookPageButton.Visibility = Visibility.Collapsed;
                    OutlookAppButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                OutlookPageButton.Visibility = Visibility.Collapsed;
                OutlookAppButton.Visibility = Visibility.Collapsed;
            }

            string GmailEnabled = localSettings.Values["GmailEnabled"].ToString();
            if (GmailEnabled is "True")
            {
                GmailButton.Visibility = Visibility.Visible;
            }
            else
            {
                GmailButton.Visibility = Visibility.Collapsed;
            }

            string iCloudEnabled = localSettings.Values["iCloudEnabled"].ToString();
            if (iCloudEnabled is "True")
            {
                iCloudButton.Visibility = Visibility.Visible;
            }
            else
            {
                iCloudButton.Visibility = Visibility.Collapsed;
            }

            string ProtonEnabled = localSettings.Values["ProtonEnabled"].ToString();
            if (ProtonEnabled is "True")
            {
                ProtonButton.Visibility = Visibility.Visible;
            }
            else
            {
                ProtonButton.Visibility = Visibility.Collapsed;
            }
        }

        private void SettingsCard_Click(object sender, RoutedEventArgs e)
        {
            string OutlookExePath = localSettings.Values["OutlookExePath"].ToString();
            System.Diagnostics.Process.Start(OutlookExePath);
        }

        private void SettingsCard_Click_1(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Proton");
            mw.nvSample.SelectedItem = item;
            Frame.Navigate(typeof(Proton), contentFrame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void SettingsCard_Click_2(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_iCloud");
            mw.nvSample.SelectedItem = item;
            Frame.Navigate(typeof(iCloud), contentFrame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void SettingsCard_Click_3(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Gmail");
            mw.nvSample.SelectedItem = item;
            Frame.Navigate(typeof(Gmail), contentFrame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void OutlookNavigate(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Outlook");
            mw.nvSample.SelectedItem = item;
            Frame.Navigate(typeof(Outlook), contentFrame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
