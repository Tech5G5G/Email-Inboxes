using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Animation;
using System.Threading.Tasks;
using Email_Inboxes.First_Boot_Window;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Email_Inboxes
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstBootWindow : Window
    {
        public FirstBootWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;

            var presenter = GetAppWindowAndPresenter();
            presenter.IsMaximizable = false;
            presenter.IsMinimizable = false;

            logoStart.Loaded += StartAnimation;

            firstBootFrame.Translation += new Vector3(0, 0, 32);
        }

        private OverlappedPresenter GetAppWindowAndPresenter()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var _apw = AppWindow.GetFromWindowId(myWndId);
            return _apw.Presenter as OverlappedPresenter;
        }

        private async void StartAnimation(object sender, RoutedEventArgs e)
        {
            await Task.Delay(50);
            logoMid.Visibility = Visibility.Visible;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("logoAnimation", logoStart);
            var logoAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logoAnimation");
            logoAnimation.Completed += LogoAnimationCompleted;
            logoAnimation.TryStart(logoMid);
            logoStart.Visibility = Visibility.Collapsed;
        }

        private async void LogoAnimationCompleted(ConnectedAnimation sender, object e)
        {
            await Task.Delay(2000);
            logoEnd.Visibility = Visibility.Visible;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("logoAnimation2", logoMid);
            var logoAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("logoAnimation2");
            logoAnimation.Completed += LogoAnimation2Completed;
            logoAnimation.TryStart(logoEnd);
            await Task.Delay(100); 
            logoMid.Visibility = Visibility.Collapsed;
            SetTitleBar(AppTitleBar);
        }

        private void LogoAnimation2Completed(ConnectedAnimation sender, object e)
        {
            firstBootFrame.Visibility = Visibility.Visible;
            firstBootFrame.Navigate(typeof(Welcome), null, null);
        }
    }
}
