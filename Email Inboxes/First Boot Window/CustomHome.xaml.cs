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
    public sealed partial class CustomHome : Page
    {
        public CustomHome()
        {
            this.InitializeComponent();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(PaneDisplay), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            FirstBootWindow fbw = (FirstBootWindow)((App)Application.Current).firstBootWindow;
            fbw.FirstBootFrame.Navigate(typeof(CommandBarVisibility), null, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton homeButton = sender as ToggleButton;
            FirstBootWindow.SettingsCache.HomeEnabled = (bool)homeButton.IsChecked;
        }

        private void CalendarService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox calendarService = sender as ComboBox;

            string CalendarServiceName = (calendarService.SelectedItem as ComboBoxItem).Content.ToString();
            string CalendarServiceUrl = "disabled";

            switch (CalendarServiceName)
            {
                case "Apple Calendar":
                    CalendarServiceUrl = "https://www.icloud.com/calendar/";
                    break;
                case "Outlook Calendar":
                    CalendarServiceUrl = "https://outlook.live.com/calendar/";
                    break;
                case "Google Calendar":
                    CalendarServiceUrl = "https://calendar.google.com";
                    break;
                case "Basic Calendar":
                    CalendarServiceUrl = "basiccalendar";
                    break;
                case "Disabled":
                    CalendarServiceUrl = "disabled";
                    break;
            }

            FirstBootWindow.SettingsCache.CalendarServiceName = CalendarServiceName;
            FirstBootWindow.SettingsCache.CalendarServiceUrl = CalendarServiceUrl;
        }

        private void ToDoService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox toDoService = sender as ComboBox;

            string ToDoServiceName = (toDoService.SelectedItem as ComboBoxItem).Content.ToString();
            string ToDoServiceUrl = "disabled";

            switch (ToDoServiceName)
            {
                case "Apple Reminders":
                    ToDoServiceUrl = "https://icloud.com/reminders";
                    break;
                case "Todoist":
                    ToDoServiceUrl = "https://todoist.com";
                    break;
                case "TickTick":
                    ToDoServiceUrl = "https://ticktick.com";
                    break;
                case "Microsoft To Do":
                    ToDoServiceUrl = "https://to-do.live.com";
                    break;
                case "Google Tasks":
                    ToDoServiceUrl = "https://tasks.google.com";
                    break;
                case "Any.do":
                    ToDoServiceUrl = "https://any.do";
                    break;
                case "Disabled":
                    ToDoServiceUrl = "disabled";
                    break;
            }

            FirstBootWindow.SettingsCache.ToDoServiceName = ToDoServiceName;
            FirstBootWindow.SettingsCache.ToDoServiceUrl = ToDoServiceUrl;
        }
    }
}
