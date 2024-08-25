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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes.First_Boot_Window
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartupPage : Page
    {
        public StartupPage()
        {
            this.InitializeComponent();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(OtherSettings), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(Backdrop), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private void pageOption_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton pageButton = sender as ToggleButton;

            switch (pageButton.Name)
            {
                case "homeButton":
                    FirstBootWindow.SettingsCache.StartupPage = "Home";
                    outlookButton.IsChecked = gmailButton.IsChecked = yahooButton.IsChecked = iCloudButton.IsChecked = protonButton.IsChecked = false;
                    break;
                case "outlookButton":
                    FirstBootWindow.SettingsCache.StartupPage = "Outlook";
                    homeButton.IsChecked = gmailButton.IsChecked = yahooButton.IsChecked = iCloudButton.IsChecked = protonButton.IsChecked = false;
                    break;
                case "gmailButton":
                    FirstBootWindow.SettingsCache.StartupPage = "Gmail";
                    homeButton.IsChecked = outlookButton.IsChecked = yahooButton.IsChecked = iCloudButton.IsChecked = protonButton.IsChecked = false;
                    break;
                case "yahooButton":
                    FirstBootWindow.SettingsCache.StartupPage = "Yahoo Mail";
                    homeButton.IsChecked = outlookButton.IsChecked = gmailButton.IsChecked = iCloudButton.IsChecked = protonButton.IsChecked = false;
                    break;
                case "iCloudButton":
                    FirstBootWindow.SettingsCache.StartupPage = "iCloud Mail";
                    homeButton.IsChecked = outlookButton.IsChecked = gmailButton.IsChecked = yahooButton.IsChecked = protonButton.IsChecked = false;
                    break;
                case "protonButton":
                    FirstBootWindow.SettingsCache.StartupPage = "Proton Mail";
                    homeButton.IsChecked = outlookButton.IsChecked = gmailButton.IsChecked = yahooButton.IsChecked = iCloudButton.IsChecked = false;
                    break;
            }
        }
    }
}
