using System.Diagnostics;
using static Email_Inboxes.MainWindow;

namespace Email_Inboxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public Home()
        {   
            this.InitializeComponent();

            if (WebViews.CalendarWebView == null)
                WebViews.CalendarWebView = new WebView2() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Height = 500, Margin = new Thickness(25,0,25,25) };

            if (WebViews.ToDoWebView == null)
                WebViews.ToDoWebView = new WebView2() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Height = 500, Margin = new Thickness(25, 0, 25, 25) };

            if (WebViews.CalendarView == null)
                WebViews.CalendarView = new CalendarView() { CornerRadius = new CornerRadius(8), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Height = 500, Margin = new Thickness(25, 0, 25, 25) };

            if (WebViews.OutlookPageButton == null)
                WebViews.OutlookPageButton = OutlookPageButton;

            if (WebViews.OutlookAppButton == null)
                WebViews.OutlookAppButton = OutlookAppButton;

            if (WebViews.GmailButton == null)
                WebViews.GmailButton = GmailButton;

            if (WebViews.YahooButton == null)
                WebViews.YahooButton = YahooButton;

            if (WebViews.IcloudButton == null)
                WebViews.IcloudButton = iCloudButton;

            if (WebViews.ProtonButton == null)
                WebViews.ProtonButton = ProtonButton;

            //homeItems.Children.Add(WebViews.CalendarWebView);
            //homeItems.Children.Add(WebViews.CalendarView);
            //homeItems.Children.Add(WebViews.ToDoWebView);

            //Shows or hides the WebView that shows the user selected to do app/service
            string ToDoServiceUrl = (string)localSettings.Values[App.Settings.ToDoServiceUrl];
            if (ToDoServiceUrl == "disabled") WebViews.ToDoWebView.Visibility = Visibility.Collapsed;
            else WebViews.ToDoWebView.Source = new Uri(ToDoServiceUrl);

            //Shows or hides the user selected calendar app/service
            string CalendarServiceUrl = (string)localSettings.Values[App.Settings.CalendarServiceUrl];
            if (CalendarServiceUrl == "disabled")
            {
                WebViews.CalendarWebView.Visibility = Visibility.Collapsed;
                WebViews.CalendarView.Visibility = Visibility.Collapsed;
            }
            else if (CalendarServiceUrl == "basiccalendar")
            {
                WebViews.CalendarWebView.Visibility = Visibility.Collapsed;
                WebViews.CalendarView.Visibility = Visibility.Visible;
            }
            else
            {
                WebViews.CalendarWebView.Source = new Uri(CalendarServiceUrl);
                WebViews.CalendarView.Visibility = Visibility.Collapsed;
            }

            //Shows or hides the card of the related service depending on the user's settings
            if ((bool)localSettings.Values[App.Settings.OutlookEnabled])
            {
                OutlookType type = SettingValues.OutlookAppType;
                OutlookPageButton.Visibility = type == OutlookType.Website || type == OutlookType.BusinessWebsite ? Visibility.Visible : Visibility.Collapsed;
                OutlookAppButton.Visibility = type == OutlookType.Website || type == OutlookType.BusinessWebsite ? Visibility.Collapsed : Visibility.Visible;
            }
            else OutlookPageButton.Visibility = OutlookAppButton.Visibility = Visibility.Collapsed;

            GmailButton.Visibility = (bool)localSettings.Values[App.Settings.GmailEnabled] ? Visibility.Visible : Visibility.Collapsed;

            iCloudButton.Visibility = (bool)localSettings.Values[App.Settings.iCloudEnabled] ? Visibility.Visible : Visibility.Collapsed;

            ProtonButton.Visibility = (bool)localSettings.Values[App.Settings.ProtonEnabled] ? Visibility.Visible : Visibility.Collapsed;

            YahooButton.Visibility = (bool)localSettings.Values[App.Settings.YahooEnabled] ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SettingsCard_Click(object sender, RoutedEventArgs e)
        {
            Process.Start((string)localSettings.Values[App.Settings.OutlookExePath]);
        }

        //All below changes the selected nvSample item with a different animation
        private void SettingsCard_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Proton");
            mw.nvSample.SelectedItem = item;
            mw.contentFrame.Navigate(typeof(Proton), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void SettingsCard_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_iCloud");
            mw.nvSample.SelectedItem = item;
            mw.contentFrame.Navigate(typeof(iCloud), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void SettingsCard_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Gmail");
            mw.nvSample.SelectedItem = item;
            mw.contentFrame.Navigate(typeof(Gmail), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void OutlookNavigate(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Outlook");
            mw.nvSample.SelectedItem = item;
            mw.contentFrame.Navigate(typeof(Outlook), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void YahooNavigate(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)((App)Application.Current).m_window;
            var item = mw.nvSample.MenuItems.First(i => ((NavigationViewItem)i).Name == "NavItem_Yahoo");
            mw.nvSample.SelectedItem = item;
            mw.contentFrame.Navigate(typeof(Yahoo), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        //Methods that make the inboxButtonsSV scroll buttons work
        private void inboxButtonsSV_ScrollRight(object sender, RoutedEventArgs e)
        {
            //Scrolls 200px to the right
            double toScroll = inboxButtonsSV.HorizontalOffset + 200;
            inboxButtonsSV.ChangeView(toScroll, null, null);
        }

        private void inboxButtonsSV_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollViewer inboxButtonsSV = sender as ScrollViewer;

            scrollRightButton.Opacity = inboxButtonsSV.HorizontalOffset == inboxButtonsSV.ScrollableWidth ? 0 : 1;

            scrollLeftButton.Opacity = inboxButtonsSV.HorizontalOffset == 0 ? 0 : 1;
        }

        private void inboxButtonsSV_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer inboxButtonsSV = sender as ScrollViewer;

            scrollRightButton.Opacity = inboxButtonsSV.HorizontalOffset == inboxButtonsSV.ScrollableWidth ? 0 : 1;

            scrollLeftButton.Opacity = inboxButtonsSV.HorizontalOffset == 0 ? 0 : 1;
        }

        private void inboxButtonsSV_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            scrollRightButton.Opacity = inboxButtonsSV.HorizontalOffset == inboxButtonsSV.ScrollableWidth ? 0 : 1;

            scrollLeftButton.Opacity = inboxButtonsSV.HorizontalOffset == 0 ? 0 : 1;
        }

        private void inboxButtonsSV_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            scrollRightButton.Opacity = scrollLeftButton.Opacity = 0;
        }

        private void scrollLeftButton_Click(object sender, RoutedEventArgs e)
        {
            //Scrolls 200px to the left
            double toScroll = inboxButtonsSV.HorizontalOffset + -200;
            var scrollable = inboxButtonsSV.ChangeView(toScroll, null, null);
        }

        private void scrollRightButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            scrollRightButton.Opacity = inboxButtonsSV.HorizontalOffset == inboxButtonsSV.ScrollableWidth ? 0 : 1;

            scrollLeftButton.Opacity = inboxButtonsSV.HorizontalOffset == 0 ? 0 : 1;
        }

        private void scrollLeftButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            scrollRightButton.Opacity = inboxButtonsSV.HorizontalOffset == inboxButtonsSV.ScrollableWidth ? 0 : 1;

            scrollLeftButton.Opacity = inboxButtonsSV.HorizontalOffset == 0 ? 0 : 1;
        }
    }
}
