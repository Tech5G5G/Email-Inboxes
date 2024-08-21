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
        }
    }
}
