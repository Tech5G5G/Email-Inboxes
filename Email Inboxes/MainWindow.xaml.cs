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
        public static class Pages
        {
            public static Page GmailPage
            {
                get { return gmailPage; }
            }
            private static Page gmailPage = new Gmail();

            public static Page IcloudPage
            {
                get { return iCloudPage; }
            }
            private static Page iCloudPage = new iCloud();

            public static Page OutlookPage { get; set; }

            public static Page ProtonPage
            {
                get { return protonPage; }
            }
            private static Page protonPage = new Proton();

            public static Page YahooPage
            {
                get { return yahooPage; }
            }
            private static Page yahooPage = new Yahoo();
        }

        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private Microsoft.UI.Windowing.AppWindow m_AppWindow;

        private Microsoft.UI.Windowing.AppWindow GetAppWindowForCurrentWindow ()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return Microsoft.UI.Windowing.AppWindow.GetFromWindowId(wndId);
        }

        private void SetRegionsForCustomTitleBar()
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
            if (ExtendsContentIntoTitleBar == true)
            {
                // Set the initial interactive regions.
                SetRegionsForCustomTitleBar();
            }
        }

        private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ExtendsContentIntoTitleBar == true)
            {
                // Update interactive regions if the size of the window changes.
                SetRegionsForCustomTitleBar();
            }
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

            //Changes the size of the interactable area in the app title bar
            AppTitleBar.Loaded += AppTitleBar_Loaded;
            AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;

            //Sets the value of OutlookPage to a new instance of Outlook
            Pages.OutlookPage = new Outlook();

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
        }

        private async void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //Navigates to a page using the contentFrame based to the users selection
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                navOptions.IsNavigationStackEnabled = false;
            }
            Type page = null;

            var selectedItem = (NavigationViewItem)args.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Home":
                    page = typeof(Home);
                    App.Settings.SettingsChangable = false;
                    break;
                case "NavItem_Gmail":
                    page = typeof(Gmail);
                    App.Settings.SettingsChangable = false;
                    break;
                case "NavItem_iCloud":
                    page = typeof(iCloud);
                    App.Settings.SettingsChangable = false;
                    break;
                case "NavItem_Proton":
                    page = typeof(Proton);
                    App.Settings.SettingsChangable = false;
                    break;
                case "NavItem_Outlook":
                    page = typeof(Outlook);
                    App.Settings.SettingsChangable = false;
                    break;
                case "NavItem_Yahoo":
                    page = typeof(Yahoo);
                    App.Settings.SettingsChangable = false;
                    break;
                case "SettingsItem":
                    page = typeof(Settings);
                    break;
                default:
                    page = typeof(Home);
                    App.Settings.SettingsChangable = false;
                    break;
            }
            _ = contentFrame.Navigate(page);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((string)nvSample.SelectedItem)
            {
                case "NavItem_Home":
                    break;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
