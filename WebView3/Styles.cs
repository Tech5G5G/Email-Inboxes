namespace WebView3;

[Flags]
internal enum WindowStyle
{
    Border = 0x800000,
    Caption = 0xC00000,
    Child = 0x40000000,
    ChildWindow = Child,
    ClipChildren = 0x2000000,
    ClipSiblings = 0x4000000,
    Disabled = 0x8000000,
    DLGFrame = 0x400000,
    Group = 0x20000,
    HScroll = 0x100000,
    Iconic = 0x20000000,
    Maximize = 0x1000000,
    MaximizeBox = 0x10000,
    Minimize = Iconic,
    MinimizeBox = Group,
    Overlapped = 0x0,
    OverlappedWindow = Overlapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox,
    Popup = int.MinValue,
    PopupWindow = Popup | Border | SysMenu,
    SizeBox = 0x40000,
    SysMenu = 0x80000,
    TabStop = MaximizeBox,
    ThickFrame = SizeBox,
    Tiled = Overlapped,
    TiledWindow = OverlappedWindow,
    Visible = 0x10000000,
    VScroll = 0x200000
}

[Flags]
internal enum ExtendedWindowStyle
{
    AcceptFiles = 0x10,
    AppWindow = 0x40000,
    ClientEdge = 0x200,
    Composited = 0x2000000,
    ContextHelp = 0x400,
    ControlParent = 0x10000,
    DLGModalFrame = 0x1,
    Layered = 0x80000,
    LayoutRTL = 0x400000,
    Left = 0x0,
    LeftScrollBar = 0x4000,
    LTRReading = Left,
    MDIChild = 0x40,
    NoActivate = 0x8000000,
    NoInheritLayout = 0x100000,
    NoParentNotify = 0x4,
    NoRedirectionBitmap = 0x200000,
    OverlappedWindow = WindowEdge | ClientEdge,
    PaletteWindow = WindowEdge | ToolWindow | TopMost,
    Right = 0x1000,
    RightScrollBar = Left,
    RTLReading = 0x2000,
    StaticEdge = 0x20000,
    ToolWindow = 0x80,
    TopMost = 0x8,
    Transparent = 0x20,
    WindowEdge = 0x100
}
