using Email_Inboxes;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Appointments;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.Services.TargetedContent;
using Windows.Storage;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    public sealed class InboxButton : Control
    {
        public InboxButton()
        {
            this.DefaultStyleKey = typeof(InboxButton);
        }

        public event RoutedEventHandler Click;

        private Button ButtonControl { get; set; }

        private FontIcon HeaderIcon { get; set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ButtonControl is not null)
            {
                ButtonControl.Click -= ButtonControl_Click;
            }

            if (GetTemplateChild(nameof(ButtonControl)) is Button button)
            {
                ButtonControl = button;
                ButtonControl.Click += ButtonControl_Click;
            }

            if (GetTemplateChild(nameof(HeaderIcon)) is FontIcon fontIcon)
            {
                HeaderIcon = fontIcon;
                HeaderIcon.FontFamily = this.IconFontFamily;
            }
        }

        private void ButtonControl_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public FontFamily IconFontFamily
        {
            get { return (FontFamily)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(InboxButton), new PropertyMetadata((FontFamily)App.Current.Resources["SymbolThemeFontFamily"]));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));

        public string ActionIcon
        {
            get { return (string)GetValue(ActionIconProperty); }
            set { SetValue(ActionIconProperty, value); }
        }

        public static readonly DependencyProperty ActionIconProperty = DependencyProperty.Register("ActionIcon", typeof(string), typeof(InboxButton), new PropertyMetadata(string.Empty));
    }

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        //Class to store settings so they can be always spelled correctly
        public static class Settings
        {
            public const string StartupPage = "StartupPage";
            public const string HomeEnabled = "HomeEnabled";
            public const string OutlookEnabled = "OutlookEnabled";
            public const string ProtonEnabled = "ProtonEnabled";
            public const string iCloudEnabled = "iCloudEnabled";
            public const string GmailEnabled = "GmailEnabled";
            public const string YahooEnabled = "YahooEnabled";
            public const string CommandBarEnabled = "CommandBarEnabled";
            public const string OutlookAppType = "OutlookAppType";
            public const string ToDoServiceName = "ToDoServiceName";
            public const string CalendarServiceName = "CalendarServiceName";
            public const string ToDoServiceUrl = "ToDoServiceUrl";
            public const string CalendarServiceUrl = "CalendarServiceUrl";
            public const string OutlookExePath = "OutlookExePath";
            public const string Backdrop = "Backdrop";
            public const string PaneDisplayMode = "PaneDisplayMode";
            public const string FirstBootScreenPassed = "FirstBootScreenPassed";
            public const string VersionNumber = "VersionNumber";
            public const string WindowState = "WindowState";
            public static bool SettingsChangable { get; set; }
        }

        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //Sets the value of SettingsChangable so that settings aren't changable within the application
            Settings.SettingsChangable = false;

            //Settings checkers to check if settings exist
            if (!localSettings.Values.ContainsKey(Settings.HomeEnabled))
                localSettings.Values[Settings.HomeEnabled] = true;

            if (!localSettings.Values.ContainsKey(Settings.iCloudEnabled))
                localSettings.Values[Settings.iCloudEnabled] = false;

            if (!localSettings.Values.ContainsKey(Settings.GmailEnabled))
                localSettings.Values[Settings.GmailEnabled] = false;

            if (!localSettings.Values.ContainsKey(Settings.ProtonEnabled))
                localSettings.Values[Settings.ProtonEnabled] = false;

            if (!localSettings.Values.ContainsKey(Settings.YahooEnabled))
                localSettings.Values[Settings.YahooEnabled] = false;

            if (!localSettings.Values.ContainsKey(Settings.OutlookEnabled))
                localSettings.Values[Settings.OutlookEnabled] = false;

            if (!localSettings.Values.ContainsKey(Settings.CommandBarEnabled))
                localSettings.Values[Settings.CommandBarEnabled] = true;

            if (!localSettings.Values.ContainsKey(Settings.OutlookAppType))
                localSettings.Values[Settings.OutlookAppType] = "Website";

            if (!localSettings.Values.ContainsKey(Settings.ToDoServiceUrl))
                localSettings.Values[Settings.ToDoServiceUrl] = "disabled";

            if (!localSettings.Values.ContainsKey(Settings.ToDoServiceName))
                localSettings.Values[Settings.ToDoServiceName] = "Disabled";

            if (!localSettings.Values.ContainsKey(Settings.CalendarServiceUrl))
                localSettings.Values[Settings.CalendarServiceUrl] = "disabled";

            if (!localSettings.Values.ContainsKey(Settings.CalendarServiceName))
                localSettings.Values[Settings.CalendarServiceName] = "Disabled";

            if (!localSettings.Values.ContainsKey(Settings.StartupPage))
                localSettings.Values[Settings.StartupPage] = "Home";

            if (!localSettings.Values.ContainsKey(Settings.OutlookExePath))
                localSettings.Values[Settings.OutlookExePath] = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";

            if (!localSettings.Values.ContainsKey(Settings.Backdrop))
                localSettings.Values[Settings.Backdrop] = "Mica";

            if (!localSettings.Values.ContainsKey(Settings.FirstBootScreenPassed))
                localSettings.Values[Settings.FirstBootScreenPassed] = false;

            if (!localSettings.Values.ContainsKey(Settings.PaneDisplayMode))
                localSettings.Values[Settings.PaneDisplayMode] = "Auto";

            if (!localSettings.Values.ContainsKey(Settings.WindowState))
                localSettings.Values[Settings.WindowState] = OverlappedPresenterState.Restored.ToString();

            //Backwards compatibility code that updates the values of Home, iCloud, Gmail, Proton, & Outlook Enabled from strings to booleans & sets YahooEnabled and FirstBootScreenPassed to true
            if (localSettings.Values[Settings.HomeEnabled] is string)
            {
                localSettings.Values[Settings.HomeEnabled] = (string)localSettings.Values[Settings.HomeEnabled] == "True";
                localSettings.Values[Settings.iCloudEnabled] = (string)localSettings.Values[Settings.iCloudEnabled] == "True";
                localSettings.Values[Settings.GmailEnabled] = (string)localSettings.Values[Settings.GmailEnabled] == "True";
                localSettings.Values[Settings.ProtonEnabled] = (string)localSettings.Values[Settings.ProtonEnabled] == "True";
                localSettings.Values[Settings.OutlookEnabled] = (string)localSettings.Values[Settings.OutlookEnabled] == "True";
                localSettings.Values[Settings.FirstBootScreenPassed] = localSettings.Values[Settings.YahooEnabled] = true;
            }

            if (!((string)localSettings.Values[Settings.VersionNumber] == "1.3"))
                localSettings.Values[Settings.VersionNumber] = "1.3";

            //Checks for value of FirstBootScreenPassed
            if ((bool)localSettings.Values[Settings.FirstBootScreenPassed])
            {
                //If true, it creates & activates the MainWindow
                m_window = new MainWindow();
                m_window.Activate();
            }
            else
            {
                //If not, it DllImports a method to get the window Dpi
                [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
                static extern uint GetDpiForWindow(IntPtr hwnd);

                //Then it creates and activates FirstBootWindow
                firstBootWindow = new FirstBootWindow();

                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(firstBootWindow);
                Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
                Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                if (appWindow is not null)
                {
                    double scale;
                    uint dpi = GetDpiForWindow(hWnd);
                    switch (dpi)
                    {
                        case 90:
                            scale = 1;
                            break;
                        case 120:
                            scale = 1.25;
                            break;
                        case 144:
                            scale = 1.5;
                            break;
                        case 168:
                            scale = 1.75;
                            break;
                        case 192:
                            scale = 2;
                            break;
                        case 216:
                            scale = 2.25;
                            break;
                        case 240:
                            scale = 2.5;
                            break;
                        case 288:
                            scale = 3;
                            break;
                        default:
                            scale = (double)dpi / 100;
                            break;
                    }
                    var newSize = new SizeInt32();
                    newSize.Width = (int)(960 * scale);
                    newSize.Height = (int)(648 * scale);
                    appWindow.Resize(newSize);

                    Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                    if (displayArea is not null)
                    {
                        var CenteredPosition = appWindow.Position;
                        CenteredPosition.X = ((displayArea.WorkArea.Width - appWindow.Size.Width) / 2);
                        CenteredPosition.Y = ((displayArea.WorkArea.Height - appWindow.Size.Height) / 2);
                        appWindow.Move(CenteredPosition);
                    }
                }
                firstBootWindow.Activate();
            }
        }

        public Window m_window;

        public Window firstBootWindow;
    }
}
