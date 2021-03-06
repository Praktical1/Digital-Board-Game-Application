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
using System.Windows.Shapes;

namespace FinalProject.Pages
{
    /// <summary>
    /// Interaction logic for Solo.xaml
    /// </summary>
    public partial class Solo : Page
    {
        Settings setting;
        public Solo(Settings setting)
        {
            InitializeComponent();
            this.setting = setting;
        }

        private void BtnMainMenu(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new MainMenu(setting));
        }
    }
}
