using FinalProject.Model;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        private SqlConnection connect;
        private Settings setting;
        public Register(Settings setting)
        {
            InitializeComponent();
            this.setting = setting;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.UserInstance = true;
            builder.InitialCatalog = "model";
            builder.IntegratedSecurity = true;
            Trace.WriteLine(builder.ConnectionString);
            String connectionString = "Data Source=PUKACHEW\\PRAKSERVER;Integrated Security=True;TrustServerCertificate=False;Encrypt=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Database=test";
            connect = new SqlConnection(connectionString);
            connect.Open();
            MessageBox.Show("Connection Open  !");
            connect.Close();
        }

        public void SQLConnectionOpen()
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
