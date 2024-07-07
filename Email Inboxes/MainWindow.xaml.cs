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
            public static Page homePage
            {
                get { return HomePage; }
            }

            private static Page HomePage = new Home();

            public static Page gmailPage
            {
                get { return GmailPage; }
            }

            private static Page GmailPage = new Gmail();

            public static Page iCloudPage
            {
                get { return IcloudPage; }
            }

            private static Page IcloudPage = new iCloud();

            public static Page outlookPage
            {
                get { return OutlookPage; }
            }

            private static Page OutlookPage = new Outlook();

            public static Page protonPage
            {
                get { return ProtonPage; }
            }

            private static Page ProtonPage = new Proton();

            public static Page settingsPage
            {
                get { return SettingsPage; }
            }

            private static Page SettingsPage = new Settings();
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

            //Changes the selected item of the nvSample NavigationView to the user's selected startup page
            string NavItem_StartupPage = "NavItem_Home";
            switch ((string)localSettings.Values[App.Settings.StartupPage])
            {
                case "Home":
                    NavItem_StartupPage = "NavItem_Home";
                    break;
                case "Outlook":
                    NavItem_StartupPage = "NavItem_Outlook";
                    break;
                case "Gmail":
                    NavItem_StartupPage = "NavItem_Gmail";
                    break;
                case "iCloud Mail":
                    NavItem_StartupPage = "NavItem_iCloud";
                    break;
                case "Proton Mail":
                    NavItem_StartupPage = "NavItem_Proton";
                    break;
            }

            var item = nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == NavItem_StartupPage);
            nvSample.SelectedItem = item;

            //Hides or shows the related nvSample NavItem depending on the user's settings
            if ((bool)localSettings.Values[App.Settings.OutlookEnabled])
                NavItem_Outlook.Visibility = ((string)localSettings.Values[App.Settings.OutlookAppType] == "Website") ? Visibility.Visible : Visibility.Collapsed;
            else NavItem_Outlook.Visibility = Visibility.Collapsed;

            NavItem_Gmail.Visibility = (bool)localSettings.Values[App.Settings.GmailEnabled] ? Visibility.Visible : Visibility.Collapsed;

            NavItem_iCloud.Visibility = (bool)localSettings.Values[App.Settings.iCloudEnabled] ? Visibility.Visible : Visibility.Collapsed;

            NavItem_Proton.Visibility = (bool)localSettings.Values[App.Settings.ProtonEnabled] ? Visibility.Visible : Visibility.Collapsed;

            NavItem_Home.Visibility = (bool)localSettings.Values[App.Settings.HomeEnabled] ? Visibility.Visible : Visibility.Collapsed;
        }

        private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //Changes the content of the contentFrame related to the users selection
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                navOptions.IsNavigationStackEnabled = false;
            }
            Type pageType = typeof(Home);

            var selectedItem = (NavigationViewItem)args.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Home":
                    App.Settings.SettingsChangable = false;
                    pageType = typeof(Home);
                    break;
                case "NavItem_Gmail":
                    App.Settings.SettingsChangable = false;
                    pageType = typeof(Gmail);
                    break;
                case "NavItem_iCloud":
                    App.Settings.SettingsChangable = false;
                    pageType = typeof(iCloud);
                    break;
                case "NavItem_Proton":
                    App.Settings.SettingsChangable = false;
                    pageType = typeof(Proton);
                    break;
                case "NavItem_Outlook":
                    App.Settings.SettingsChangable = false;
                    pageType = typeof(Outlook);
                    break;
                case "SettingsItem":
                    pageType = typeof(Settings);
                    break;
                default:
                    App.Settings.SettingsChangable = false;
                    pageType = typeof(Home);
                    break;
            }
            _ = contentFrame.Navigate(pageType);
        }
    }
}
