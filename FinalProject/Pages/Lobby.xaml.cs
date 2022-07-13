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
        private Boolean otherReady = false;
        private Boolean ping = false;
        private int counter = 0;
        public Lobby(Settings setting, string lobbyId, int player, string hostName)
        {
            InitializeComponent();
            this.setting = setting;
            this.lobbyId = lobbyId;
            this.player = player;
            this.hostName = hostName;
            Player1.Content = hostName;
            Player2.Content = setting.userId;
            GetConnect();
        }
        
        public Lobby(Settings setting, string lobbyId, int player)
        {
            InitializeComponent();
            this.setting = setting;
            this.lobbyId = lobbyId;
            this.player = player;
            Player1.Content = setting.userId;
            GetConnect();
            
            try
            {
                connect.Open();
                string sql = "Insert [dbo].[" + lobbyId + "] ([ID], [Player1], [Player2]) VALUES (1, '"+ setting.userId + "', 'Unknown')";
                SqlCommand command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                sql = "Insert [dbo].[" + lobbyId + "] ([ID], [Player1], [Player2]) VALUES (2, 'waiting', 'waiting')";
                command.ExecuteNonQuery();
                command = new SqlCommand(sql, connect);
                connect.Close();
            }
            catch { Trace.WriteLine("Failed to initialise"); connect.Close(); }
        }

        public async void PingService()
        {
            while (ping)
            {
                try
                {
                    int opponent = 0;
                    if (player == 1)
                    {
                        opponent = 2;
                    }
                    else { opponent = 1; }

                    String sql = "Select Player" + opponent + " from [dbo].[" + lobbyId + "]";
                    SqlCommand command = new SqlCommand(sql, connect);
                    connect.Open();
                    String name = "";
                    String check = "";
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        name = reader.GetValue(0).ToString();
                        check = reader.GetValue(0).ToString();
                    }
                    Trace.WriteLine(name);
                    Trace.WriteLine(check);

                    connect.Close();
                    if (player == 1)
                    {
                        Player2.Content = name;
                    }
                    if (check == "Ready")
                    {
                        otherReady = true;
                        if (opponent == 2)
                        {
                            Player2Ready.Background = Brushes.Green;
                        }
                        else
                        {
                            Player1Ready.Background = Brushes.Green;
                        }
                    }
                    else
                    {
                        otherReady = false;
                        if (player == 1)
                        {
                            Player2Ready.Background = Brushes.Red;
                        }
                        else
                        {
                            Player1Ready.Background = Brushes.Red;
                        }
                    }
                } catch { Trace.WriteLine("Ping service broke :/"); try { connect.Close(); } catch { } }
                switch (counter)
                {
                    case 0:
                        Countdown.Visibility = Visibility.Hidden;
                        Player1Ready.Visibility = Visibility.Visible;
                        Player2Ready.Visibility = Visibility.Visible;
                        break;
                    case 1:
                        Countdown.Visibility = Visibility.Visible;
                        Countdown.Content = "3";
                        break;
                    case 2:
                        Countdown.Content = "2";
                        break;
                    case 3:
                        Countdown.Content = "1";
                        Player1Ready.Visibility = Visibility.Hidden;
                        Player2Ready.Visibility = Visibility.Hidden; ;
                        break;
                    case 4:
                        Countdown.Content = "0";
                        break;
                }
                await Task.Delay(1000);
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
                    //string sql = "DELETE FROM [dbo].[" + lobbyId + "] WHERE CONVERT(VARCHAR, Player1)='Ready'";
                    //command = new SqlCommand(sql, connect);
                    try { 
                        connect.Open();
                        //command.ExecuteNonQuery();
                        string sql = "Update [dbo].[" + lobbyId + "] SET Player1 = 'waiting' WHERE ID=2";
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
                    string sql = "Update [dbo].["+lobbyId+"] SET Player1 = 'Ready' WHERE ID=2";
                    command =new SqlCommand(sql,connect);
                    try
                    {
                        connect.Open();
                        command.ExecuteNonQuery();
                        connect.Close();
                    } catch { Trace.WriteLine("Failed to ready up"); connect.Close(); }
                    while (ready)
                    {
                        if (otherReady)
                        {
                            counter++;
                        }
                        if (counter > 4)
                        {
                            NavigationService.Navigate(new Checkers(setting, lobbyId, 1));
                            ready = false;
                        }
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
                    //string sql = "DELETE FROM [dbo].[" + lobbyId + "] WHERE CONVERT(VARCHAR, Player2)='Ready'";
                    //command = new SqlCommand(sql, connect);
                    try
                    {
                        connect.Open();
                        //command.ExecuteNonQuery();
                        string sql = "Update [dbo].[" + lobbyId + "] SET Player2 = 'waiting' WHERE ID=2";
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
                    string sql = "Update [dbo].[" + lobbyId + "] SET Player2 = 'Ready' WHERE ID=2";
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
                        if (otherReady)
                        {
                            counter++;
                        }
                        if (counter > 4)
                        {
                            NavigationService.Navigate(new Checkers(setting, lobbyId, 2));
                            ready = false;
                        }
                        await Task.Delay(1000);
                    }
                }
            }
            
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
                sql = "Update [dbo].[" + lobbyId + "] SET Player2 = 'waiting' WHERE ID=2";
                command = new SqlCommand(sql, connect);
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

        //For starting ping service
        private void BtnPingService(object sender, RoutedEventArgs x)
        {
            PingService();
            StartPing.Visibility = Visibility.Hidden;
        }

        //Ready Buttons
        public void BtnPlayer1Ready(object sender, RoutedEventArgs e)
        {
            PlayerReady(1);
        }
        public void BtnPlayer2Ready(object sender, RoutedEventArgs e)
        {
            PlayerReady(2);
        }

        //Returning to lobby explorer
        private void BtnMultiplayer(object sender, RoutedEventArgs x)
        {
            Close();
            NavigationService.Navigate(new OnlineMultiplayer(setting));
        }

    }
}
