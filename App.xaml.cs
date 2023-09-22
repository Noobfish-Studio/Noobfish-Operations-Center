using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT;
using WinUI3NavigationExample.Helpers;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Resources;
using static Noobfish_Operations_Center.Pages.Setting;
using System.Diagnostics;
using System.Security.Principal;

using System.Windows;
using System.Drawing;
using Microsoft.UI.Composition.SystemBackdrops;

namespace Noobfish_Operations_Center
{

    public partial class App : Application
    {
        private WindowsSystemDispatcherQueueHelper m_wsqdHelper;
        private Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
        private Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;
        private AppWindow appWindow;

        public App()
        {
            ChangeStartingLanguage();
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            if (AppWindowTitleBar.IsCustomizationSupported()) //Run only on Windows 11
            {
                m_window.SizeChanged += SizeChanged; //Register handler for setting draggable rects

                appWindow = GetAppWindow(m_window); //Set ExtendsContentIntoTitleBar for the AppWindow not the window
                appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
                appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
            TrySetMicaBackdrop();
            m_window.Activate();
        }

        private void ChangeStartingLanguage()
        {
            string filePath = "C:\\ProgramData\\Noobfish Operations Center\\Settings\\language.txt"; // 文件路径
            string content = "";
            if(File.Exists(filePath)) 
            { 
                using (StreamReader sr = new StreamReader(filePath))
                {
                    content = sr.ReadToEnd();
                }
            }
            else
            {
                content = "en-US";
            }
            var culture = new System.Globalization.CultureInfo(content);

            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.TwoLetterISOLanguageName;
        }

        private void ChangeStartingBgStyle()
        {
            string filePath = "C:\\ProgramData\\Noobfish Operations Center\\Settings\\language.txt"; // 文件路径
            string content = "";
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    content = sr.ReadToEnd();
                }
            }
            else
            {
                content = "en-US";
            }
            var culture = new System.Globalization.CultureInfo(content);

            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.TwoLetterISOLanguageName;
        }

        private void SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            //Update the title bar draggable region. We need to indent from the left both for the nav back button and to avoid the system menu
            Windows.Graphics.RectInt32[] rects = new Windows.Graphics.RectInt32[] { new Windows.Graphics.RectInt32(48, 0, (int)args.Size.Width - 48, 48) };
            appWindow.TitleBar.SetDragRectangles(rects);
        }

        private AppWindow GetAppWindow(Window window)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            return AppWindow.GetFromWindowId(windowId);
        }

        bool TrySetMicaBackdrop()
        {
            string filePath = "C:\\ProgramData\\Noobfish Operations Center\\Settings\\bgstyle.txt"; // 文件路径
            string content = "";
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    content = sr.ReadToEnd();
                }
            }
            else
            {
                content = "mica";
            }

            if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported()&&content!="none")
            {
                m_wsqdHelper = new WindowsSystemDispatcherQueueHelper();
                m_wsqdHelper.EnsureWindowsSystemDispatcherQueueController();

                // Hooking up the policy object
                m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
                m_window.Activated += Window_Activated;
                m_window.Closed += Window_Closed;
                ((FrameworkElement)m_window.Content).ActualThemeChanged += Window_ThemeChanged;

                // Initial configuration state.
                m_configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                if (content=="mica_alt") 
                {
                    m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController()
                    {
                        Kind = MicaKind.BaseAlt
                    };
                }
                else
                {
                    m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();
                }

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                m_micaController.AddSystemBackdropTarget(m_window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
            // use this closed window.
            if (m_micaController != null)
            {
                m_micaController.Dispose();
                m_micaController = null;
            }
            m_window.Activated -= Window_Activated;
            m_configurationSource = null;
        }

        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            if (m_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }

        private void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)m_window.Content).ActualTheme)
            {
                case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
                case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
            }
        }

        private Window m_window;
    }
}
