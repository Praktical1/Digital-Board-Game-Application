using FinalProject.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
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
using System.ComponentModel;
using System.Diagnostics;

namespace FinalProject.Pages
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {
        private Settings setting;
        private string lobbyId;
        int player;
        private IConfiguration Configuration;
        private SqlConnection connect;
        private Boolean ready = false;
        private string hostName;
        public Lobby(Settings setting, string lobbyId, int player, string hostName)
        {
            InitializeComponent();
            this.setting = setting;
            this.lobbyId = lobbyId;
            this.player = player;
            this.hostName = hostName;
            GetConnect();
        }
        public Lobby(Settings setting, string lobbyId, int player)
        {
            InitializeComponent();
            this.setting = setting;
            this.lobbyId = lobbyId;
            this.player = player;
            GetConnect();
            if (player == 1)
            {
                string sql = "Insert [dbo].[" + lobbyId + "] ([ID], [Player1], [Player2]) VALUES (1, 'waiting', 'waiting')";
                SqlCommand command = new SqlCommand(sql, connect);
                try
                {
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                }
                catch { Trace.WriteLine("Failed to initialise"); connect.Close(); }
            }
        }
        public void GetConnect()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            connect = new SqlConnection(Configuration.GetConnectionString("SQLconnectionstring"));
        }

        public async void PlayerReady(int num)
        {
            SqlCommand command;
            if (player == 1 && num == 1)
            {
                if (Player1Ready.Background == Brushes.Green)
                {
                    Player1Ready.Background = Brushes.Red;
                    ready = false;
                    string sql = "DELETE FROM [dbo].[" + lobbyId + "] WHERE CONVERT(VARCHAR, Player1)='Ready'";
                    command = new SqlCommand(sql, connect);
                    try { 
                        connect.Open();
                        command.ExecuteNonQuery();
                        sql = "Update [dbo].[" + lobbyId + "] SET Player1 = 'waiting' WHERE ID=1";
                        command = new SqlCommand(sql, connect);
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                    catch { Trace.WriteLine("Failed to unready up"); connect.Close(); }
                }
                else
                {
                    Player1Ready.Background = Brushes.Green;
                    ready = true;
                    string sql = "Update [dbo].["+lobbyId+"] SET Player1 = 'Ready' WHERE ID=1";
                    command =new SqlCommand(sql,connect);
                    try
                    {
                        connect.Open();
                        command.ExecuteNonQuery();
                        connect.Close();
                    } catch { Trace.WriteLine("Failed to ready up"); connect.Close(); }
                    while (ready)
                    {
                        try
                        {
                            sql = "Select Player2 from [dbo].[" + lobbyId + "]";
                            command = new SqlCommand(sql, connect);
                            connect.Open();
                            String check = "";
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                check = reader.GetValue(0).ToString();
                            }
                            connect.Close();
                            if (check == "Ready")
                            {
                                NavigationService.Navigate(new Checkers(setting, lobbyId, 1));
                                ready = false;
                            }
                        } catch { Trace.WriteLine("failed to check for ready"); connect.Close(); }
                        await Task.Delay(1000);
                    }
                }
            }
            else if (player == 2 && num == 2)
            {
                if (Player2Ready.Background == Brushes.Green)
                {
                    Player2Ready.Background = Brushes.Red;
                    ready = false;
                    string sql = "DELETE FROM [dbo].[" + lobbyId + "] WHERE CONVERT(VARCHAR, Player2)='Ready'";
                    command = new SqlCommand(sql, connect);
                    try
                    {
                        connect.Open();
                        command.ExecuteNonQuery();
                        sql = "Update [dbo].[" + lobbyId + "] SET Player2 = 'waiting' WHERE ID=1";
                        command = new SqlCommand(sql, connect);
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                    catch { Trace.WriteLine("Failed to unready up"); connect.Close(); }
                }
                else
                {
                    Player2Ready.Background = Brushes.Green;
                    ready = true;
                    string sql = "Update [dbo].[" + lobbyId + "] SET Player2 = 'Ready' WHERE ID=1";
                    command = new SqlCommand(sql, connect);
                    try
                    {
                        connect.Open();
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                    catch { Trace.WriteLine("Failed to ready up"); connect.Close(); }
                    while (ready)
                    {
                        try
                        {
                            sql = "Select Player1 from [dbo].[" + lobbyId + "]";
                            command = new SqlCommand(sql, connect);
                            connect.Open();
                            String check = "";
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                check = reader.GetValue(0).ToString();
                            }
                            connect.Close();
                            if (check == "Ready")
                            {
                                NavigationService.Navigate(new Checkers(setting, lobbyId, 2));
                                ready = false;
                            }
                        }
                        catch { Trace.WriteLine("failed to check for ready"); connect.Close(); }
                        await Task.Delay(1000);
                    }
                }
            }
            
        }
        public void BtnPlayer1Ready(object sender, RoutedEventArgs e)
        {
            PlayerReady(1);
        }
        public void BtnPlayer2Ready(object sender, RoutedEventArgs e)
        {
            PlayerReady(2);
        }
        private void BtnMultiplayer(object sender, RoutedEventArgs x)
        {
            Close();
            NavigationService.Navigate(new OnlineMultiplayer(setting));
        }

        private void CloseLobby()
        {
            string sql = "DELETE FROM [dbo].[lobbies] WHERE CONVERT(VARCHAR, Lobby)='" + lobbyId + "'";
            SqlCommand command = new SqlCommand(sql, connect);
            try
            {
                connect.Open();
                Trace.WriteLine("Going to delete entries");
                command.ExecuteNonQuery();
                Trace.WriteLine("deleted entries");
                sql = "DROP TABLE [dbo].[" + lobbyId + "]";
                command = new SqlCommand(sql, connect);
                Trace.WriteLine("Going to delete table");
                command.ExecuteNonQuery();
                Trace.WriteLine("deleted table");
                connect.Close();
            }
            catch { MessageBox.Show("Failed to remove lobby from listing and deleting lobby table"); }
            NavigationService.Navigate(new OnlineMultiplayer(setting));
        }
        private async void GoToGame()
        {
            await Task.Delay(5000);
            NavigationService.Navigate(new Checkers(setting, lobbyId, 1));
        }
        private void CreateLobbyListing()
        {
            Player2Ready.Background = Brushes.Red;
            try 
            { 
                connect.Open();
                string sql = "Insert [dbo].[lobbies] ([Lobby],[Host]) VALUES ('" + lobbyId + "','" + hostName + "')";
                SqlCommand command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                connect.Close();
                NavigationService.Navigate(new OnlineMultiplayer(setting));
            }
            catch { MessageBox.Show("Failed to relist this lobby"); }
        }

        public void Close()
        {
            if (player == 1)
            {
                CloseLobby();
            } else
            {
                CreateLobbyListing();
            }
        }

    }
}
