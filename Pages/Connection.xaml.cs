// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Noobfish_Operations_Center.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Connection : Page
    {
        private readonly DispatcherTimer timer;
        private Process p3d_ngxu_process;
        private Process msfs_ng3_process;
        public Connection()
        {
            this.InitializeComponent();
            // 创建一个1秒的DispatcherTimer对象
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);

            timer.Tick += (sender, e) =>
            {
                CheckProcessStatus();
            };

            timer.Start();
        }

        private void CheckProcessStatus()
        {
            Process[] p4d_run = Process.GetProcessesByName("p4d");
            Process[] msfs_run = Process.GetProcessesByName("msfs");

            if (p4d_run.Length > 0)
            {
                P3DConnectionStatus.Text = "P3D connection status : Connected";
            }
            else
            {
                P3DConnectionStatus.Text = "P3D connection status : Disonnected";
            }
            if (msfs_run.Length > 0)
            {
                MSFSConnectionStatus.Text = "MSFS connection status : Connected";
            }
            else
            {
                MSFSConnectionStatus.Text = "MSFS connection status : Disonnected";
            }
        }

        private async void connect_to_p4d(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "Connections\\p4d.exe";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            p3d_ngxu_process = new Process();
            p3d_ngxu_process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            p3d_ngxu_process.StartInfo = startInfo;

            p3d_ngxu_process.Start();

            await p3d_ngxu_process.WaitForExitAsync();
        }

        private async void connect_to_msfs(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "Connections\\msfs.exe";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            msfs_ng3_process = new Process();
            msfs_ng3_process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            msfs_ng3_process.StartInfo = startInfo;

            msfs_ng3_process.Start();

            await msfs_ng3_process.WaitForExitAsync();
        }

        private void stop_p4d(object sender, RoutedEventArgs e)
        {
            if (p3d_ngxu_process != null && !p3d_ngxu_process.HasExited)
            {
                p3d_ngxu_process.Kill();
            }
        }

        private void stop_msfs(object sender, RoutedEventArgs e)
        {
            if (msfs_ng3_process != null && !msfs_ng3_process.HasExited)
            {
                msfs_ng3_process.Kill();
            }
        }

        /*
        private void StartProcessButton_Click(object sender, RoutedEventArgs e)
        {
            
            new task(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "Connections\\p4d.exe";
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                Process process = new Process();
                process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                process.StartInfo = startInfo;
                process.OutputDataReceived += Process_OutputDataReceived;
                process.Start();

                process.BeginOutputReadLine();
            });
            task.start;
            return Task;
            
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            
            if (!string.IsNullOrEmpty(e.Data))
            {
                OutputTextBlock.DispatcherQueue.TryEnqueue(() =>
                {
                    OutputTextBlock.Text += e.Data + Environment.NewLine;
                });
            }
            
        }*/
    }  
}
