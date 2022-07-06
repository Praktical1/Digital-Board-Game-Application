using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using FinalProject.Model;

namespace FinalProject.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private Settings setting;
        public MainMenu(Settings setting)
        {
            InitializeComponent();
            this.setting = setting;
            Trace.WriteLine(setting.background);
            Trace.WriteLine(setting.style);
            Trace.WriteLine(setting.userId);
        }

        //Responsible for taking user to Single player page
        private void BtnSolo(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new Solo(setting));
        }

        //Responsible for taking user to Multiplayer page
        private void BtnMultiplayer(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new Multiplayer(setting));
        }

        //Responsible for taking user to Settings page
        private void BtnSettings(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new SettingsPage(setting));
        }

        //Responsible for closing program
        private void BtnExit(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Close();
        }
    }
}
