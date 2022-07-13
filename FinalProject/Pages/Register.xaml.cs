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
        private List<String[]> loginDatabase = new List<String[]>();
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
                    sql = "CREATE TABLE [dbo].[loginDatabase]([Username] [text] NOT NULL , [Password] [text] NOT NULL)";
                    command = new SqlCommand(sql, connect);
                    command.ExecuteNonQuery();
                    sql = "Select Username,Password from loginDatabase";
                    command = new SqlCommand(sql, connect);
                    dataReader = command.ExecuteReader();
                }

                while (dataReader.Read())
                {
                    string a = dataReader.GetValue(0).ToString();
                    String b = dataReader.GetValue(1).ToString();
                    loginDatabase.Add(new string[2] { a, b });
                }
                connect.Close();
            } catch { MessageBox.Show("Failed to connect to database"); NavigationService.Navigate(new SettingsPage(setting)); }
        }

        private void SaveSettings()
        {
            String settingsFile = setting.background + " " + setting.userId;
            File.WriteAllText("Settings.txt", settingsFile);
        }

        private void BtnSignUp(object sender, RoutedEventArgs x)
        {
            String? username = user.Text;
            String? password = pass.Text;
            String? passwordConfirm = confirm_pass.Text;
            SqlCommand command;

            if (username != null && password != null)     //Confirms username and password is not null
            {
                if (password.Equals(passwordConfirm))
                {
                    GetLoginDatabase();
                    Boolean found = false;
                    for (int i = 0; i < loginDatabase.Count; i ++)
                    {
                        if (loginDatabase[i][0].Equals(username))
                        {
                            MessageBox.Show("Please try a different username");
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        string sql = "Insert [dbo].[loginDatabase] ([Username],[Password]) VALUES ('" + username + "','" + password + "')";
                        command = new SqlCommand(sql, connect);
                        try
                        {
                            connect.Open();
                            command.ExecuteNonQuery();
                            MessageBox.Show("Registered User");
                            connect.Close();
                            setting.userId = username;
                            SaveSettings();
                        } catch { MessageBox.Show("Failed to register user to database"); }
                        NavigationService.Navigate(new SettingsPage(setting));
                    }
                }
                else
                {
                    MessageBox.Show("The two passwords do not match");
                }
            }
            else
            {
                MessageBox.Show("Make sure you have entered a username and password");
            }
        }
        private void BtnMainMenu(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new MainMenu(setting));
        }
    }
}
