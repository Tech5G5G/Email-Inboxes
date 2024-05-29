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
using Windows.Storage;
using Windows.Devices.SmartCards;
using Windows.ApplicationModel.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public Settings()
        {
            this.InitializeComponent();

            string OutlookExePath = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";
            if (localSettings.Values.ContainsKey("OutlookExePath"))
            {
                OutlookExePath = localSettings.Values["OutlookExePath"].ToString();
            }
            else
            {
                localSettings.Values["OutlookExePath"] = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";
                OutlookExePath = localSettings.Values["OutlookExePath"].ToString();
            }
            ExePath.Text = OutlookExePath;

            string ToDoServiceName = "None";
            if (localSettings.Values.ContainsKey("ToDoServiceName"))
            {
                ToDoServiceName = this.localSettings.Values["ToDoServiceName"].ToString();
            }
            else
            {
                localSettings.Values["ToDoServiceName"] = "None";
                ToDoServiceName = this.localSettings.Values["ToDoServiceName"].ToString();
            }
            switch (ToDoServiceName)
            {
                case "Apple Reminders":
                    ToDoService.SelectedIndex = 3;
                    break;
                case "Todoist":
                    ToDoService.SelectedIndex = 0;
                    break;
                case "TickTick":
                    ToDoService.SelectedIndex = 1;
                    break;
                case "Microsoft To Do":
                    ToDoService.SelectedIndex = 2;
                    break;
                case "Google Tasks":
                    ToDoService.SelectedIndex = 4;
                    break;
                case "Any.do":
                    ToDoService.SelectedIndex = 5;
                    break;
                case "Disabled":
                    ToDoService.SelectedIndex = 6;
                    break;
                case "None":
                    break;
            }

            string CalendarServiceName = "None";
            if (localSettings.Values.ContainsKey("CalendarServiceName"))
            {
                CalendarServiceName = this.localSettings.Values["CalendarServiceName"].ToString();
            }
            else
            {
                localSettings.Values["CalendarServiceName"] = "None";
                CalendarServiceName = this.localSettings.Values["CalendarServiceName"].ToString();
            }
            switch (CalendarServiceName)
            {
                case "Apple Calendar":
                    CalendarService.SelectedIndex = 2;
                    break;
                case "Google Calendar":
                    CalendarService.SelectedIndex = 0;
                    break;
                case "Outlook Calendar":
                    CalendarService.SelectedIndex = 1;
                    break;
                case "Disabled":
                    CalendarService.SelectedIndex = 3;
                    break;
                case "None":
                    break;
            }

            string OutlookEnabled = localSettings.Values["OutlookEnabled"].ToString();

            if (OutlookEnabled is "True")
            {
                OutlookToggle.IsOn = true;
            }
            else
            {
                OutlookToggle.IsOn = false;
            }

            string GmailEnabled = localSettings.Values["GmailEnabled"].ToString();

            if (GmailEnabled is "True")
            {
                GmailToggle.IsOn = true;
            }
            else
            {
                GmailToggle.IsOn = false;
            }

            string iCloudEnabled = localSettings.Values["iCloudEnabled"].ToString();
            if (iCloudEnabled is "True")
            {
                iCloudToggle.IsOn = true;
            }
            else
            {
                iCloudToggle.IsOn = false;
            }

            string ProtonEnabled = localSettings.Values["ProtonEnabled"].ToString();
            if (ProtonEnabled is "True")
            {
                ProtonToggle.IsOn = true;   
            }
            else
            {
                ProtonToggle.IsOn = false;
            }

            string StartupPageSelected = localSettings.Values["StartupPage"].ToString();

            switch (StartupPageSelected)
            {
                case "Home":
                    StartupPage.SelectedIndex = 0;
                    break;
                case "Outlook":
                    StartupPage.SelectedIndex = 1;
                    break;
                case "Gmail":
                    StartupPage.SelectedIndex = 2;
                    break;
                case "iCloud Mail":
                    StartupPage.SelectedIndex = 3;
                    break;
                case "Proton Mail":
                    StartupPage.SelectedIndex = 4;
                    break;
            }

            string OutlookAppTypeSetting = localSettings.Values["OutlookAppType"].ToString();
            if (OutlookAppTypeSetting is "Website")
            {
                OutlookAppType.SelectedIndex = 0;
                ExePathCard.Visibility = Visibility.Collapsed;
            }
            else
            {
                OutlookAppType.SelectedIndex = 1;
                ExePathCard.Visibility = Visibility.Visible;
            }

            string HomeEnabled = localSettings.Values["HomeEnabled"].ToString();

            if (HomeEnabled is "True")
            {
                HomeToggle.IsOn = true;
            }
            else
            {
                HomeToggle.IsOn = false;
            }
        }

        private void ToDoService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string ToDoServiceName = (this.ToDoService.SelectedItem as ComboBoxItem).Content.ToString();

            string ToDoServiceUrl = null;

            switch (ToDoServiceName)
            {
                case "Apple Reminders":
                    ToDoServiceUrl = "https://icloud.com/reminders";
                    break;
                case "Todoist":
                    ToDoServiceUrl = "https://todoist.com";
                    break;
                case "TickTick":
                    ToDoServiceUrl = "https://ticktick.com";
                    break;
                case "Microsoft To Do":
                    ToDoServiceUrl = "https://to-do.live.com";
                    break;
                case "Google Tasks":
                    ToDoServiceUrl = "https://tasks.google.com";
                    break;
                case "Any.do":
                    ToDoServiceUrl = "https://any.do";
                    break;
                case "Disabled":
                    ToDoServiceUrl = "disabled";
                    break;
            }

            localSettings.Values["ToDoServiceName"] = ToDoServiceName;
            localSettings.Values["ToDoServiceUrl"] = ToDoServiceUrl;
        }

        private void CalendarService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string CalendarServiceName = (this.CalendarService.SelectedItem as ComboBoxItem).Content.ToString();

            string CalendarServiceUrl = null;

            switch (CalendarServiceName)
            {
                case "Apple Calendar":
                    CalendarServiceUrl = "https://www.icloud.com/calendar/";
                    break;
                case "Outlook Calendar":
                    CalendarServiceUrl = "https://outlook.live.com/calendar/";
                    break;
                case "Google Calendar":
                    CalendarServiceUrl = "https://calendar.google.com/";
                    break;
                case "Disabled":
                    CalendarServiceUrl = "disabled";
                    break;
            }

            localSettings.Values["CalendarServiceName"] = CalendarServiceName;
            localSettings.Values["CalendarServiceUrl"] = CalendarServiceUrl;
        }

        private void OutlookToggle_Toggled(object sender, RoutedEventArgs e)
        {
            string IsOutlookEnabled = OutlookToggle.IsOn.ToString();
            localSettings.Values["OutlookEnabled"] = IsOutlookEnabled;

            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            string OutlookAppType = localSettings.Values["OutlookAppType"].ToString();
            if (IsOutlookEnabled is "True")
            {
                if (OutlookAppType == "Website")
                {
                    mw.NavItem_Outlook.Visibility = Visibility.Visible;
                }
                else
                {
                    mw.NavItem_Outlook.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                mw.NavItem_Outlook.Visibility = Visibility.Collapsed;
            }
        }

        private void StartupPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string StartupPage = (this.StartupPage.SelectedItem as ComboBoxItem).Content.ToString();
            localSettings.Values["StartupPage"] = StartupPage;
        }

        private void GmailToggle_Toggled(object sender, RoutedEventArgs e)
        {
            string IsGmailEnabled = GmailToggle.IsOn.ToString();
            localSettings.Values["GmailEnabled"] = IsGmailEnabled;

            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            if (IsGmailEnabled is "True")
            {
                mw.NavItem_Gmail.Visibility = Visibility.Visible;
            }
            else
            {
                mw.NavItem_Gmail.Visibility = Visibility.Collapsed;
            }
        }

        private void iCloudToggle_Toggled(object sender, RoutedEventArgs e)
        {
            string IsiCloudEnabled = iCloudToggle.IsOn.ToString();
            localSettings.Values["iCloudEnabled"] = IsiCloudEnabled;

            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            if (IsiCloudEnabled is "True")
            {
                mw.NavItem_iCloud.Visibility = Visibility.Visible;
            }
            else
            {
                mw.NavItem_iCloud.Visibility = Visibility.Collapsed;
            }
        }

        private void ProtonToggle_Toggled(object sender, RoutedEventArgs e)
        {
            string IsProtonEnabled = ProtonToggle.IsOn.ToString();
            localSettings.Values["ProtonEnabled"] = IsProtonEnabled;

            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            if (IsProtonEnabled is "True")
            {
                mw.NavItem_Proton.Visibility = Visibility.Visible;
            }
            else
            {
                mw.NavItem_Proton.Visibility = Visibility.Collapsed;
            }
        }

        private void OutlookAppType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string OutlookAppType = (this.OutlookAppType.SelectedItem as ComboBoxItem).Content.ToString();
            localSettings.Values["OutlookAppType"] = OutlookAppType;

            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            string IsOutlookEnabled = localSettings.Values["OutlookEnabled"].ToString();
            string OtherOutlookAppType = localSettings.Values["OutlookAppType"].ToString();
            if (IsOutlookEnabled is "True")
            {
                if (OtherOutlookAppType == "Website")
                {
                    mw.NavItem_Outlook.Visibility = Visibility.Visible;
                    ExePathCard.Visibility = Visibility.Collapsed;
                }
                else
                {
                    mw.NavItem_Outlook.Visibility = Visibility.Collapsed;
                    ExePathCard.Visibility = Visibility.Visible;
                }
            }
            else
            {
                mw.NavItem_Outlook.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveExePath_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["OutlookExePath"] = ExePath.Text;
        }

        private void HomeToggle_Toggled(object sender, RoutedEventArgs e)
        {
            string IsHomeEnabled = HomeToggle.IsOn.ToString();
            localSettings.Values["HomeEnabled"] = IsHomeEnabled;

            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            if (IsHomeEnabled is "True")
            {
                mw.NavItem_Home.Visibility = Visibility.Visible;
            }
            else
            {
                mw.NavItem_Home.Visibility = Visibility.Collapsed;
            }
        }
    }
}
