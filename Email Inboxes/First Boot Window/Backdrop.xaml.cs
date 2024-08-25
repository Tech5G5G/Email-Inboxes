using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes.First_Boot_Window
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Backdrop : Page
    {
        public Backdrop()
        {
            this.InitializeComponent();
        }

        FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;

        private void NextClick(object sender, RoutedEventArgs e)
        {
            fbw.FirstBootFrame.Navigate(typeof(StartupPage), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            fbw.FirstBootFrame.Navigate(typeof(PaneDisplay), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (sender as SettingsCard).Content as RadioButton;
            radioButton.IsChecked = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            string checkedItem = (sender as RadioButton).Content as string;

            if (checkedItem is not null)
                FirstBootWindow.SettingsCache.Backdrop = checkedItem;

            switch (checkedItem)
            {
                case "Mica":
                    fbw.SystemBackdrop = new MicaBackdrop();
                    micaAltCheck.IsChecked = acrylicCheck.IsChecked = false;
                    break;
                case "Mica Alt":
                    fbw.SystemBackdrop = new MicaBackdrop() { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt };
                    micaCheck.IsChecked = acrylicCheck.IsChecked = false;
                    break;
                case "Acrylic":
                    fbw.SystemBackdrop = new DesktopAcrylicBackdrop();
                    micaCheck.IsChecked = micaAltCheck.IsChecked = false;
                    break;
            }
        }
    }
}
