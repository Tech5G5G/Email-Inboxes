using Windows.Graphics;

namespace WebView3;

/// <summary>
/// Hosts a window in a WinUI 3 application.
/// </summary>
public abstract partial class WindowHost : Control, IDisposable
{
    #region PInvoke

    [DllImport("User32.dll")]
    private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("User32.dll")]
    private static extern int DestroyWindow(IntPtr hWnd);

    [DllImport("User32.dll")]
    private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int cx, int cy);

    const uint SWP_ASYNCWINDOWPOS = 0x4000,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOACTIVATE = 0x0010,
        SWP_NOZORDER = 0x0004,
        SWP_NOSIZE = 0x0001,
        SWP_NOMOVE = 0x0002,
        SWP_SHOWWINDOW = 0x0040,
        SWP_HIDEWINDOW = 0x0080;

    #endregion

    public nint WindowHandle => handle;
    private IntPtr handle;

    private Rect viewport;

    public WindowHost()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        SizeChanged += (s, e) => OnSizeChanged(handle, new(GetOffset(), e.NewSize), viewport, XamlRoot.RasterizationScale);
        EffectiveViewportChanged += (s, e) =>
        {
            viewport = e.EffectiveViewport;
            UpdateSize();
        };
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        if (handle == IntPtr.Zero)
        {
            handle = WindowHandleRequested();
            UpdateSize();
        }

        _ = SetWindowPos(handle, IntPtr.Zero,
            0, 0, 0, 0,
            SWP_ASYNCWINDOWPOS | SWP_NOZORDER | SWP_NOCOPYBITS | SWP_NOACTIVATE | SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
    }

    private void OnUnloaded(object sender, RoutedEventArgs args)
    {
        _ = SetWindowPos(handle, IntPtr.Zero,
            0, 0, 0, 0,
            SWP_ASYNCWINDOWPOS | SWP_NOZORDER | SWP_NOCOPYBITS | SWP_NOACTIVATE | SWP_NOSIZE | SWP_NOMOVE | SWP_HIDEWINDOW);
    }

    private Point GetOffset()
    {
        var visual = TransformToVisual(null);
        visual.TryTransform(new(), out Point point);
        return point;
    }

    /// <summary>
    /// Invoked once the <see cref="WindowHost"/> has been loaded to get the handle of the window to host.
    /// </summary>
    /// <returns>The window handle of the window to host in the form of a <see cref="nint"/>.</returns>
    protected abstract nint WindowHandleRequested();

    /// <summary>
    /// Invoked when the size of the <see cref="WindowHost"/> changes and the hosted window needs to be resized.
    /// </summary>
    /// <param name="hWnd">The handle of the hosted window.</param>
    /// <param name="rect">The <see cref="Rect"/> of the <see cref="WindowHost"/>, containing its size and location relative to its parent <see cref="Window"/>.</param>
    /// <param name="viewportRect">The <see cref="Rect"/> of the <see cref="WindowHost"/> viewport; that is, the <see cref="Rect"/> of what is actually visible.</param>
    /// <param name="scale">The current rasterization scale.</param>
    /// <remarks>While this method already automatically resizes the hosted window, derived classes can override it to add or overwrite behaviors.</remarks>
    protected virtual void OnSizeChanged(IntPtr hWnd, Rect rect, Rect viewportRect, double scale)
    {
        int width = (int)(rect.Width * scale);
        int height = (int)(rect.Height * scale);

        int x = (int)(rect.X * scale);
        int y = (int)(rect.Y * scale) + 1;

        SetWindowRegion(hWnd, viewportRect, scale);
        _ = SetWindowPos(hWnd, IntPtr.Zero,
            x, y, width, height,
            SWP_ASYNCWINDOWPOS | SWP_NOZORDER | SWP_NOCOPYBITS | SWP_NOACTIVATE);
    }

    /// <summary>
    /// Disposes the <see cref="WindowHost"/>.
    /// </summary>
    /// <remarks>While this method already automatically destroys the hosted window, derived classes can override it to add or overwrite behaviors.</remarks>
    public virtual void Dispose()
    {
        _ = DestroyWindow(handle);
        GC.SuppressFinalize(this);
    }

    #region Utility Methods

    /// <summary>
    /// Forcibly invokes the <see cref="OnSizeChanged"/> method, resizing the hosted window.
    /// </summary>
    protected void UpdateSize()
    {
        if (XamlRoot is not null)
            OnSizeChanged(handle, new(GetOffset(), new Size(ActualWidth, ActualHeight)), viewport, XamlRoot.RasterizationScale);
    }

    /// <summary>
    /// A wrapper for the SetWindowPos function.
    /// </summary>
    /// <param name="hWnd">The handle of the window to resize.</param>
    /// <param name="rect">The resize dimensions.</param>
    protected static void ResizeWindow(nint hWnd, RectInt32 rect) => _ = SetWindowPos(hWnd, IntPtr.Zero,
        rect.X, rect.Y, rect.Width, rect.Height, SWP_ASYNCWINDOWPOS | SWP_NOZORDER | SWP_NOCOPYBITS | SWP_NOACTIVATE);

    /// <summary>
    /// Sets the region of a window.
    /// </summary>
    /// <param name="hWnd">The handle to the window to set the region of.</param>
    /// <param name="rect">The dimensions to use for the region.</param>
    /// <param name="scale">The current rasterization scale.</param>
    /// <param name="radius">The radius of the region corners.</param>
    protected static void SetWindowRegion(nint hWnd, Rect rect, double scale, int radius = 0)
    {
        int width = (int)(rect.Width * scale);
        int height = (int)(rect.Height * scale);

        int x = (int)(rect.X * scale);
        int y = (int)(rect.Y * scale);

        nint region = CreateRoundRectRgn(x, y, x + width + 1, y + height + 1, (int)(radius * scale), (int)(radius * scale));
        _ = SetWindowRgn(hWnd, region, true);
    }

    #endregion
}
