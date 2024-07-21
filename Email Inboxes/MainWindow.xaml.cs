using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using WinRT;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Email_Inboxes;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Windows.UI.Popups;
using Windows.UI.ApplicationSettings;
using Windows.Media.Playback;
using System.Security.Cryptography;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using System.Reflection.Metadata;
using Windows.UI.WindowManagement;
using Microsoft.UI.Xaml.Media.Animation;
using CommunityToolkit.WinUI;
using System.Threading.Tasks;
using Email_Inboxes.Services;
using Microsoft.UI.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainWindow : Window
    {
        public static class WebViews
        {
            public static WebView2 OutlookWebView
            {
                get { return outlookWebView; }
                set { outlookWebView = value; }
            }
            private static WebView2 outlookWebView;

            public static WebView2 GmailWebView
            {
                get { return gmailWebView; } 
                set {  gmailWebView = value; }
            }
            private static WebView2 gmailWebView;

            public static WebView2 YahooWebView
            {
                get { return yahooWebView; }
                set { yahooWebView = value; }
            }
            private static WebView2 yahooWebView;

            public static WebView2 IcloudWebView
            {
                get { return iCloudWebView; }
                set { iCloudWebView = value; }
            }
            private static WebView2 iCloudWebView;

            public static WebView2 ProtonWebView
            {
                get { return protonWebView; }
                set { protonWebView = value; }
            }
            private static WebView2 protonWebView;

            public static WebView2 ToDoWebView
            {
                get { return toDoWebView; }
                set { toDoWebView = value; }
            }
            private static WebView2 toDoWebView;

            public static WebView2 CalendarWebView
            {
                get { return calendarWebView; }
                set { calendarWebView = value; }
            }
            private static WebView2 calendarWebView;
        }

        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private Microsoft.UI.Windowing.AppWindow m_AppWindow;

        private Microsoft.UI.Windowing.AppWindow GetAppWindowForCurrentWindow ()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return Microsoft.UI.Windowing.AppWindow.GetFromWindowId(wndId);
        }

        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            m_AppWindow = GetAppWindowForCurrentWindow();
            Title = $"Email Inboxes";
            SetTitleBar(AppTitleBar);
            m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            m_AppWindow.SetIcon("Mail.ico");

            //Sets the backdrop of the window based on the user's preference
            SystemBackdrop backdropToSet = null;
            switch ((string)localSettings.Values[App.Settings.Backdrop])
            {
                case "Mica":
                    backdropToSet = new MicaBackdrop() { Kind = MicaKind.Base };
                    break;
                case "Mica Alt":
                    backdropToSet = new MicaBackdrop() { Kind = MicaKind.BaseAlt };
                    break;
                case "Acrylic":
                    backdropToSet = new DesktopAcrylicBackdrop();
                    break;
            }
            mainwindow.SystemBackdrop = backdropToSet;

            //Changes the pane display mode depending on the user's setting for PaneDisplayMode
            var displayModeToSet = NavigationViewPaneDisplayMode.Left;
            switch (localSettings.Values[App.Settings.PaneDisplayMode])
            {
                case "Auto":
                    nvSample.Margin = new Thickness() { Top = 0 };
                    AppTitleBar.Margin = new Thickness() { Left = 51 };
                    displayModeToSet = NavigationViewPaneDisplayMode.Auto;
                    break;
                case "Left":
                    nvSample.Margin = new Thickness() { Top = 0 };
                    AppTitleBar.Margin = new Thickness() { Left = 51 };
                    displayModeToSet = NavigationViewPaneDisplayMode.Left;
                    break;
                case "Compact":
                    nvSample.Margin = new Thickness() { Top = 0 };
                    AppTitleBar.Margin = new Thickness() { Left = 51 };
                    displayModeToSet = NavigationViewPaneDisplayMode.LeftCompact;
                    break;
                case "Minimal":
                    nvSample.Margin = new Thickness() { Top = 0 };
                    AppTitleBar.Margin = new Thickness() { Left = 51 };
                    displayModeToSet = NavigationViewPaneDisplayMode.LeftMinimal;
                    break;
                case "Top":
                    nvSample.Margin = new Thickness() { Top = 48 };
                    AppTitleBar.Margin = new Thickness() { Left = 16 };
                    displayModeToSet = NavigationViewPaneDisplayMode.Top;
                    break;
            }
            nvSample.PaneDisplayMode = displayModeToSet;

            //Hides or shows the related nvSample NavItem depending on the user's settings
            if ((bool)localSettings.Values[App.Settings.OutlookEnabled])
            {
                string outlookAppType = (string)localSettings.Values[App.Settings.OutlookAppType];
                NavItem_Outlook.Visibility = (outlookAppType == "Website" || outlookAppType == "Business website") ? Visibility.Visible : Visibility.Collapsed;
            }
            else NavItem_Outlook.Visibility = Visibility.Collapsed;

            NavItem_Gmail.Visibility = (bool)localSettings.Values[App.Settings.GmailEnabled] ? Visibility.Visible : Visibility.Collapsed;

            NavItem_iCloud.Visibility = (bool)localSettings.Values[App.Settings.iCloudEnabled] ? Visibility.Visible : Visibility.Collapsed;

            NavItem_Proton.Visibility = (bool)localSettings.Values[App.Settings.ProtonEnabled] ? Visibility.Visible : Visibility.Collapsed;

            NavItem_Home.Visibility = (bool)localSettings.Values[App.Settings.HomeEnabled] ? Visibility.Visible : Visibility.Collapsed;

            NavItem_Yahoo.Visibility = (bool)localSettings.Values[App.Settings.YahooEnabled] ? Visibility.Visible : Visibility.Collapsed;

            //Changes the size of the interactable area in the app title bar
            AppTitleBar.Loaded += AppTitleBar_Loaded;
            AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        }

        private void SetRegionsForCustomTitleBar()
        {
            if (CommandBar.Visibility == Visibility.Visible)
            {
            // Specify the interactive regions of the title bar.

            double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

            GeneralTransform transform = CommandBar.TransformToVisual(null);
            Rect bounds = transform.TransformBounds(new Rect(0, 0,
                                                             CommandBar.ActualWidth,
                                                             CommandBar.ActualHeight));
            Windows.Graphics.RectInt32 commandBarRect = GetRect(bounds, scaleAdjustment);

            var rectArray = new Windows.Graphics.RectInt32[] { commandBarRect };

            InputNonClientPointerSource nonClientInputSrc =
                InputNonClientPointerSource.GetForWindowId(this.AppWindow.Id);
            nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, rectArray);
            }
            else
            {
                InputNonClientPointerSource nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(this.AppWindow.Id);
                nonClientInputSrc.ClearRegionRects(NonClientRegionKind.Passthrough);
            }
        }

        private Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
        {
            return new Windows.Graphics.RectInt32(
                _X: (int)Math.Round(bounds.X * scale),
                _Y: (int)Math.Round(bounds.Y * scale),
                _Width: (int)Math.Round(bounds.Width * scale),
                _Height: (int)Math.Round(bounds.Height * scale)
            );
        }

        private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
        {
            //Hides the CommandBar if it disabled
            if (!(bool)localSettings.Values[App.Settings.CommandBarEnabled])
            {
                CommandBar.Visibility = Visibility.Collapsed;
                SetRegionsForCustomTitleBar();
            }

            //Changes the selected item of the nvSample NavigationView to the user's selected startup page
            NavigationViewItem navItem_StartupPage = NavItem_Home;
            switch ((string)localSettings.Values[App.Settings.StartupPage])
            {
                case "Home":
                    navItem_StartupPage = NavItem_Home;
                    break;
                case "Outlook":
                    navItem_StartupPage = NavItem_Outlook;
                    break;
                case "Gmail":
                    navItem_StartupPage = NavItem_Gmail;
                    break;
                case "Yahoo Mail":
                    navItem_StartupPage = NavItem_Yahoo;
                    break;
                case "iCloud Mail":
                    navItem_StartupPage = NavItem_iCloud;
                    break;
                case "Proton Mail":
                    navItem_StartupPage = NavItem_Proton;
                    break;
            }

            nvSample.SelectedItem = navItem_StartupPage;

            //Makes the forward and back buttons disabled
            ForwardButton.IsEnabled = BackButton.IsEnabled = false;
        }

        private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ExtendsContentIntoTitleBar == true)
            {
                // Update interactive regions if the size of the window changes.
                SetRegionsForCustomTitleBar();
            }
        }

        private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //Changes the content of the contentFrame related to the users selection
            Type pageType = typeof(Home);

            var selectedItem = (NavigationViewItem)args.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Home":
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Visible && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Collapsed;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(Home);
                    break;
                case "NavItem_Gmail":
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(Gmail);
                    BackButton.IsEnabled = WebViews.GmailWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.GmailWebView.CanGoForward;
                    break;
                case "NavItem_iCloud":
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(iCloud);
                    BackButton.IsEnabled = WebViews.IcloudWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.IcloudWebView.CanGoForward;
                    break;
                case "NavItem_Proton":
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(Proton);
                    BackButton.IsEnabled = WebViews.ProtonWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.ProtonWebView.CanGoForward;
                    break;
                case "NavItem_Outlook":
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(Outlook);
                    contentFrame.Navigate(typeof(Outlook), null, null);
                    BackButton.IsEnabled = WebViews.OutlookWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.OutlookWebView.CanGoForward;
                    break;
                case "NavItem_Yahoo":
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(Yahoo);
                    BackButton.IsEnabled = WebViews.YahooWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.YahooWebView.CanGoForward;
                    break;
                case "SettingsItem":
                    if (CommandBar.Visibility == Visibility.Visible && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Collapsed;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(Settings);
                    break;
                default:
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Visible && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Collapsed;
                        SetRegionsForCustomTitleBar();
                    }
                    pageType = typeof(Home);
                    break;
            }

            _ = contentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromBottom });
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    string outlookWebViewSource = "https://https://outlook.live.com";
                    if ((string)localSettings.Values[App.Settings.OutlookAppType] == "Website")
                        outlookWebViewSource = "https://outlook.live.com";
                    else
                        outlookWebViewSource = "https://outlook.office.com";
                    WebViews.OutlookWebView.Source = new Uri(outlookWebViewSource);
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.Source = new Uri("https://mail.google.com/mail/u/0/");
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.Source = new Uri("https://mail.yahoo.com");
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.Source = new Uri("https://www.icloud.com/mail");
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.Source = new Uri("https://mail.proton.me/");
                    break;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.Reload();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.Reload();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.Reload();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.Reload();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.Reload();
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.GoBack();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.GoBack();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.GoBack();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.GoBack();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.GoBack();
                    break;
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.GoForward();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.GoForward();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.GoForward();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.GoForward();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.GoForward();
                    break;
            }
        }

        private void DevToolsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
            }
        }

        private void TaskManagerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
            }
        }
    }
}
