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
            SaveExePath.Click += (sender, e) => SettingValues.OutlookExePath.Value = ExePath.Text;

            //Disables parts of UI depending on the user's settings
            appTypeCard.IsEnabled = ExePathCard.IsEnabled = OutlookToggle.IsOn;
            ExePathCard.Visibility = SettingValues.OutlookAppType == OutlookType.Desktop ? Visibility.Visible : Visibility.Collapsed;

            //Initialize ToggleSwitches
            InitializeToggleSwitch(HomeToggle, SettingValues.HomeEnabled);
            InitializeToggleSwitch(OutlookToggle, SettingValues.OutlookEnabled, () => appTypeCard.IsEnabled = ExePathCard.IsEnabled = OutlookToggle.IsOn);
            InitializeToggleSwitch(GmailToggle, SettingValues.GmailEnabled);
            InitializeToggleSwitch(iCloudToggle, SettingValues.iCloudEnabled);
            InitializeToggleSwitch(ProtonToggle, SettingValues.ProtonEnabled);
            InitializeToggleSwitch(yahooToggle, SettingValues.YahooEnabled);
            InitializeToggleSwitch(commandBarToggle, SettingValues.CommandBarEnabled);

            //Initialize ComboBoxes
            InitializeComboBox(OutlookAppType, SettingValues.OutlookAppType, () => ExePathCard.Visibility = SettingValues.OutlookAppType == OutlookType.Desktop ? Visibility.Visible : Visibility.Collapsed);
            InitializeComboBox(ToDoService, SettingValues.ToDoService);
            InitializeComboBox(CalendarService, SettingValues.CalendarService);

            InitializeComboBox(WindowBackdrop, SettingValues.Backdrop);
            InitializeComboBox(StartupPage, SettingValues.StartupPage);
            InitializeComboBox(PaneDisplayMode, SettingValues.PaneDisplayMode);
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

        private static void InitializeToggleSwitch(ToggleSwitch toggle, Setting<bool> setting, Action action = null)
        {
            toggle.IsOn = setting.Value;
            toggle.Toggled += (s, e) =>
            {
                setting.Value = toggle.IsOn;
                action?.Invoke();
            };
        }

        private static void InitializeComboBox<T>(ComboBox box, EnumSetting<T> setting, Action action = null) where T : Enum
        {
            box.SelectedIndex = (int)(object)setting.Value;
            box.SelectionChanged += (s, e) =>
            {
                setting.Value = (T)(object)box.SelectedIndex;
                action?.Invoke();
            };
        }
    }
}
