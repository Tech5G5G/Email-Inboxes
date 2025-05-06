using Windows.Storage.Pickers;
using Microsoft.Web.WebView2.Core;

namespace Email_Inboxes.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            //Initialize Outlook ExePath textbox and button
            ExePath.Text = SettingValues.OutlookExePath;
            SaveExePath.Click += (sender, e) => SettingValues.OutlookExePath = ExePath.Text;

            //Uses combo boxes to display the users preferences
            ToDoService.SelectedIndex = (int)SettingValues.ToDoService;
            OutlookAppType.SelectedIndex = (int)SettingValues.OutlookAppType;
            CalendarService.SelectedIndex = (int)SettingValues.CalendarService;
            WindowBackdrop.SelectedIndex = (int)SettingValues.Backdrop;
            StartupPage.SelectedIndex = (int)SettingValues.StartupPage;

            //Switch to determine which pane display mode the user has selected and display it
            PaneDisplayMode.SelectedIndex = SettingValues.PaneDisplayMode switch
            {
                NavigationViewPaneDisplayMode.Auto => 0,
                NavigationViewPaneDisplayMode.Left => 1,
                NavigationViewPaneDisplayMode.LeftCompact => 2,
                NavigationViewPaneDisplayMode.LeftMinimal => 3,
                NavigationViewPaneDisplayMode.Top => 4,
                _ => 0
            };

            //Gets user's service enablement preferences and displays them
            //Also shows or hides a card depending on the user's setting.
            HomeToggle.IsOn = SettingValues.HomeEnabled;
            OutlookToggle.IsOn = ExePathCard.IsEnabled = appTypeCard.IsEnabled = SettingValues.OutlookEnabled;
            GmailToggle.IsOn = SettingValues.GmailEnabled;
            yahooToggle.IsOn = SettingValues.YahooEnabled;
            iCloudToggle.IsOn = SettingValues.iCloudEnabled;
            ProtonToggle.IsOn = SettingValues.ProtonEnabled;
            commandBarToggle.IsOn = SettingValues.CommandBarEnabled;
            ExePathCard.Visibility = SettingValues.OutlookAppType == OutlookType.Desktop ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void ExePathCard_OpenFilePicker(object sender, RoutedEventArgs e)
        {
            //Creates FileOpenPicker
            var openPicker = new FileOpenPicker() { ViewMode = PickerViewMode.List, SuggestedStartLocation = PickerLocationId.ComputerFolder };
            openPicker.FileTypeFilter.Add(".exe");

            //Initializes WinRT Interop
            var window = (MainWindow)((App)Application.Current).m_window;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            //Requests for file to be chosen
            var file = await openPicker.PickSingleFileAsync();
            ExePath.Text = file != null ? file.Path : ExePath.Text;
        }

        private async void ClearWebViewCache(object sender, RoutedEventArgs e)
        {
            //Shows and disables UI elements
            clearCacheButton.IsEnabled = false;
            cacheProgressRing.Visibility = Visibility.Visible;

            //Generates a new WebView
            WebView2 webView2 = new WebView2();
            await webView2.EnsureCoreWebView2Async();

            //Clears the profile cache and closes the WebView
            CoreWebView2Profile profile = webView2.CoreWebView2.Profile;
            CoreWebView2BrowsingDataKinds dataKinds = CoreWebView2BrowsingDataKinds.DiskCache | CoreWebView2BrowsingDataKinds.CacheStorage | CoreWebView2BrowsingDataKinds.IndexedDb | CoreWebView2BrowsingDataKinds.WebSql | CoreWebView2BrowsingDataKinds.FileSystems;
            await profile.ClearBrowsingDataAsync(dataKinds);
            webView2.Close();

            //Hides and reenables UI elements
            clearCacheButton.IsEnabled = true;
            cacheProgressRing.Visibility = Visibility.Collapsed;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var senderBox = sender as ComboBox;
            var senderTag = (senderBox.SelectedItem as ComboBoxItem).Tag;
            switch (senderBox.Name)
            {
                case "WindowBackdrop":
                    SettingValues.Backdrop = (BackdropType)senderTag;
                    break;
                case "PaneDisplayMode":
                    SettingValues.PaneDisplayMode = (NavigationViewPaneDisplayMode)senderTag;
                    break;
                case "OutlookAppType":
                    SettingValues.OutlookAppType = (OutlookType)senderTag;
                    ExePathCard.Visibility = (OutlookType)senderTag == OutlookType.Desktop ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "StartupPage":
                    SettingValues.StartupPage = (PageType)senderTag;
                    break;
                case "CalendarService":
                    SettingValues.CalendarService = (CalendarService)senderTag;
                    break;
                case "ToDoService":
                    SettingValues.ToDoService = (ToDoService)senderTag;
                    break;
            }
        }

        private void Toggle_Toggled(object sender, RoutedEventArgs e)
        {
            var senderToggle = sender as ToggleSwitch;
            switch (senderToggle.Name)
            {
                case "HomeToggle":
                    SettingValues.HomeEnabled = senderToggle.IsOn;
                    break;
                case "OutlookToggle":
                    SettingValues.OutlookEnabled = senderToggle.IsOn;
                    appTypeCard.IsEnabled = ExePathCard.IsEnabled = senderToggle.IsOn;
                    break;
                case "GmailToggle":
                    SettingValues.GmailEnabled = senderToggle.IsOn;
                    break;
                case "yahooToggle":
                    SettingValues.YahooEnabled = senderToggle.IsOn;
                    break;
                case "iCloudToggle":
                    SettingValues.iCloudEnabled = senderToggle.IsOn;
                    break;
                case "ProtonToggle":
                    SettingValues.ProtonEnabled = senderToggle.IsOn;
                    break;
                case "commandBarToggle":
                    SettingValues.CommandBarEnabled = senderToggle.IsOn;
                    break;
            }
        }
    }
}
