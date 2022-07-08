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
    public partial class Login : Page
    {
        private Settings setting;
        public Login(Settings setting)
        {
            InitializeComponent();
            this.setting = setting;
        }

        private void BtnLogin(object sender, RoutedEventArgs x)
        {
            String? username = user.Text;
            String? password = pass.Text;
            NavigationService.Navigate(new SettingsPage(setting));
        }
    }
}
