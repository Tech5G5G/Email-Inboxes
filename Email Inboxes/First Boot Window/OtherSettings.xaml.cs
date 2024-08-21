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
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes.First_Boot_Window
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OtherSettings : Page
    {
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public OtherSettings()
        {
            this.InitializeComponent();

            outlookButton.Visibility = FirstBootWindow.SettingsCache.OutlookEnabled ? Visibility.Visible : Visibility.Collapsed;
            gmailButton.Visibility = FirstBootWindow.SettingsCache.GmailEnabled ? Visibility.Visible : Visibility.Collapsed;
            yahooButton.Visibility = FirstBootWindow.SettingsCache.YahooEnabled ? Visibility.Visible : Visibility.Collapsed;
            iCloudButton.Visibility = FirstBootWindow.SettingsCache.IcloudEnabled ? Visibility.Visible : Visibility.Collapsed;
            protonButton.Visibility = FirstBootWindow.SettingsCache.ProtonEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            //fbw.FirstBootFrame.Navigate(typeof(OtherSettings), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(CustomHome), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
        }
    }
}
