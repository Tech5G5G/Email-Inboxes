namespace Email_Inboxes.Settings;

public class SettingValues
{
    #region Services

    public static Setting<bool> GmailEnabled { get; } = new(nameof(GmailEnabled), false);

    public static Setting<bool> iCloudEnabled { get; } = new(nameof(iCloudEnabled), false);

    public static Setting<bool> ProtonEnabled { get; } = new(nameof(ProtonEnabled), false);

    public static Setting<bool> YahooEnabled { get; } = new(nameof(YahooEnabled), false);

    public static Setting<bool> OutlookEnabled { get; } = new(nameof(OutlookEnabled), false);

    public static EnumSetting<OutlookType> OutlookAppType { get; } = new(nameof(OutlookAppType), OutlookType.Website);

    public static Setting<string> OutlookExePath { get; } = new(nameof(OutlookExePath), @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE");

    #endregion

    #region Home

    public static Setting<bool> HomeEnabled { get; } = new(nameof(HomeEnabled), true);

    public static EnumSetting<CalendarService> CalendarService { get; } = new(nameof(CalendarService), Settings.CalendarService.None);

    public static EnumSetting<ToDoService> ToDoService { get; } = new(nameof(ToDoService), Settings.ToDoService.None);

    #endregion

    #region Appearance

    public static EnumSetting<PageType> StartupPage { get; } = new(nameof(StartupPage), PageType.Home);

    public static EnumSetting<NavigationViewPaneDisplayMode> PaneDisplayMode { get; } = new(nameof(PaneDisplayMode), NavigationViewPaneDisplayMode.Auto);

    #endregion

    #region Backdrop

    public static EnumSetting<BackdropType> Backdrop { get; } = new(nameof(Backdrop), BackdropType.Mica);

    public static Setting<bool> ExtendBackdrop { get; } = new(nameof(ExtendBackdrop), false);

    public static Setting<int> BackdropKind { get; } = new(nameof(BackdropKind), default);

    public static Setting<string> BackdropTint { get; } = new(nameof(BackdropTint), "#00000000");

    public static Setting<float> BackdropLuminosity { get; } = new(nameof(BackdropLuminosity), default);

    #endregion

    #region States

    public static Setting<bool> FirstBootScreenPassed { get; } = new(nameof(FirstBootScreenPassed), false);

    public static EnumSetting<OverlappedPresenterState> WindowState { get; } = new(nameof(WindowState), OverlappedPresenterState.Restored);

    public static Setting<float> VersionNumber { get; } = new(nameof(VersionNumber), 1.4f);

    public static Setting<bool> CommandBarEnabled { get; } = new(nameof(CommandBarEnabled), true);

    #endregion
}
