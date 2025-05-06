namespace Email_Inboxes.Settings;

public class SettingValues
{
    #region Services

    public static Setting<bool> HomeEnabled = new(nameof(HomeEnabled), true);

    public static Setting<bool> GmailEnabled = new(nameof(GmailEnabled), false);

    public static Setting<bool> iCloudEnabled = new(nameof(iCloudEnabled), false);

    public static Setting<bool> ProtonEnabled = new(nameof(ProtonEnabled), false);

    public static Setting<bool> YahooEnabled = new(nameof(YahooEnabled), false);

    public static Setting<bool> OutlookEnabled = new(nameof(OutlookEnabled), false);

    public static EnumSetting<OutlookType> OutlookAppType = new(nameof(OutlookAppType), OutlookType.Website);

    public static Setting<string> OutlookExePath = new(nameof(OutlookExePath), @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE");

    #endregion

    #region Home

    public static EnumSetting<CalendarService> CalendarService = new(nameof(CalendarService), Settings.CalendarService.None);

    public static EnumSetting<ToDoService> ToDoService = new(nameof(ToDoService), Settings.ToDoService.None);

    public static Setting<bool> CommandBarEnabled = new(nameof(CommandBarEnabled), true);

    #endregion

    #region Appearance

    public static EnumSetting<PageType> StartupPage = new(nameof(StartupPage), PageType.Home);

    public static EnumSetting<BackdropType> Backdrop = new(nameof(Backdrop), BackdropType.Mica);

    public static EnumSetting<NavigationViewPaneDisplayMode> PaneDisplayMode = new(nameof(PaneDisplayMode), NavigationViewPaneDisplayMode.Auto);

    #endregion

    #region States

    public static Setting<bool> FirstBootScreenPassed = new(nameof(FirstBootScreenPassed), false);

    public static Setting<OverlappedPresenterState> WindowState = new(nameof(WindowState), OverlappedPresenterState.Restored);

    #endregion
}
