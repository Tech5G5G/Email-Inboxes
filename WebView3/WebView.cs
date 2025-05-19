using System.Threading.Tasks;

namespace WebView3;

/// <summary>
/// Hosts a <see cref="CoreWebView2Controller"/> within a WinUI 3 application.
/// </summary>
public sealed partial class WebView : WindowHost
{
    #region PInvoke

    [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr CreateWindowEx(ExtendedWindowStyle dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, WindowStyle dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("User32.dll")]
    private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("User32.dll")]
    private static extern long SetWindowLongPtr(IntPtr hWnd, int nIndex, long dwNewLong);

    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern long GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

    const int GWL_STYLE = -16;

    const int GW_CHILD = 5;

    #endregion

    #region WebView Properties

    public Uri Source
    {
        get => source;
        set
        {
            source = value;
            EnsureController();
            controller?.CoreWebView2.Navigate(value.ToString());
        }
    }
    private Uri source;

    public new int CornerRadius
    {
        get => radius;
        set
        {
            radius = value;
            UpdateSize();
        }
    }
    int radius = 0;

    public new Color Background
    {
        get => backgroundColor;
        set
        {
            backgroundColor = value;
            if (controller is not null)
                controller.DefaultBackgroundColor = value;
        }
    }
    private Color backgroundColor;

    public double ZoomFactor
    {
        get => zoomFactor;
        set
        {
            zoomFactor = value;
            if (controller is not null)
                controller.ZoomFactor = value;
        }
    }
    private double zoomFactor = 1;

    public bool AllowExternalDrop
    {
        get => allowExternalDrop;
        set
        {
            allowExternalDrop = value;
            if (controller is not null)
                controller.AllowExternalDrop = value;
        }
    }
    private bool allowExternalDrop;

    public CoreWebView2Environment Environment
    {
        get => environment;
        set => environment ??= value;
    }
    CoreWebView2Environment environment;

    public CoreWebView2ControllerOptions ControllerOptions
    {
        get => options;
        set => options ??= value;
    }
    CoreWebView2ControllerOptions options;

    public CoreWebView2Controller Controller => controller;
    CoreWebView2Controller controller;

    public CoreWebView2 CoreWebView => controller?.CoreWebView2;

    public bool CanGoBack => controller?.CoreWebView2.CanGoBack ?? false;

    public bool CanGoForward => controller?.CoreWebView2.CanGoForward ?? false;

    #endregion

    #region WebView Events

    public event TypedEventHandler<CoreWebView2, CoreWebView2SourceChangedEventArgs> SourceChanged;
    public event TypedEventHandler<CoreWebView2, CoreWebView2NavigationStartingEventArgs> NavigationStarting;
    public event TypedEventHandler<CoreWebView2, CoreWebView2NavigationCompletedEventArgs> NavigationCompleted;
    public event TypedEventHandler<CoreWebView2, CoreWebView2ContentLoadingEventArgs> ContentLoading;
    public event TypedEventHandler<CoreWebView2, CoreWebView2WebMessageReceivedEventArgs> WebMessageReceived;

    public event TypedEventHandler<WebView, CoreWebView2Controller> ControllerInitialized;
    public event TypedEventHandler<CoreWebView2Controller, double> ZoomFactorChanged;

    private void HookCoreWebViewEvents(CoreWebView2 core)
    {
        core.SourceChanged += (s, e) => SourceChanged?.Invoke(s, e);
        core.NavigationStarting += (s, e) => NavigationStarting?.Invoke(s, e);
        core.NavigationCompleted += (s, e) => NavigationCompleted?.Invoke(s, e);
        core.ContentLoading += (s, e) => ContentLoading?.Invoke(s, e);
        core.WebMessageReceived += (s, e) => WebMessageReceived?.Invoke(s, e);

        controller.ZoomFactorChanged += (s, e) => ZoomFactorChanged?.Invoke(s, s.ZoomFactor);


        core.ProcessFailed += (s, e) => System.Diagnostics.Debug.WriteLine($"Failed due to reason: {e.Reason} ({e.ProcessFailedKind}, {e.ExitCode})");
    }

    #endregion

    readonly nint hWnd;
    nint controllerHandle;

    public WebView()
    {
        hWnd = CreateWindowEx(
            ExtendedWindowStyle.Layered |
            ExtendedWindowStyle.ToolWindow,
            "static", string.Empty,
            WindowStyle.Visible |
            WindowStyle.ClipSiblings,
            0, 0, int.MaxValue, int.MaxValue, //Make window take up entire screen
            IntPtr.Zero, IntPtr.Zero, Marshal.GetHINSTANCE(typeof(WebView).Module), IntPtr.Zero);
        ResizeWindow(hWnd, new()); //Reset window dimensions to zero
    }

    protected override nint WindowHandleRequested()
    {
        //Reparent the WebView controller window
        SetParent(hWnd, (nint)XamlRoot.ContentIslandEnvironment.AppWindowId.Value);

        //Add the WS_CHILD window style and remove the WS_CAPTION window style
        WindowStyle style = ((WindowStyle)GetWindowLongPtr(hWnd, GWL_STYLE) | WindowStyle.Child) & ~WindowStyle.Caption;
        SetWindowLongPtr(hWnd, GWL_STYLE, (long)style);

        return hWnd;
    }

    protected override void OnSizeChanged(nint hWnd, Rect rect, Rect viewportRect, double scale)
    {
        //Updates the WebView controller bounds
        if (controller is not null)
            controller.Bounds = new(0, 0, rect.Width, rect.Height);

        //Updates the Webview controller window region
        SetWindowRegion(controllerHandle, new(0, 0, rect.Width, rect.Height), scale, radius);

        base.OnSizeChanged(hWnd, rect, viewportRect, scale);
    }

    #region WebView Methods

    /// <summary>
    /// Ensures that <see cref="Controller"/> is created.
    /// </summary>
    /// <remarks>In order to provide <see cref="CoreWebView2Environment"/> and <see cref="CoreWebView2ControllerOptions"/>, set the <see cref="Environment"/> and <see cref="ControllerOptions"/> properties, preferably before the <see cref="Source"/> property.</remarks>
    public async void EnsureController() => await EnsureControllerAsync();

    /// <summary>
    /// Ensures that <see cref="Controller"/> is created, asynchronously.
    /// </summary>
    /// <remarks>In order to provide <see cref="CoreWebView2Environment"/> and <see cref="CoreWebView2ControllerOptions"/>, set the <see cref="Environment"/> and <see cref="ControllerOptions"/> properties, preferably before the <see cref="Source"/> property.</remarks>
    public async Task EnsureControllerAsync()
    {
        if (controller is not null)
            return;

        environment ??= await CoreWebView2Environment.CreateAsync();
        var reference = CoreWebView2ControllerWindowReference.CreateFromWindowHandle((ulong)hWnd);
        controller = options is null ? await environment.CreateCoreWebView2ControllerAsync(reference) :
            await environment.CreateCoreWebView2ControllerAsync(reference, options);

        controller.BoundsMode = CoreWebView2BoundsMode.UseRasterizationScale;
        if (source is not null)
            controller.CoreWebView2?.Navigate(source.ToString());

        controller.DefaultBackgroundColor = backgroundColor;
        controller.AllowExternalDrop = allowExternalDrop;
        controller.ZoomFactor = zoomFactor;

        controllerHandle = GetWindow(hWnd, GW_CHILD);
        HookCoreWebViewEvents(controller.CoreWebView2);
        ControllerInitialized?.Invoke(this, controller);
    }

    public override void Dispose()
    {
        controller?.Close();
        controller = null;

        base.Dispose();
    }

    public void GoBack() => controller?.CoreWebView2.GoBack();

    public void GoForward() => controller?.CoreWebView2.GoForward();

    public void Reload() => controller?.CoreWebView2.Reload();

    public void ExecuteScript(string javaScript) => controller?.CoreWebView2.ExecuteScriptAsync(javaScript);

    public async Task<string> ExecuteScriptAsync(string javaScript) => await controller?.CoreWebView2.ExecuteScriptAsync(javaScript);

    #endregion
}
