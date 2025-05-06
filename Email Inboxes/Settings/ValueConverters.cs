using Microsoft.UI.Composition.SystemBackdrops;

namespace Email_Inboxes.Settings
{
    public static class ValueConverters
    {
        public static SystemBackdrop ToSystemBackdrop(this BackdropType type) => type switch
        {
            BackdropType.MicaAlt => new MicaBackdrop() { Kind = MicaKind.BaseAlt },
            BackdropType.Acrylic => new DesktopAcrylicBackdrop(),
            _ => new MicaBackdrop() { Kind = MicaKind.Base }
        };

        public static TitleBarSpacingResult FindSpacing(this NavigationViewPaneDisplayMode mode) => mode switch
        {
            NavigationViewPaneDisplayMode.Top => new TitleBarSpacingResult(new Thickness() { Top = 48 }, new Thickness() { Left = 16 }),
            _ => new TitleBarSpacingResult(new Thickness(), new Thickness { Left = 51 })
        };

        public readonly struct TitleBarSpacingResult(Thickness nvMargin, Thickness titleBarMargin)
        {
            public Thickness NavigationViewMargin { get; } = nvMargin;
            public Thickness TitleBarMargin { get; } = titleBarMargin;
        }

        public static ReadOnlyDictionary<CalendarService, string> CalendarServiceUris { get; } = new(new Dictionary<CalendarService, string>()
        {
            { CalendarService.GoogleCalendar, "https://calendar.google.com" },
            { CalendarService.Outlook, "https://outlook.live.com/calendar/" },
            { CalendarService.AppleCalendar, "https://www.icloud.com/calendar/" },
            { CalendarService.Basic, "Basic" },
            { CalendarService.None, "None" }
        });

        public static ReadOnlyDictionary<ToDoService, string> ToDoServiceUris { get; } = new(new Dictionary<ToDoService, string>()
        {
            { ToDoService.Todoist, "https://todoist.com" },
            { ToDoService.TickTick, "https://ticktick.com" },
            { ToDoService.MSToDo, "https://to-do.live.com" },
            { ToDoService.AppleReminders, "https://icloud.com/reminders" },
            { ToDoService.GoogleTasks, "https://tasks.google.com" },
            { ToDoService.AnyDo, "https://any.do" },
            { ToDoService.None, "None" }
        });

        //Update ToDoServiceName -> ToDoService
        //Values:
        //-Todoist
        //-TickTick
        //-Microsoft To Do
        //-Apple Reminders
        //-Google Tasks
        //-Any.do
        //-Disabled
        //-None

        //Update CalendarServiceName -> CalendarService
        //Values:
        //-Google Calendar
        //-Outlook Calendar
        //-Apple Calendar
        //-Basic Calendar
        //-Disabled
        //-None

        //Update PaneDisplayMode
        //-Auto
        //-Left
        //-Compact
        //-Minimal
        //-Top

        //Update OutlookAppType
        //-Website
        //-Business
        //-exe

        //CalendarService URIs
        //-https://www.icloud.com/calendar/
        //-https://outlook.live.com/calendar/
        //-https://calendar.google.com

        //ToDoService URIs
        //-https://icloud.com/reminders
        //-https://todoist.com
        //-https://ticktick.com
        //-https://to-do.live.com
        //-https://tasks.google.com
        //-https://any.do
    }
}
