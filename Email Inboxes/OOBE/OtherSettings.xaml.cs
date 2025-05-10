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
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes.First_Boot_Window
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OtherSettings : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public OtherSettings()
        {
            this.InitializeComponent();

            outlookButton.Visibility = FirstBootWindow.SettingsCache.OutlookEnabled ? Visibility.Visible : Visibility.Collapsed;
            gmailButton.Visibility = FirstBootWindow.SettingsCache.GmailEnabled ? Visibility.Visible : Visibility.Collapsed;
            yahooButton.Visibility = FirstBootWindow.SettingsCache.YahooEnabled ? Visibility.Visible : Visibility.Collapsed;
            iCloudButton.Visibility = FirstBootWindow.SettingsCache.IcloudEnabled ? Visibility.Visible : Visibility.Collapsed;
            protonButton.Visibility = FirstBootWindow.SettingsCache.ProtonEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;

            localSettings.Values[App.Settings.FirstBootScreenPassed] = true;

            //localSettings.Values[App.Settings.Backdrop] = FirstBootWindow.SettingsCache.Backdrop;
            localSettings.Values[App.Settings.CalendarServiceName] = FirstBootWindow.SettingsCache.CalendarServiceName;
            localSettings.Values[App.Settings.CalendarServiceUrl] = FirstBootWindow.SettingsCache.CalendarServiceUrl;
            localSettings.Values[App.Settings.CommandBarEnabled] = FirstBootWindow.SettingsCache.CommandBarEnabled;
            localSettings.Values[App.Settings.GmailEnabled] = FirstBootWindow.SettingsCache.GmailEnabled;
            localSettings.Values[App.Settings.HomeEnabled] = FirstBootWindow.SettingsCache.HomeEnabled;
            localSettings.Values[App.Settings.iCloudEnabled] = FirstBootWindow.SettingsCache.IcloudEnabled;
            //localSettings.Values[App.Settings.OutlookAppType] = FirstBootWindow.SettingsCache.OutlookAppType;
            localSettings.Values[App.Settings.OutlookEnabled] = FirstBootWindow.SettingsCache.OutlookEnabled;
            localSettings.Values[App.Settings.OutlookExePath] = FirstBootWindow.SettingsCache.OutlookExePath;
            //localSettings.Values[App.Settings.PaneDisplayMode] = FirstBootWindow.SettingsCache.PaneDisplayMode;
            localSettings.Values[App.Settings.ProtonEnabled] = FirstBootWindow.SettingsCache.ProtonEnabled;
            //localSettings.Values[App.Settings.StartupPage] = FirstBootWindow.SettingsCache.StartupPage;
            localSettings.Values[App.Settings.ToDoServiceName] = FirstBootWindow.SettingsCache.ToDoServiceName;
            localSettings.Values[App.Settings.ToDoServiceUrl] = FirstBootWindow.SettingsCache.ToDoServiceUrl;
            localSettings.Values[App.Settings.YahooEnabled] = FirstBootWindow.SettingsCache.YahooEnabled;

            app.m_window = new MainWindow();
            app.m_window.Activate();

            app.firstBootWindow.Close();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(StartupPage), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
        }
    }
}
