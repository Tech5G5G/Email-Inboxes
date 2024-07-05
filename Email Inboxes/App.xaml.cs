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
using Windows.ApplicationModel.Appointments;
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
            public const string YahooEnabled = "YahooEnabled";
            public const string OutlookAppType = "OutlookAppType";
            public const string ToDoServiceName = "ToDoServiceName";
            public const string CalendarServiceName = "CalendarServiceName";
            public const string ToDoServiceUrl = "ToDoServiceUrl";
            public const string CalendarServiceUrl = "CalendarServiceUrl";
            public const string OutlookExePath = "OutlookExePath";
            public const string VersionNumber = "VersionNumber";
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
            //Settings checkers to check if settings exist
            if (!localSettings.Values.ContainsKey(Settings.HomeEnabled))
                localSettings.Values[Settings.HomeEnabled] = true;

            if (!localSettings.Values.ContainsKey(Settings.iCloudEnabled))
                localSettings.Values[Settings.iCloudEnabled] = true;

            if (!localSettings.Values.ContainsKey(Settings.GmailEnabled))
                localSettings.Values[Settings.GmailEnabled] = true;

            if (!localSettings.Values.ContainsKey(Settings.ProtonEnabled))
                localSettings.Values[Settings.ProtonEnabled] = true;

            if (!localSettings.Values.ContainsKey(Settings.OutlookEnabled))
                localSettings.Values[Settings.OutlookEnabled] = true;

            if (!localSettings.Values.ContainsKey(Settings.OutlookAppType))
                localSettings.Values[Settings.OutlookAppType] = "Website";

            if (!localSettings.Values.ContainsKey(Settings.ToDoServiceUrl))
                localSettings.Values[Settings.ToDoServiceUrl] = "disabled";

            if (!localSettings.Values.ContainsKey(Settings.ToDoServiceName))
                localSettings.Values[Settings.ToDoServiceName] = "Disabled";

            if (!localSettings.Values.ContainsKey(Settings.CalendarServiceUrl))
                localSettings.Values[Settings.CalendarServiceUrl] = "disabled";

            if (!localSettings.Values.ContainsKey(Settings.CalendarServiceName))
                localSettings.Values[Settings.CalendarServiceName] = "Disabled";

            if (!localSettings.Values.ContainsKey(Settings.StartupPage))
                localSettings.Values[Settings.StartupPage] = "Home";

            if (!localSettings.Values.ContainsKey(Settings.OutlookExePath))
                localSettings.Values[Settings.OutlookExePath] = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";

            //Backwards compatibility code that updates the values of Home, iCloud, Gmail, Proton, & Outlook Enabled settings from strings to booleans
            if (!localSettings.Values.ContainsKey(Settings.VersionNumber))
            {
                //object homeEnabledSetting = localSettings.Values[Settings.HomeEnabled];
                //string homeEnabledString = homeEnabledSetting as string;
                //bool homeEnabled = true;

                //if (string.IsNullOrWhiteSpace(homeEnabledString))
                //{
                //    if(!bool.TryParse(homeEnabledString, out homeEnabled))
                //    {
                //        homeEnabled = true;
                //    }
                //}
                localSettings.Values[Settings.HomeEnabled] = (string)localSettings.Values[Settings.HomeEnabled] == "True";
                localSettings.Values[Settings.iCloudEnabled] = (string)localSettings.Values[Settings.iCloudEnabled] == "True";
                localSettings.Values[Settings.GmailEnabled] = (string)localSettings.Values[Settings.GmailEnabled] == "True";
                localSettings.Values[Settings.ProtonEnabled] = (string)localSettings.Values[Settings.ProtonEnabled] == "True";
                localSettings.Values[Settings.OutlookEnabled] = (string)localSettings.Values[Settings.OutlookEnabled] == "True";
            }

            //Creates setting to prevent backwards compatibility code from running in the future
            if (!((string)localSettings.Values[Settings.VersionNumber] == "1.3"))
                localSettings.Values[Settings.VersionNumber] = "1.3";

            m_window = new MainWindow();
            m_window.Activate();
        }

        public Window m_window;

        public Frame contentFrame { get; private set; }
    }
}
