namespace System.Windows.Web;

public class WebViewControllerOptions
{
    internal CoreWebView2ControllerOptions Options => options;
    readonly CoreWebView2ControllerOptions options;

    internal WebViewControllerOptions(CoreWebView2ControllerOptions options) => this.options = options;

    public bool InPrivate
    {
        get => options.IsInPrivateModeEnabled;
        set => options.IsInPrivateModeEnabled = value;
    }

    public string ScriptLocale
    {
        get => options.ScriptLocale;
        set => options.ScriptLocale = value;
    }

    public string ProfileName
    {
        get => options.ProfileName;
        set => options.ProfileName = value;
    }
}
