using Windows.UI.WindowManagement;

namespace Email_Inboxes
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainWindow : Window
    {
        public static class WebViews
        {
            public static WebView2 OutlookWebView
            {
                get { return outlookWebView; }
                set { outlookWebView = value; }
            }
            private static WebView2 outlookWebView;

            public static WebView2 GmailWebView
            {
                get { return gmailWebView; } 
                set {  gmailWebView = value; }
            }
            private static WebView2 gmailWebView;

            public static WebView2 YahooWebView
            {
                get { return yahooWebView; }
                set { yahooWebView = value; }
            }
            private static WebView2 yahooWebView;

            public static WebView2 IcloudWebView
            {
                get { return iCloudWebView; }
                set { iCloudWebView = value; }
            }
            private static WebView2 iCloudWebView;

            public static WebView2 ProtonWebView
            {
                get { return protonWebView; }
                set { protonWebView = value; }
            }
            private static WebView2 protonWebView;

            public static WebView2 ToDoWebView
            {
                get { return toDoWebView; }
                set { toDoWebView = value; }
            }
            private static WebView2 toDoWebView;

            public static WebView2 CalendarWebView
            {
                get { return calendarWebView; }
                set { calendarWebView = value; }
            }
            private static WebView2 calendarWebView;


            public static CalendarView CalendarView
            {
                get { return calendarView; }
                set { calendarView = value; }
            }
            private static CalendarView calendarView;


            public static InboxButton OutlookPageButton
            {
                get { return outlookPageButton; }
                set { outlookPageButton = value; }
            }
            private static InboxButton outlookPageButton;

            public static InboxButton OutlookAppButton
            {
                get { return outlookAppButton; }
                set { outlookAppButton = value; }
            }
            private static InboxButton outlookAppButton;

            public static InboxButton GmailButton
            {
                get { return gmailButton; }
                set { gmailButton = value; }
            }
            private static InboxButton gmailButton;

            public static InboxButton YahooButton
            {
                get { return yahooButton; }
                set { yahooButton = value; }
            }
            private static InboxButton yahooButton;

            public static InboxButton IcloudButton
            {
                get { return iCloudButton; }
                set { iCloudButton = value; }
            }
            private static InboxButton iCloudButton;

            public static InboxButton ProtonButton
            {
                get { return protonButton; }
                set { protonButton = value; }
            }
            private static InboxButton protonButton;
        }

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public MainWindow()
        {
            this.InitializeComponent();

            Title = "Email Inboxes";
            ExtendsContentIntoTitleBar = true;

            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            AppWindow.SetIcon("Assets/Mail.ico");

            //Title bar initialization
            SetTitleBar(AppTitleBar);
            AppTitleBar.PaneToggleRequested += (sender, args) => nvSample.IsPaneOpen = !nvSample.IsPaneOpen;

            //Updates setting value when window size changed
            var presenter = AppWindow.Presenter as OverlappedPresenter;
            this.SizeChanged += (sender, args) =>
            {
                //If the presenter is null, do not continue with method
                if (presenter is null)
                    return;

                //Gets & saves window state
                OverlappedPresenterState windowState = presenter.State;
                if (windowState != OverlappedPresenterState.Minimized)
                    SettingValues.WindowState = windowState;
            };

            //Reads value of WindowState and decides whether to maximize the window
            if (SettingValues.WindowState == OverlappedPresenterState.Maximized)
                presenter.Maximize();

            //Sets the backdrop of the window based on the user's preference
            this.SystemBackdrop = SettingValues.Backdrop.ToSystemBackdrop();
            SettingValues.BackdropChanged += (args) => this.SystemBackdrop = ((BackdropType)args.NewValue).ToSystemBackdrop();

            //Changes the pane display mode depending on the user's setting for PaneDisplayMode
            Set_PaneDisplayMode(SettingValues.PaneDisplayMode);
            SettingValues.PaneDisplayModeChanged += (args) => Set_PaneDisplayMode((NavigationViewPaneDisplayMode)args.NewValue);

            //Hides or shows the related nvSample NavItem depending on the user's settings
            NavItem_Outlook.Visibility = SettingValues.OutlookEnabled ? SettingValues.OutlookAppType == OutlookType.Desktop ? Visibility.Collapsed : Visibility.Visible : Visibility.Collapsed;
            NavItem_Home.Visibility = SettingValues.HomeEnabled ? Visibility.Visible : Visibility.Collapsed;
            NavItem_Gmail.Visibility = SettingValues.GmailEnabled ? Visibility.Visible : Visibility.Collapsed;
            NavItem_Yahoo.Visibility = SettingValues.YahooEnabled ? Visibility.Visible : Visibility.Collapsed;
            NavItem_iCloud.Visibility = SettingValues.iCloudEnabled ? Visibility.Visible : Visibility.Collapsed;
            NavItem_Proton.Visibility = SettingValues.ProtonEnabled ? Visibility.Visible : Visibility.Collapsed;

            //Changes the selected item of the nvSample NavigationView to the user's selected startup page
            nvSample.SelectedItem = SettingValues.StartupPage switch
            {
                PageType.Outlook => NavItem_Outlook,
                PageType.Gmail => NavItem_Gmail,
                PageType.Yahoo => NavItem_Yahoo,
                PageType.iCloud => NavItem_iCloud,
                PageType.Proton => NavItem_Proton,
                _ => NavItem_Home
            };
        }

        private void Set_PaneDisplayMode(NavigationViewPaneDisplayMode mode)
        {
            nvSample.PaneDisplayMode = mode;
            AppTitleBar.IsPaneToggleButtonVisible = mode != NavigationViewPaneDisplayMode.Top;
            AppTitleBar.Margin = mode == NavigationViewPaneDisplayMode.Top ? new Thickness(-14, 0, 0, 0) : new Thickness();
        }

        private void nvSample_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //Changes the content of the contentFrame related to the users selection
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Home":
                    contentFrame.Navigate(typeof(Home), null, new EntranceNavigationTransitionInfo());
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Visible && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Collapsed;
                    }
                    break;
                case "NavItem_Gmail":
                    contentFrame.Navigate(typeof(Gmail), null, new EntranceNavigationTransitionInfo());
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                    }
                    BackButton.IsEnabled = WebViews.GmailWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.GmailWebView.CanGoForward;
                    break;
                case "NavItem_iCloud":
                    contentFrame.Navigate(typeof(iCloud), null, new EntranceNavigationTransitionInfo());
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                    }
                    BackButton.IsEnabled = WebViews.IcloudWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.IcloudWebView.CanGoForward;
                    break;
                case "NavItem_Proton":
                    contentFrame.Navigate(typeof(Proton), null, new EntranceNavigationTransitionInfo());
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                    }
                    BackButton.IsEnabled = WebViews.ProtonWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.ProtonWebView.CanGoForward;
                    break;
                case "NavItem_Outlook":
                    contentFrame.Navigate(typeof(Outlook), null, new EntranceNavigationTransitionInfo());
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                    }
                    BackButton.IsEnabled = WebViews.OutlookWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.OutlookWebView.CanGoForward;
                    break;
                case "NavItem_Yahoo":
                    contentFrame.Navigate(typeof(Yahoo), null, new EntranceNavigationTransitionInfo());
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Collapsed && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Visible;
                    }
                    BackButton.IsEnabled = WebViews.YahooWebView.CanGoBack;
                    ForwardButton.IsEnabled = WebViews.YahooWebView.CanGoForward;
                    break;
                case "SettingsItem":
                    contentFrame.Navigate(typeof(SettingsPage), null, new EntranceNavigationTransitionInfo());
                    if (CommandBar.Visibility == Visibility.Visible && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Collapsed;
                    }
                    break;
                default:
                    contentFrame.Navigate(typeof(Home), null, new EntranceNavigationTransitionInfo());
                    App.Settings.SettingsChangable = false;
                    if (CommandBar.Visibility == Visibility.Visible && (bool)localSettings.Values[App.Settings.CommandBarEnabled])
                    {
                        CommandBar.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    string outlookWebViewSource = "https://https://outlook.live.com";
                    if ((string)localSettings.Values[App.Settings.OutlookAppType] == "Website")
                        outlookWebViewSource = "https://outlook.live.com";
                    else
                        outlookWebViewSource = "https://outlook.office.com";
                    WebViews.OutlookWebView.Source = new Uri(outlookWebViewSource);
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.Source = new Uri("https://mail.google.com/mail/u/0/");
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.Source = new Uri("https://mail.yahoo.com");
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.Source = new Uri("https://www.icloud.com/mail");
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.Source = new Uri("https://mail.proton.me/");
                    break;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.Reload();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.Reload();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.Reload();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.Reload();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.Reload();
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.GoBack();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.GoBack();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.GoBack();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.GoBack();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.GoBack();
                    break;
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.GoForward();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.GoForward();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.GoForward();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.GoForward();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.GoForward();
                    break;
            }
        }

        private void DevToolsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.CoreWebView2.OpenDevToolsWindow();
                    break;
            }
        }

        private void TaskManagerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)nvSample.SelectedItem;
            switch (selectedItem.Name)
            {
                case "NavItem_Outlook":
                    WebViews.OutlookWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_Gmail":
                    WebViews.GmailWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_Yahoo":
                    WebViews.YahooWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_iCloud":
                    WebViews.IcloudWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
                case "NavItem_Proton":
                    WebViews.ProtonWebView.CoreWebView2.OpenTaskManagerWindow();
                    break;
            }
        }
    }
}
