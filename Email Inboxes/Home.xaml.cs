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
using System.Diagnostics;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
        }

        private void SettingsCard_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE");
        }

        private void SettingsCard_Click_1(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            Frame.Navigate(typeof(Proton), contentFrame);
        }

        private void SettingsCard_Click_2(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            Frame.Navigate(typeof(iCloud), contentFrame);
        }

        private void SettingsCard_Click_3(object sender, RoutedEventArgs e)
        {
            Frame contentFrame = ((App)Application.Current).contentFrame;
            Frame.Navigate(typeof(Gmail), contentFrame);
        }
    }
}
