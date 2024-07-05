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

            //Shows or hides the WebView that shows the user selected to do app/service
            string ToDoServiceUrl = (string)localSettings.Values[App.Settings.ToDoServiceUrl];
            if (ToDoServiceUrl == "disabled") HomeWebView.Visibility = Visibility.Collapsed;
            else HomeWebView.Source = new Uri(ToDoServiceUrl);

            //Shows or hides the WebView that shows the user selected calendar app/service
            string CalendarServiceUrl = (string)localSettings.Values[App.Settings.CalendarServiceUrl];
            if (CalendarServiceUrl == "disabled") HomeWebView.Visibility = Visibility.Collapsed;
            else HomeWebView.Source = new Uri(CalendarServiceUrl);

            //Shows or hides the card of the related service depending on the user's settings
            if ((bool)localSettings.Values[App.Settings.OutlookEnabled])
            {
                OutlookPageButton.Visibility = (string)localSettings.Values[App.Settings.OutlookAppType] == "Website" ? Visibility.Visible : Visibility.Collapsed;
                OutlookAppButton.Visibility = (string)localSettings.Values[App.Settings.OutlookAppType] == "Website" ? Visibility.Collapsed : Visibility.Visible;
            }
            else OutlookPageButton.Visibility = OutlookAppButton.Visibility = Visibility.Collapsed;

            GmailButton.Visibility = (bool)localSettings.Values[App.Settings.GmailEnabled] ? Visibility.Visible : Visibility.Collapsed;

            iCloudButton.Visibility = (bool)localSettings.Values[App.Settings.iCloudEnabled] ? Visibility.Visible : Visibility.Collapsed;

            ProtonButton.Visibility = (bool)localSettings.Values[App.Settings.ProtonEnabled] ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SettingsCard_Click(object sender, RoutedEventArgs e)
        {
            Process.Start((string)localSettings.Values[App.Settings.OutlookExePath]);
        }

        //All below changes the selected nvSample item with a different animation
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

        private void YahooNavigate(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Yahoo");
            mw.nvSample.SelectedItem = item;
            Frame.Navigate(typeof(Outlook), contentFrame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
    }
}
