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
using System.Windows.Media.Imaging;
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
        }
        private void BtnMainMenu(object sender, RoutedEventArgs x)
        {
            String settingsFile = setting.background + " " + setting.style + " " + setting.userId;
            File.WriteAllText("Settings.txt", settingsFile);
            NavigationService.Navigate(new MainMenu(setting));
        }

        private void ChangeBackgroundtoDefault(object sender, RoutedEventArgs e)
        {
            setting.background = "../Images/Background.png";
        }

        private void ChangeBackgroundtoCool(object sender, RoutedEventArgs e)
        {
            setting.background = "../Images/CoolBackground.png";
        }
    }
}
