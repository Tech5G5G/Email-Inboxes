using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Email_Inboxes.Settings
{
    public class SettingValues
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static PageType StartupPage
        {
            get
            {
                if (localSettings.Values.TryGetValue("StartupPage", out object value))
                    return Enum.Parse<PageType>((string)value);
                else
                {
                    localSettings.Values["StartupPage"] = PageType.Home;
                    return PageType.Home;
                }
            }
            set
            {
                localSettings.Values["StartupPage"] = value.ToString();
                StartupPageChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler StartupPageChanged;

        public static bool HomeEnabled
        {
            get
            {
                if (localSettings.Values.TryGetValue("HomeEnabled", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["HomeEnabled"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["HomeEnabled"] = value.ToString();
                HomeEnabledChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler HomeEnabledChanged;

        public static bool OutlookEnabled
        {
            get
            {
                if (localSettings.Values.TryGetValue("OutlookEnabled", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["OutlookEnabled"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["OutlookEnabled"] = value.ToString();
                OutlookEnabledChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler OutlookEnabledChanged;

        public static bool ProtonEnabled
        {
            get
            {
                if (localSettings.Values.TryGetValue("ProtonEnabled", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["ProtonEnabled"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["ProtonEnabled"] = value.ToString();
                ProtonEnabledChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler ProtonEnabledChanged;

        public static bool iCloudEnabled
        {
            get
            {
                if (localSettings.Values.TryGetValue("iCloudEnabled", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["iCloudEnabled"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["iCloudEnabled"] = value.ToString();
                iCloudEnabledChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler iCloudEnabledChanged;

        public static bool GmailEnabled
        {
            get
            {
                if (localSettings.Values.TryGetValue("GmailEnabled", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["GmailEnabled"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["GmailEnabled"] = value.ToString();
                GmailEnabledChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler GmailEnabledChanged;

        public static bool YahooEnabled
        {
            get
            {
                if (localSettings.Values.TryGetValue("YahooEnabled", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["YahooEnabled"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["YahooEnabled"] = value.ToString();
                YahooEnabledChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler YahooEnabledChanged;

        public static bool CommandBarEnabled
        {
            get
            {
                if (localSettings.Values.TryGetValue("CommandBarEnabled", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["CommandBarEnabled"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["CommandBarEnabled"] = value.ToString();
                CommandBarEnabledChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler CommandBarEnabledChanged;

        public static OutlookType OutlookAppType
        {
            get
            {
                if (localSettings.Values.TryGetValue("OutlookAppType", out object value))
                    return Enum.Parse<OutlookType>((string)value);
                else
                {
                    localSettings.Values["OutlookAppType"] = OutlookType.Website.ToString();
                    return OutlookType.Website;
                }
            }
            set
            {
                localSettings.Values["OutlookAppType"] = value.ToString();
                OutlookAppTypeChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler OutlookAppTypeChanged;

        public static CalendarService CalendarService
        {
            get
            {
                if (localSettings.Values.TryGetValue("CalendarService", out object value))
                    return Enum.Parse<CalendarService>((string)value);
                else
                {
                    localSettings.Values["CalendarService"] = CalendarService.None.ToString();
                    return CalendarService.None;
                }
            }
            set
            {
                localSettings.Values["CalendarService"] = value.ToString();
                CalendarServiceChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler CalendarServiceChanged;

        public static ToDoService ToDoService
        {
            get
            {
                if (localSettings.Values.TryGetValue("ToDoService", out object value))
                    return Enum.Parse<ToDoService>((string)value);
                else
                {
                    localSettings.Values["ToDoService"] = ToDoService.None.ToString();
                    return ToDoService.None;
                }
            }
            set
            {
                localSettings.Values["ToDoService"] = value.ToString();
                ToDoServiceChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler ToDoServiceChanged;

        public static string OutlookExePath
        {
            get
            {
                if (localSettings.Values.TryGetValue("OutlookExePath", out object value))
                    return (string)value;
                else
                {
                    localSettings.Values["OutlookExePath"] = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";
                    return @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";
                }
            }
            set
            {
                localSettings.Values["OutlookExePath"] = value.ToString();
                OutlookExePathChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler OutlookExePathChanged;

        public static BackdropType Backdrop
        {
            get
            {
                if (localSettings.Values.TryGetValue("Backdrop", out object value))
                    return Enum.Parse<BackdropType>((string)value);
                else
                {
                    localSettings.Values["Backdrop"] = BackdropType.Mica.ToString();
                    return BackdropType.Mica;
                }
            }
            set
            {
                localSettings.Values["Backdrop"] = value.ToString();
                BackdropChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler BackdropChanged;

        public static NavigationViewPaneDisplayMode PaneDisplayMode
        {
            get
            {
                if (localSettings.Values.TryGetValue("PaneDisplayMode", out object value))
                    return Enum.Parse<NavigationViewPaneDisplayMode>((string)value);
                else
                {
                    localSettings.Values["PaneDisplayMode"] = NavigationViewPaneDisplayMode.Auto.ToString();
                    return NavigationViewPaneDisplayMode.Auto;
                }
            }
            set
            {
                localSettings.Values["PaneDisplayMode"] = value.ToString();
                PaneDisplayModeChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler PaneDisplayModeChanged;

        public static bool FirstBootScreenPassed
        {
            get
            {
                if (localSettings.Values.TryGetValue("FirstBootScreenPassed", out object value))
                    return (bool)value;
                else
                {
                    localSettings.Values["FirstBootScreenPassed"] = true;
                    return true;
                }
            }
            set
            {
                localSettings.Values["FirstBootScreenPassed"] = value.ToString();
                FirstBootScreenPassedChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler FirstBootScreenPassedChanged;

        public static OverlappedPresenterState WindowState
        {
            get
            {
                if (localSettings.Values.TryGetValue("WindowState", out object value))
                    return Enum.Parse<OverlappedPresenterState>((string)value);
                else
                {
                    localSettings.Values["WindowState"] = OverlappedPresenterState.Restored.ToString();
                    return OverlappedPresenterState.Restored;
                }
            }
            set
            {
                localSettings.Values["WindowState"] = value.ToString();
                WindowStateChanged?.Invoke(new SettingChangedEventArgs(value));
            }
        }
        public static event SettingChangedEventHandler WindowStateChanged;
    }
}
