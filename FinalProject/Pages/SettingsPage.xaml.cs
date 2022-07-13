using FinalProject.Model;
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
using System.Windows.Navigation;
using System.IO;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace FinalProject.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private Settings setting;

        public SettingsPage(Settings setting)
        {
            this.setting = setting;
            InitializeComponent();
            if (setting.background == "../Images/CoolBackground.png")
            {
                Default.Background = new BrushConverter().ConvertFromString("#00FFFFFF") as SolidColorBrush;
                Cool.Background = Brushes.Green;
            }
            if (setting.forcedMove == true)
            {
                ForceCapture.Background = Brushes.Green;
            }
            Online.Content = "Online - Logged in as " + setting.userId;
        }
        private void BtnMainMenu(object sender, RoutedEventArgs x)
        {
            SaveSettings();
            NavigationService.Navigate(new MainMenu(setting));
        }

        private void ChangeBackgroundtoDefault(object sender, RoutedEventArgs e)
        {
            setting.background = "../Images/Background.png";
            MessageBox.Show("Please restart program to see changes to Background");
            SaveSettings();
            Default.Background = Brushes.Green;
            Cool.Background = new BrushConverter().ConvertFromString("#00FFFFFF") as SolidColorBrush;
        }

        private void ChangeBackgroundtoCool(object sender, RoutedEventArgs e)
        {
            setting.background = "../Images/CoolBackground.png";
            MessageBox.Show("Please restart program to see changes to Background");
            SaveSettings();
            Default.Background = new BrushConverter().ConvertFromString("#00FFFFFF") as SolidColorBrush;
            Cool.Background = Brushes.Green;
        }

        private void BtnRegister(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Register(setting));
        }

        private void BtnLogin(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login(setting));
        }

        private void BtnForcedCaptureRule(object sender, RoutedEventArgs e)
        {
            if (setting.forcedMove == true)
            {
                setting.forcedMove = false;
                ForceCapture.Background = Brushes.Red;
            } else
            {
                setting.forcedMove = true;
                ForceCapture.Background = Brushes.Green;
            }
        }

        private void SaveSettings()
        {
            String settingsFile = setting.background + " " + setting.userId;
            File.WriteAllText("Settings.txt", settingsFile);
        }
    }
}
