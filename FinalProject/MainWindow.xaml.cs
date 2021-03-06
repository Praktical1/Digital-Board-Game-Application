using FinalProject.Model;
using FinalProject.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings setting { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.setting = new Settings();
            if (File.Exists(@"Settings.txt"))
            {
                String[] settingsFile = System.IO.File.ReadAllLines(@"Settings.txt");
                String[] settingsParts = settingsFile[0].Split(" ");
                setting.background = settingsParts[0];
                setting.userId = settingsParts[1];
                Background.Source = new BitmapImage(new Uri(@setting.background, UriKind.Relative));
            };
            Main.Content = new MainMenu(setting);
        }
        void CloseGame(object sender, CancelEventArgs e)
        {
            var a = Main.Content as Page;
            Main.Content.GetType().GetMethod("Close");
        }
    }
}
