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
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using Email_Inboxes.First_Boot_Window;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstBootWindow : Window
    {
        public static class SettingsCache
        {
            public static string StartupPage
            {
                get { return StartupPage; }
                set { StartupPage = value; }
            }
            private static string startupPage;

            public static bool HomeEnabled
            {
                get { return homeEnabled; }
                set { homeEnabled = value; }
            }
            private static bool homeEnabled;

            public static bool OutlookEnabled
            {
                get { return outlookEnabled; }
                set { outlookEnabled = value; }
            }
            private static bool outlookEnabled;

            public static bool ProtonEnabled
            {
                get { return protonEnabled; }
                set { protonEnabled = value; }
            }
            private static bool protonEnabled;

            public static bool IcloudEnabled
            {
                get { return iCloudEnabled; }
                set { iCloudEnabled = value; }
            }
            private static bool iCloudEnabled;

            public static bool GmailEnabled
            {
                get { return gmailEnabled; }
                set { gmailEnabled = value; }
            }
            private static bool gmailEnabled;

            public static bool YahooEnabled
            {
                get { return yahooEnabled; }
                set { yahooEnabled = value; }
            }
            private static bool yahooEnabled;

            public static bool CommandBarEnabled
            {
                get { return commandBarEnabled; }
                set { commandBarEnabled = value; }
            }
            private static bool commandBarEnabled;

            public static string OutlookAppType
            {
                get { return outlookAppType; }
                set { outlookAppType = value; }
            }
            private static string outlookAppType;

            public static string ToDoServiceName
            {
                get { return toDoServiceName; }
                set { toDoServiceName = value; }
            }
            private static string toDoServiceName;

            public static string CalendarServiceName
            {
                get { return calendarServiceName; }
                set { calendarServiceName = value; }
            }
            private static string calendarServiceName;

            public static string ToDoServiceUrl
            {
                get { return toDoServiceUrl; }
                set { toDoServiceUrl = value; }
            }
            private static string toDoServiceUrl;

            public static string CalendarServiceUrl
            {
                get { return calendarServiceUrl; }
                set { calendarServiceUrl = value; }
            }
            private static string calendarServiceUrl;

            public static string OutlookExePath
            {
                get { return outlookExePath; }
                set { outlookExePath = value; }
            }
            private static string outlookExePath;

            public static string Backdrop
            {
                get { return backdrop; }
                set { backdrop = value; }
            }
            private static string backdrop;

            public static string PaneDisplayMode
            {
                get { return paneDisplayMode; }
                set { paneDisplayMode = value; }
            }
            private static string paneDisplayMode;
        }
            
        OverlappedPresenter presenter;

        public FirstBootWindow()
        {
            this.InitializeComponent();

            AppWindow.SetIcon("Mail.ico");
            ExtendsContentIntoTitleBar = true;
            Title = @"Setup";

            OverlappedPresenter presenter = AppWindow.Presenter as OverlappedPresenter;
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;
            presenter.IsResizable = false;

            logoStart.Loaded += StartAnimation;
        }

        private async void StartAnimation(object sender, RoutedEventArgs e)
        {
            await Task.Delay(50);
            logoMid.Visibility = Visibility.Visible;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("logoAnimation", logoStart);
            var logoAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logoAnimation");
            logoAnimation.Completed += LogoAnimationCompleted;
            logoAnimation.TryStart(logoMid);
            logoStart.Visibility = Visibility.Collapsed;
        }

        private async void LogoAnimationCompleted(ConnectedAnimation sender, object e)
        {
            await Task.Delay(1000);
            logoEnd.Visibility = Visibility.Visible;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("logoAnimation2", logoMid);
            var logoAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logoAnimation2");
            logoAnimation.Completed += LogoAnimation2Completed;
            logoAnimation.TryStart(logoEnd);
            await Task.Delay(100); 
            logoMid.Visibility = Visibility.Collapsed;
            SetTitleBar(AppTitleBar);
        }

        private void LogoAnimation2Completed(ConnectedAnimation sender, object e)
        {
            FirstBootFrame.Visibility = Visibility.Visible;
            FirstBootFrame.Navigate(typeof(Welcome), null, null);
        }
    }
}
