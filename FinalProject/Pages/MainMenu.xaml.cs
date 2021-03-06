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
            if (setting.gameType == "Checkers")
            {
                Checkers.Background = Brushes.DimGray;
            } else
            {
                Chess.Background = Brushes.DimGray;
            }
        }

        //Responsible for taking user to Single player page
        private void BtnSolo(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new Solo(setting));
        }

        //Responsible for taking user to Multiplayer page
        private void BtnLocalMultiplayer(object sender, RoutedEventArgs x)
        {
            if (setting.gameType == "Checkers")
            {
                NavigationService.Navigate(new Checkers(setting));
            } else if (setting.gameType == "Chess")
            {
                NavigationService.Navigate(new Chess(setting));
            }
        }

        //Responsible for taking user to Multiplayer page
        private void BtnOnlineMultiplayer(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new OnlineMultiplayer(setting));
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

        private void BtnCheckers(object sender, RoutedEventArgs x)
        {
            setting.gameType = "Checkers";
            Checkers.Background = Brushes.DimGray;
            Chess.Background = Brushes.Gray;
        }
        private void BtnChess(object sender, RoutedEventArgs x)
        {
            setting.gameType = "Chess";
            Checkers.Background = Brushes.Gray;
            Chess.Background = Brushes.DimGray;
        }
    }
}
