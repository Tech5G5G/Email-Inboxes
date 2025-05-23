using Microsoft.UI.Composition.SystemBackdrops;

namespace Email_Inboxes.Settings
{
    public static class ValueConverters
    {
        public static SystemBackdrop ToSystemBackdrop(this BackdropType type) => type switch
        {
            //BackdropType.MicaAlt => new Media.MicaBackdrop { Kind = MicaKind.BaseAlt },
            BackdropType.Acrylic => new AcrylicBackdrop(),
            BackdropType.Transparent => new WinUIEx.TransparentTintBackdrop(),
            _ => new Media.MicaBackdrop { Kind = MicaKind.Base }
        };

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

        //Update code in SettingValues static constructor
        //Use VersionNumber setting (check if doesn't exist, equal to "1.3" or 1.4f)

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
