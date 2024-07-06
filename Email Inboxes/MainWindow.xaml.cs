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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{

    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainWindow : Window
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private AppWindow m_AppWindow;

        private AppWindow GetAppWindowForCurrentWindow ()
        {
        IntPtr hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
        }

        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            m_AppWindow = GetAppWindowForCurrentWindow();
            Title = $"Email Inboxes";
            this.Activated += MainWindow_Activated;
            SetTitleBar(AppTitleBar);
            m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

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

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            //Code to set the icon of the window to the app logo in ICO form
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.SetIcon("Mail.ico");
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
                    pageType = typeof(Home);
                    break;
                case "NavItem_Gmail":
                    pageType = typeof(Gmail);
                    break;
                case "NavItem_iCloud":
                    pageType = typeof(iCloud);
                    break;
                case "NavItem_Proton":
                    pageType = typeof(Proton);
                    break;
                case "NavItem_Outlook":
                    pageType = typeof(Outlook);
                    break;
                default:
                    pageType = typeof (Settings);
                    break;
            }
            _ = contentFrame.Navigate(pageType);
        }
    }
}
