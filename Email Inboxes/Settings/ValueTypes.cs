using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Inboxes.Settings
{
    public delegate void SettingChangedEventHandler(SettingChangedEventArgs args);

    public class SettingChangedEventArgs(object newValue)
    {
        public object NewValue { get; } = newValue;
    };

    public enum PageType
    {
        Home,
        Outlook,
        Gmail,
        Yahoo,
        iCloud,
        Proton
    }

    public enum OutlookType
    {
        Website,
        BusinessWebsite,
        Desktop
    }

    public enum CalendarService
    {
        GoogleCalendar,
        Outlook,
        AppleCalendar,
        Basic,
        None
    }

    public enum ToDoService
    {
        Todoist,
        TickTick,
        MSToDo,
        AppleReminders,
        GoogleTasks,
        AnyDo,
        None
    }

    public enum BackdropType
    {
        Mica,
        MicaAlt,
        Acrylic
    }
}
