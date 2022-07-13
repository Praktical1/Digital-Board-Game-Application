using FinalProject.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Data.SqlClient;
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
using System.Diagnostics;

namespace FinalProject.Pages
{
    public partial class Login : Page
    {
        private IConfiguration Configuration;
        private SqlConnection connect;
        private Settings setting;
        private List<String[]> loginDatabase = new List<String[]>();
        public Login(Settings setting)
        {
            InitializeComponent();
            this.setting = setting;
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

        public void GetLoginDatabase()
        {
            try
            {
                connect.Open();
                Trace.WriteLine("Connection Open");

                SqlCommand command;
                SqlDataReader dataReader;
                String sql = "";

                sql = "Select Username,Password from loginDatabase";

                command = new SqlCommand(sql, connect);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch
                {
                    MessageBox.Show("Please create an account first");
                    sql = "CREATE TABLE [dbo].[loginDatabase]([Username] [text] NOT NULL , [Password] [text] NOT NULL)";
                    command = new SqlCommand(sql, connect);
                    command.ExecuteNonQuery();
                    sql = "Select Username,Password from loginDatabase";
                    command = new SqlCommand(sql, connect);
                    dataReader = command.ExecuteReader();
                    NavigationService.Navigate(new SettingsPage(setting));
                }

                while (dataReader.Read())
                {
                    String usernames = dataReader.GetValue(0).ToString();
                    String passwords = dataReader.GetValue(1).ToString();
                    loginDatabase.Add(new string[2] { usernames, passwords });
                }
                connect.Close();
            }
            catch { MessageBox.Show("Failed to connect to database"); NavigationService.Navigate(new SettingsPage(setting)); }
        }
        private void SaveSettings()
        {
            String settingsFile = setting.background + " " + setting.userId;
            File.WriteAllText("Settings.txt", settingsFile);
        }

        private void BtnLogin(object sender, RoutedEventArgs x)
        {
            String? username = user.Text;
            String? password = pass.Text;
            GetLoginDatabase();
            if (loginDatabase.Count == 0)
            {
                MessageBox.Show("Please create an account first");
            }
            else if (username=="" || password == "")
            {
                MessageBox.Show("Please enter a username and password");
            }
            else
            {
                Boolean found = false;
                for (int i = 0; i < loginDatabase.Count; i += 2)
                {
                    if (loginDatabase[i][0].Equals(username))
                    {
                        if (password.Equals(loginDatabase[i][1]))
                        {
                            MessageBox.Show("You've Logged in as " + loginDatabase[i][0]);
                            setting.userId = username;
                            SaveSettings();
                            NavigationService.Navigate(new SettingsPage(setting));
                        } else { MessageBox.Show("Wrong credentials"); }
                        found = true;
                    }
                }
                if (!found) { MessageBox.Show("Wrong credentials"); }
            }
        }

        private void BtnMainMenu(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new MainMenu(setting));
        }
    }
}