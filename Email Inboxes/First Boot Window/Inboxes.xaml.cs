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
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes.First_Boot_Window
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Inboxes : Page
    {
        public Inboxes()
        {
            this.InitializeComponent();

            ExePath.Text = (string)ApplicationData.Current.LocalSettings.Values[App.Settings.OutlookExePath];
            OutlookAppType.SelectedIndex = 0;
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(CommandBarVisibility), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void OutlookToggle_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton outlookToggle = sender as ToggleButton;

            moreOutlookOptions.Visibility = (bool)outlookToggle.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            FirstBootWindow.SettingsCache.OutlookEnabled = (bool)outlookToggle.IsChecked;
        }

        private void SaveExePath_Click(object sender, RoutedEventArgs e)
        {
            FirstBootWindow.SettingsCache.OutlookExePath = ExePath.Text;
        }

        private async void OpenExePathFilePicker_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker();

            var window = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            openPicker.FileTypeFilter.Add(".exe");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
                ExePath.Text = file.Path;
        }

        private void OutlookAppType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string outlookAppType = (OutlookAppType.SelectedItem as ComboBoxItem).Content.ToString();
            FirstBootWindow.SettingsCache.OutlookAppType = outlookAppType;

            ExePathCard.Visibility = outlookAppType == "exe" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;

            switch (toggleButton.Name)
            {
                case "GmailButton":
                    FirstBootWindow.SettingsCache.GmailEnabled = (bool)toggleButton.IsChecked;
                    break;
                case "YahooButton":
                    FirstBootWindow.SettingsCache.YahooEnabled = (bool)toggleButton.IsChecked;
                    break;
                case "IcloudButton":
                    FirstBootWindow.SettingsCache.IcloudEnabled = (bool)toggleButton.IsChecked;
                    break;
                case "ProtonButton":
                    FirstBootWindow.SettingsCache.ProtonEnabled = (bool)toggleButton.IsChecked;
                    break;
            }
        }
    }
}
