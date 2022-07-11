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
            //if (loginInfo.Count == 0)
            //{
            //    Console.WriteLine("Please create an account first\n");
            //}
            //else
            //{
            //    while (!loggedIn)
            //    {
            //        Console.Write("Username:");
            //        String? user = Console.ReadLine();
            //        Console.Write("Password:");
            //        String? pass = Console.ReadLine();
            //        for (int i = 0; i < loginInfo.Count; i += 2)
            //        {
            //            if (loginInfo[i].Equals(user))
            //            {
            //                if (pass.Equals(loginInfo[i + 1]))
            //                {
            //                    loggedIn = true;
            //                }
            //            }
            //        }
            //    }
            //    if (loggedIn)
            //    {
            //        Console.WriteLine("Awesome you logged in now");
            //    }
            //    else { Console.WriteLine("Wrong credentials"); }
            //    break;
            //}
        }
    }
}