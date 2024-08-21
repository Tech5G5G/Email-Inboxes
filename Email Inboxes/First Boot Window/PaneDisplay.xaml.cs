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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes.First_Boot_Window
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PaneDisplay : Page
    {
        bool autoPaneModeCarasel_Enabled = true;

        public PaneDisplay()
        {
            this.InitializeComponent();

            Auto_PaneMode();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(Backdrop), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(CustomHome), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private async void Auto_PaneMode()
        {
            if (autoPaneModeCarasel_Enabled)
            {
                leftVisual.Visibility = Visibility.Visible;
                compactVisual.Visibility = Visibility.Collapsed;
                topVisual.Visibility = Visibility.Collapsed;
            }

            await Task.Delay(2000);

            if (autoPaneModeCarasel_Enabled)
            {
                leftVisual.Visibility = Visibility.Collapsed;
                compactVisual.Visibility = Visibility.Visible;
                topVisual.Visibility = Visibility.Collapsed;
            }

            await Task.Delay(2000);

            if (autoPaneModeCarasel_Enabled)
            {
                leftVisual.Visibility = compactVisual.Visibility = topVisual.Visibility = Visibility.Collapsed;
            }

            await Task.Delay(2000);

            if (autoPaneModeCarasel_Enabled)
            {
                Auto_PaneMode();
            }
        }

        private void paneModeSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            if (autoPaneModeCarasel_Enabled)
            {
                FirstBootWindow.SettingsCache.PaneDisplayMode = "Left";

                autoPaneModeCarasel_Enabled = false;

                leftVisual.Visibility = Visibility.Visible;
                compactVisual.Visibility = Visibility.Collapsed;
                topVisual.Visibility = Visibility.Collapsed;

                paneDisplayModeDisplay.Text = "Left";
                ToolTipService.SetToolTip(paneDisplayModeDisplay, "Left mode shows a collapsable pane with icons and labels");
            }
            else
            {
                if (leftVisual.Visibility == Visibility.Visible)
                {
                    FirstBootWindow.SettingsCache.PaneDisplayMode = "Compact";

                    leftVisual.Visibility = Visibility.Collapsed;
                    compactVisual.Visibility = Visibility.Visible;
                    topVisual.Visibility = Visibility.Collapsed;

                    paneDisplayModeDisplay.Text = "Compact";
                    ToolTipService.SetToolTip(paneDisplayModeDisplay, "Compact mode shows an expandable pane only with icons");
                }
                else if (compactVisual.Visibility == Visibility.Visible)
                {
                    FirstBootWindow.SettingsCache.PaneDisplayMode = "Minimal";

                    leftVisual.Visibility = Visibility.Collapsed;
                    compactVisual.Visibility = Visibility.Collapsed;
                    topVisual.Visibility = Visibility.Collapsed;

                    paneDisplayModeDisplay.Text = "Minimal";
                    ToolTipService.SetToolTip(paneDisplayModeDisplay, "Minimal mode only shows an expandable pane");
                }
                else if (compactVisual.Visibility == Visibility.Collapsed && leftVisual.Visibility == Visibility.Collapsed && topVisual.Visibility == Visibility.Collapsed)
                {
                    FirstBootWindow.SettingsCache.PaneDisplayMode = "Top";

                    leftVisual.Visibility = Visibility.Collapsed;
                    compactVisual.Visibility = Visibility.Collapsed;
                    topVisual.Visibility = Visibility.Visible;

                    paneDisplayModeDisplay.Text = "Top";
                    ToolTipService.SetToolTip(paneDisplayModeDisplay, "Top mode shows a bar near the top of the window");

                }
                else if (topVisual.Visibility == Visibility.Visible)
                {
                    FirstBootWindow.SettingsCache.PaneDisplayMode = "Auto";

                    autoPaneModeCarasel_Enabled = true;

                    paneDisplayModeDisplay.Text = "Auto";
                    ToolTipService.SetToolTip(paneDisplayModeDisplay, "Auto mode cycles between Left, Compact and Minimal mode depending on the window size");

                    Auto_PaneMode();
                }
            }
        }
    }
}
