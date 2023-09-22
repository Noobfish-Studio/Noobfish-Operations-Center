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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Windows.ApplicationModel.Resources;
using Windows.Storage;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Noobfish_Operations_Center.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Setting : Page
    {
        public Setting()
        {
            this.InitializeComponent();
            language_init();
            bgstyle_init();
        }

        private void language_init()
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
            if (content == "zh-CN")
            {
                LanguageRadioButtons.SelectedIndex = 1;
            }
            else
            {
                LanguageRadioButtons.SelectedIndex = 0;
            }
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Directory.CreateDirectory("C:\\ProgramData\\Noobfish Operations Center\\Settings");
            string filePath = "C:\\ProgramData\\Noobfish Operations Center\\Settings\\language.txt"; // 文件路径
            string content = "";
            if (sender is RadioButtons rb)
            {
                string language = rb.SelectedItem as string;
                switch (language)
                {
                    case "en-US":
                        content = "en-US";
                        break;
                    case "zh-CN":
                        content = "zh-CN";
                        break;
                }
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(content);
                }
            }
        }

        private void bgstyle_init()
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
            switch(content)
            {
                case "mica":
                    BgStyleRadioButtons.SelectedIndex = 0; break;
                case "mica_alt":    
                    BgStyleRadioButtons.SelectedIndex = 1; break;
                case "none":        
                    BgStyleRadioButtons.SelectedIndex = 2; break;
                default:            
                    BgStyleRadioButtons.SelectedIndex = 0; break;
            }
        }

        private void BgStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Directory.CreateDirectory("C:\\ProgramData\\Noobfish Operations Center\\Settings");
            string filePath = "C:\\ProgramData\\Noobfish Operations Center\\Settings\\bgstyle.txt"; // 文件路径
            string content = "";
            if (sender is RadioButtons rb)
            {
                string bgstyle = rb.SelectedItem as string;
                switch (bgstyle)
                {
                    case "Mica":
                        content = "mica";
                        break;
                    case "Mica Alt":
                        content = "mica_alt";
                        break;
                    case "None":
                        content = "none";
                        break;
                }
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(content);
                }
            }
        }
    }
}
