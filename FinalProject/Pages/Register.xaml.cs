using FinalProject.Model;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Data.SqlClient;
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
using System.Diagnostics;

namespace FinalProject.Pages
{
    public partial class Register : Page
    {
        private IConfiguration Configuration;
        private SqlConnection connect;
        private Settings setting;
        public Register(Settings setting)
        {
            this.setting = setting;
            InitializeComponent();
            GetConnect();
        }

        public void GetConnect()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            connect = new SqlConnection(Configuration.GetConnectionString("SQLconnectionstring"));
        }

        public void SQLOperation()
        {
            connect.Open();
            MessageBox.Show("Connection Open  !");

            SqlCommand command;
            SqlDataReader dataReader;
            String sql, Output = "";

            sql = "Select Username,Password from loginDatabase";

            command = new SqlCommand(sql, connect);
            dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                Output = Output + dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + "\n";
            }

            MessageBox.Show(Output);
            connect.Close();
        }

        private void BtnSignUp(object sender, RoutedEventArgs x)
        {
            String? username = user.Text;
            String? password = pass.Text;
            String? passwordConfirm = confirm_pass.Text;
            Boolean found = false;      
            NavigationService.Navigate(new SettingsPage(setting));
            //if (username != null && password != null)     //Confirms username and password is not null
            //{
            //    if (password.Equals(passwordConfirm))
            //    {
            //        for (int i = 0; i < loginInfo.Count; i += 2)
            //        {
            //            if (loginInfo[i].Equals(username))
            //            {
            //                MessageBox.Show("Please try a different username");
            //                found = true;
            //            }
            //        }
            //        if (!found)
            //        {
            //            loginInfo.Add(username);         //adds user to login list
            //            loginInfo.Add(password);
            //            MessageBox.Show("Account Registered");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("The two passwords do not match");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Make sure you have entered a username and password");
            //}
        }
    }
}
