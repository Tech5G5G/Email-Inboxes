using Windows.Storage.Pickers;
using Microsoft.Web.WebView2.Core;
using WinUIEx;

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
            //Creates picker
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.ComputerFolder,
                FileTypeFilter = { ".exe" }
            };
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, App.MainWindow.GetWindowHandle());

            //Requests for file to be chosen
            if (await openPicker.PickSingleFileAsync() is StorageFile file)
                ExePath.Text = file.Path;
        }

        private async void ClearWebViewCache(object sender, RoutedEventArgs e)
        {
            //Shows and disables UI elements
            clearCacheButton.IsEnabled = false;
            cacheProgressRing.Visibility = Visibility.Visible;

            //Generates a new WebView
            WebView2 webView = new();
            await webView.EnsureCoreWebView2Async();

            //Clears the profile cache and closes the WebView
            //await webView.CoreWebView2.Profile.ClearBrowsingDataAsync(
            //    CoreWebView2BrowsingDataKinds.DiskCache |
            //    CoreWebView2BrowsingDataKinds.CacheStorage |
            //    CoreWebView2BrowsingDataKinds.IndexedDb |
            //    CoreWebView2BrowsingDataKinds.WebSql |
            //    CoreWebView2BrowsingDataKinds.FileSystems);
            //webView.Close();

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
