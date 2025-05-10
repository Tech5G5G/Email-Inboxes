namespace System.Windows.Web;

public class WebViewEnvironment
{
    internal CoreWebView2Environment Environment => env;
    readonly CoreWebView2Environment env;

    internal WebViewEnvironment(CoreWebView2Environment env) => this.env = env;

    public WebViewControllerOptions CreateControllerOptions() => new(env.CreateCoreWebView2ControllerOptions());

    public async static Task<WebViewEnvironment> CreateAsync() => new(await CoreWebView2Environment.CreateAsync());
}
