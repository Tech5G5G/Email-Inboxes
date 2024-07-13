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
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage.Pickers;

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

            ExePath.Text = (string)localSettings.Values[App.Settings.OutlookExePath];
            
            //Switch to determine which to do service the user has selected and display it
            switch ((string)localSettings.Values[App.Settings.ToDoServiceName])
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
                //Backwards compatiblity code
                case "None":
                    ToDoService.SelectedIndex = 6;
                    break;
            }

            //Switch to determine which calendar service the user has selected and display it
            switch (localSettings.Values[App.Settings.CalendarServiceName])
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
                //Backwards compatiblity code
                case "None":
                    CalendarService.SelectedIndex = 3;
                    break;
            }

            //Enables or disables toggles & two cards depending on user's setting 
            OutlookToggle.IsOn = ExePathCard.IsEnabled = appTypeCard.IsEnabled = (bool)localSettings.Values[App.Settings.OutlookEnabled];

            GmailToggle.IsOn = (bool)localSettings.Values[App.Settings.GmailEnabled];

            iCloudToggle.IsOn = (bool)localSettings.Values[App.Settings.iCloudEnabled];

            ProtonToggle.IsOn = (bool)localSettings.Values[App.Settings.ProtonEnabled];

            HomeToggle.IsOn = (bool)localSettings.Values[App.Settings.HomeEnabled];

            yahooToggle.IsOn = (bool)localSettings.Values[App.Settings.YahooEnabled];

            commandBarToggle.IsOn = (bool)localSettings.Values[App.Settings.CommandBarEnabled];

            //Switch to determine which pane display mode the user has selected and display it
            switch ((string)localSettings.Values[App.Settings.PaneDisplayMode])
            {
                case "Auto":
                    PaneDisplayMode.SelectedIndex = 0;
                    break;
                case "Left":
                    PaneDisplayMode.SelectedIndex = 1;
                    break;
                case "Compact":
                    PaneDisplayMode.SelectedIndex = 2;
                    break;
                case "Minimal":
                    PaneDisplayMode.SelectedIndex = 3;
                    break;
                case "Top":
                    PaneDisplayMode.SelectedIndex = 4;
                    break;
            }

            //Switch to determine which Outlook app type the user has selected and display it.
            //Also shows or hides a card depending on the user's setting.
            switch ((string)localSettings.Values[App.Settings.OutlookAppType])
            {
                case "Website":
                    OutlookAppType.SelectedIndex = 0;
                    ExePathCard.Visibility = Visibility.Collapsed;
                    break;
                case "Business website":
                    OutlookAppType.SelectedIndex = 1;
                    ExePathCard.Visibility = Visibility.Collapsed;
                    break;
                case "exe":
                    OutlookAppType.SelectedIndex = 2;
                    ExePathCard.Visibility = Visibility.Visible;
                    break;
            }

            //Shows the user's setting for window backdrop
            switch ((string)localSettings.Values[App.Settings.Backdrop])
            {
                case "Mica":
                    WindowBackdrop.SelectedIndex = 0;
                    break;
                case "Mica Alt":
                    WindowBackdrop.SelectedIndex = 1;
                    break;
                case "Acrylic":
                    WindowBackdrop.SelectedIndex = 2;
                    break;
            }

            //Switch to determine which startup page the user has selected and display it
            switch ((string)localSettings.Values[App.Settings.StartupPage])
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
                case "Yahoo Mail":
                    StartupPage.SelectedIndex = 3;
                    break;
                case "iCloud Mail":
                    StartupPage.SelectedIndex = 4;
                    break;
                case "Proton Mail":
                    StartupPage.SelectedIndex = 5;
                    break;
            }

            MakeSettingsChangeable();
        }

        private void ToDoService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves the to do service name and URL
                string ToDoServiceName = (ToDoService.SelectedItem as ComboBoxItem).Content.ToString();
                string ToDoServiceUrl = "disabled";

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

                localSettings.Values[App.Settings.ToDoServiceName] = ToDoServiceName;
                localSettings.Values[App.Settings.ToDoServiceUrl] = ToDoServiceUrl;
            }
        }

        private void CalendarService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves the calendar service name and URL
                string CalendarServiceName = (CalendarService.SelectedItem as ComboBoxItem).Content.ToString();
                string CalendarServiceUrl = "disabled";

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

                localSettings.Values[App.Settings.CalendarServiceName] = CalendarServiceName;
                localSettings.Values[App.Settings.CalendarServiceUrl] = CalendarServiceUrl;
            }
        }

        private void OutlookToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves and updates the UI when the OutlookToggle is toggled
                bool IsOutlookEnabled = OutlookToggle.IsOn;
                ExePathCard.IsEnabled = appTypeCard.IsEnabled = IsOutlookEnabled;
                localSettings.Values[App.Settings.OutlookEnabled] = IsOutlookEnabled;

                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                if (IsOutlookEnabled)
                {
                    mw.NavItem_Outlook.Visibility = (string)localSettings.Values[App.Settings.OutlookAppType] == "Website" ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    mw.NavItem_Outlook.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void StartupPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves the value of StartupPage when its selection is changed
                localSettings.Values[App.Settings.StartupPage] = (StartupPage.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void GmailToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves and updates the UI when the GmailToggle is toggled
                bool IsGmailEnabled = GmailToggle.IsOn;
                localSettings.Values[App.Settings.GmailEnabled] = IsGmailEnabled;

                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                mw.NavItem_Gmail.Visibility = IsGmailEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void iCloudToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves and updates the UI when the iCloudToggle is toggled
                bool IsiCloudEnabled = iCloudToggle.IsOn;
                localSettings.Values[App.Settings.iCloudEnabled] = IsiCloudEnabled;

                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                mw.NavItem_iCloud.Visibility = IsiCloudEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void ProtonToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves and updates the UI when the ProtonToggle is toggled
                bool IsProtonEnabled = ProtonToggle.IsOn;
                localSettings.Values[App.Settings.ProtonEnabled] = IsProtonEnabled;

                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                mw.NavItem_Proton.Visibility = IsProtonEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OutlookAppType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves and updates the UI when the selection of OutlookAppType is changed
                string outlookAppType = (OutlookAppType.SelectedItem as ComboBoxItem).Content.ToString();
                localSettings.Values[App.Settings.OutlookAppType] = outlookAppType;

                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                if ((bool)localSettings.Values[App.Settings.OutlookEnabled])
                {
                    mw.NavItem_Outlook.Visibility = outlookAppType == "Website" || outlookAppType == "Business website" ? Visibility.Visible : Visibility.Collapsed;
                    ExePathCard.Visibility = outlookAppType == "Website" || outlookAppType == "Business website" ? Visibility.Collapsed : Visibility.Visible;

                    //Refreshes the OutlookPage to reflect changes
                    if (outlookAppType == "Website" || outlookAppType == "Business website")
                    {
                        MainWindow.Pages.HomePage.HomeWebView.Close();
                        MainWindow.Pages.HomePage.CalendarWebView.Close();
                        MainWindow.Pages.HomePage = new Home();
                        MainWindow.Pages.OutlookPage.OutlookWebView.Close();
                        MainWindow.Pages.OutlookPage = new Outlook();
                    }
                    else
                    {
                        MainWindow.Pages.HomePage.HomeWebView.Close();
                        MainWindow.Pages.HomePage.CalendarWebView.Close();
                        MainWindow.Pages.HomePage = new Home();
                    }
                }
                else
                {
                    mw.NavItem_Outlook.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SaveExePath_Click(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves the OutlookExePath when the save button is clicked
                localSettings.Values[App.Settings.OutlookExePath] = ExePath.Text;
            }
        }

        private void HomeToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves and updates the UI when the HomeToggle is toggled
                bool isHomeEnabled = HomeToggle.IsOn;
                localSettings.Values[App.Settings.HomeEnabled] = isHomeEnabled;

                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                mw.NavItem_Home.Visibility = isHomeEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void WindowBackdrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves the user's selection for the window backdrop
                var windowBackdrop = (WindowBackdrop.SelectedItem as ComboBoxItem).Content.ToString();
                localSettings.Values[App.Settings.Backdrop] = windowBackdrop;

                //Updates the UI accordingly
                SystemBackdrop backdropToSet = null;
                switch (windowBackdrop)
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
                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                mw.mainwindow.SystemBackdrop = backdropToSet;
            }
        }

        private void PaneDisplayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves the user's selection for the pane display mode
                var paneDisplayMode = (PaneDisplayMode.SelectedItem as ComboBoxItem).Content.ToString();
                localSettings.Values[App.Settings.PaneDisplayMode] = paneDisplayMode;

                //Updates the UI accordingly
                var displayModeToSet = NavigationViewPaneDisplayMode.Left;
                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                switch (paneDisplayMode)
                {
                    case "Auto":
                        mw.nvSample.Margin = new Thickness() { Top = 0 };
                        mw.AppTitleBar.Margin = new Thickness() { Left = 51 };
                        displayModeToSet = NavigationViewPaneDisplayMode.Auto;
                        break;
                    case "Left":
                        mw.nvSample.Margin = new Thickness() { Top = 0 };
                        mw.AppTitleBar.Margin = new Thickness() { Left = 51 };
                        displayModeToSet = NavigationViewPaneDisplayMode.Left;
                        break;
                    case "Compact":
                        mw.nvSample.Margin = new Thickness() { Top = 0 };
                        mw.AppTitleBar.Margin = new Thickness() { Left = 51 };
                        displayModeToSet = NavigationViewPaneDisplayMode.LeftCompact;
                        break;
                    case "Minimal":
                        mw.nvSample.Margin = new Thickness() { Top = 0 };
                        mw.AppTitleBar.Margin = new Thickness() { Left = 51 };
                        displayModeToSet = NavigationViewPaneDisplayMode.LeftMinimal;
                        break;
                    case "Top":
                        mw.nvSample.Margin = new Thickness() { Top = 48 };
                        mw.AppTitleBar.Margin = new Thickness() { Left = 16 };
                        displayModeToSet = NavigationViewPaneDisplayMode.Top;
                        break;
                }
                mw.nvSample.PaneDisplayMode = displayModeToSet;
            }
        }

        private async void openExePathFilePicker_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker();

            var window = (MainWindow)((App)Application.Current).m_window;

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            openPicker.FileTypeFilter.Add(".exe");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
                ExePath.Text = file.Path;
        }

        private void YahooToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves and updates the UI when the yahooToggle is toggled
                bool isYahooEnabled = yahooToggle.IsOn;
                localSettings.Values[App.Settings.YahooEnabled] = isYahooEnabled;

                MainWindow mw = (MainWindow)((App)Application.Current).m_window;
                mw.NavItem_Yahoo.Visibility = isYahooEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void CommandBarToggle_Toggled(object sender, RoutedEventArgs e)
        {
            //Only allows settings to be changed once the settings page has completely loaded in
            if (App.Settings.SettingsChangable)
            {
                //Saves the status of commandBarToggle when it is toggled
                localSettings.Values[App.Settings.CommandBarEnabled] = commandBarToggle.IsOn;
            }
        }

        private void MakeSettingsChangeable()
        {
            App.Settings.SettingsChangable = true;
        }
    }
}
