using Email_Inboxes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.TargetedContent;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        //Class to store settings so they can be always spelled correctly
        public static class Settings
        {
            public const string StartupPage = "StartupPage";
            public const string HomeEnabled = "HomeEnabled";
            public const string OutlookEnabled = "OutlookEnabled";
            public const string ProtonEnabled = "ProtonEnabled";
            public const string iCloudEnabled = "iCloudEnabled";
            public const string GmailEnabled = "GmailEnabled";
            public const string OutlookAppType = "OutlookAppType";
            public const string ToDoServiceName = "ToDoServiceName";
            public const string CalendarServiceName = "CalendarServiceName";
            public const string ToDoServiceUrl = "ToDoServiceUrl";
            public const string CalendarServiceUrl = "CalendarServiceUrl";
            public const string OutlookExePath = "OutlookExePath";
        }

        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();

            //Settings checkers to check if settings exist
            string StartupPage = "Home";
            if (localSettings.Values.ContainsKey(Settings.StartupPage))
            {
                StartupPage = localSettings.Values[Settings.StartupPage].ToString();
            }
            else
            {
                localSettings.Values[Settings.StartupPage] = "Home";
                StartupPage = localSettings.Values[Settings.StartupPage].ToString();
            }

            string ProtonEnabled = "True";
            if (localSettings.Values.ContainsKey(Settings.ProtonEnabled))
            {
                ProtonEnabled = localSettings.Values[Settings.ProtonEnabled].ToString();
            }
            else
            {
                localSettings.Values[Settings.ProtonEnabled] = "True";
                ProtonEnabled = localSettings.Values[Settings.ProtonEnabled].ToString();
            }

            bool HomeEnabled = true;
            if (!localSettings.Values.ContainsKey(Settings.HomeEnabled))
            {
                localSettings.Values[Settings.HomeEnabled] = HomeEnabled;
            }
            else
            {
                HomeEnabled = (bool)localSettings.Values[Settings.HomeEnabled];
            }

            string iCloudEnabled = "True";
            if (localSettings.Values.ContainsKey(Settings.iCloudEnabled))
            {
                iCloudEnabled = localSettings.Values[Settings.iCloudEnabled].ToString();
            }
            else
            {
                localSettings.Values[Settings.iCloudEnabled] = "True";
                iCloudEnabled = localSettings.Values[Settings.iCloudEnabled].ToString();
            }

            string GmailEnabled = "True";
            if (localSettings.Values.ContainsKey(Settings.GmailEnabled))
            {
                GmailEnabled = localSettings.Values[Settings.GmailEnabled].ToString();
            }
            else
            {
                localSettings.Values[Settings.GmailEnabled] = "True";
                GmailEnabled = localSettings.Values[Settings.GmailEnabled].ToString();
            }

            string OutlookEnabled = "True";
            if (localSettings.Values.ContainsKey(Settings.OutlookEnabled))
            {
                OutlookEnabled = localSettings.Values[Settings.OutlookEnabled].ToString();
            }
            else
            {
                localSettings.Values[Settings.OutlookEnabled] = "True";
                OutlookEnabled = localSettings.Values[Settings.OutlookEnabled].ToString();
            }

            string OutlookAppType = "Website";
            if (localSettings.Values.ContainsKey(Settings.OutlookAppType))
            {
                OutlookAppType = localSettings.Values[Settings.OutlookAppType].ToString();
            }
            else
            {
                localSettings.Values[Settings.OutlookAppType] = "Website";
                OutlookAppType = localSettings.Values[Settings.OutlookAppType].ToString();
            }

            string ToDoServiceUrl = "disabled";
            if (localSettings.Values.ContainsKey(Settings.ToDoServiceUrl))
            {
                ToDoServiceUrl = localSettings.Values[Settings.ToDoServiceUrl].ToString();
            }
            else
            {
                localSettings.Values[Settings.ToDoServiceUrl] = "disabled";
            }

            string CalendarServiceUrl = "disabled";
            if (localSettings.Values.ContainsKey(Settings.CalendarServiceUrl))
            {
                CalendarServiceUrl = localSettings.Values[Settings.CalendarServiceUrl].ToString();
            }
            else
            {
                localSettings.Values[Settings.CalendarServiceUrl] = "disabled";
            }

            string CalendarServiceName = "None";
            if (localSettings.Values.ContainsKey(Settings.CalendarServiceName))
            {
                CalendarServiceName = this.localSettings.Values[Settings.CalendarServiceName].ToString();
            }
            else
            {
                localSettings.Values[Settings.CalendarServiceName] = "None";
                CalendarServiceName = this.localSettings.Values[Settings.CalendarServiceName].ToString();
            }

            string OutlookExePath = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";
            if (localSettings.Values.ContainsKey(Settings.OutlookExePath))
            {
                OutlookExePath = localSettings.Values[Settings.OutlookExePath].ToString();
            }
            else
            {
                localSettings.Values[Settings.OutlookExePath] = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";
                OutlookExePath = localSettings.Values[Settings.OutlookExePath].ToString();
            }

            string ToDoServiceName = "None";
            if (localSettings.Values.ContainsKey(Settings.ToDoServiceName))
            {
                ToDoServiceName = this.localSettings.Values[Settings.ToDoServiceName].ToString();
            }
            else
            {
                localSettings.Values[Settings.ToDoServiceName] = "None";
                ToDoServiceName = this.localSettings.Values[Settings.ToDoServiceName].ToString();
            }

            string NavItem_StartupPage = "NavItem_Home";
            switch (StartupPage)
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

            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == NavItem_StartupPage);
            mw.nvSample.SelectedItem = item;
        }

        public Window m_window;

        public Frame contentFrame { get; private set; }
    }
}
