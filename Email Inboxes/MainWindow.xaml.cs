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

        private WindowsSystemDispatcherQueueHelper? _wsdqHelper;

        // See separate sample below for implementation
        private DesktopAcrylicController? _acrylicController;

        private MicaController? _micaController;

        private SystemBackdropConfiguration? _configurationSource;

        public MainWindow()
        {
            this.InitializeComponent();
            var result = TrySetMicaBackdrop();
            ExtendsContentIntoTitleBar = true;
            m_AppWindow = GetAppWindowForCurrentWindow();
            Title = $"Email Inboxes";
            this.Activated += MainWindow_Activated;
            SetTitleBar(AppTitleBar);
            m_AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

            string OutlookEnabled = "True";
            if (localSettings.Values.ContainsKey("OutlookEnabled"))
            {
                OutlookEnabled = localSettings.Values["OutlookEnabled"].ToString();
            }
            else
            {
                localSettings.Values["OutlookEnabled"] = "True";
                OutlookEnabled = localSettings.Values["OutlookEnabled"].ToString();
            }

            string OutlookAppType = "Website";
            if (localSettings.Values.ContainsKey("OutlookAppType"))
            {
                OutlookAppType = localSettings.Values["OutlookAppType"].ToString();
            }
            else
            {
                localSettings.Values["OutlookAppType"] = "Website";
                OutlookAppType = localSettings.Values["OutlookAppType"].ToString();
            }

            if (OutlookEnabled is "True")
            {
                if (OutlookAppType == "Website")
                {
                    NavItem_Outlook.Visibility = Visibility.Visible;
                }
                else
                {
                    NavItem_Outlook.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                NavItem_Outlook.Visibility = Visibility.Collapsed;
            }

            string GmailEnabled = "True";
            if (localSettings.Values.ContainsKey("GmailEnabled"))
            {
                GmailEnabled = localSettings.Values["GmailEnabled"].ToString();
            }
            else
            {
                localSettings.Values["GmailEnabled"] = "True";
                GmailEnabled = localSettings.Values["GmailEnabled"].ToString();
            }

            if (GmailEnabled is "True")
            {
                NavItem_Gmail.Visibility = Visibility.Visible;
            }
            else
            {
                NavItem_Gmail.Visibility = Visibility.Collapsed;
            }

            string iCloudEnabled = "True";
            if (localSettings.Values.ContainsKey("iCloudEnabled"))
            {
                iCloudEnabled = localSettings.Values["iCloudEnabled"].ToString();
            }
            else
            {
                localSettings.Values["iCloudEnabled"] = "True";
                iCloudEnabled = localSettings.Values["iCloudEnabled"].ToString();
            }

            if (iCloudEnabled is "True")
            {
                NavItem_iCloud.Visibility = Visibility.Visible;
            }
            else
            {
                NavItem_iCloud.Visibility = Visibility.Collapsed;
            }

            string ProtonEnabled = "True";
            if (localSettings.Values.ContainsKey("ProtonEnabled"))
            {
                ProtonEnabled = localSettings.Values["ProtonEnabled"].ToString();
            }
            else
            {
                localSettings.Values["ProtonEnabled"] = "True";
                ProtonEnabled = localSettings.Values["ProtonEnabled"].ToString();
            }

            if (ProtonEnabled is "True")
            {
                NavItem_Proton.Visibility = Visibility.Visible;
            }
            else
            {
                NavItem_Proton.Visibility = Visibility.Collapsed;
            }

            string HomeEnabled = "True";
            if (localSettings.Values.ContainsKey("HomeEnabled"))
            {
                HomeEnabled = localSettings.Values["HomeEnabled"].ToString();
            }
            else
            {
                localSettings.Values["HomeEnabled"] = "True";
                HomeEnabled = localSettings.Values["HomeEnabled"].ToString();
            }

            if (HomeEnabled is "True")
            {
                NavItem_Home.Visibility = Visibility.Visible;
            }
            else
            {
                NavItem_Home.Visibility = Visibility.Collapsed;
            }
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            appWindow.SetIcon("Mail.ico");
        }

        private bool TrySetMicaBackdrop()
        {
            if (MicaController.IsSupported() is true)
            {
                _wsdqHelper = new WindowsSystemDispatcherQueueHelper();
                _wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

                // Hooking up the policy object
                _configurationSource = new SystemBackdropConfiguration();
                this.Activated += Window_Activated;
                this.Closed += Window_Closed;
                ((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;

                // Initial configuration state.
                _configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                _micaController = new MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                _micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                _micaController.SetSystemBackdropConfiguration(_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (_configurationSource is not null)
            {
                _configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
            }
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
            // use this closed window.
            if (_acrylicController is not null)
            {
                _acrylicController.Dispose();
                _acrylicController = null;
            }

            this.Activated -= Window_Activated;
            _configurationSource = null;
        }

        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            if (_configurationSource is not null)
            {
                SetConfigurationSourceTheme();
            }
        }

        private void SetConfigurationSourceTheme()
        {
            if (_configurationSource is not null)
            {
                switch (((FrameworkElement)this.Content).ActualTheme)
                {
                    case ElementTheme.Dark:
                        _configurationSource.Theme = SystemBackdropTheme.Dark;
                        break;
                    case ElementTheme.Light:
                        _configurationSource.Theme = SystemBackdropTheme.Light;
                        break;
                    case ElementTheme.Default:
                        _configurationSource.Theme = SystemBackdropTheme.Default;
                        break;
                }
            }
        }

        private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
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
