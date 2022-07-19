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
        private int player;
        private IConfiguration Configuration;
        private SqlConnection connect;
        private Boolean ready = false;
        private string hostName;
        private Boolean otherReady = false;
        private Boolean ping = false;
        private int counter = 0;
        private String sql;
        public Lobby(Settings setting, string lobbyId, string hostName)
        {
            InitializeComponent();
            this.setting = setting;
            this.lobbyId = lobbyId;
            this.player = 2;
            this.hostName = hostName;
            Player1.Content = hostName;
            Player2.Content = setting.userId;
            StartPing.Visibility = Visibility.Visible;
            GetConnect();
        }
        
        public Lobby(Settings setting, string lobbyId)
        {
            InitializeComponent();
            this.setting = setting;
            this.lobbyId = lobbyId;
            this.player = 1;
            Player1.Content = setting.userId;
            StartPing.Visibility = Visibility.Visible;
            GetConnect();
            
            // Creates basis for further SQL commands to work with
            try
            {
                connect.Open();
                string sql = String.Format("Insert [dbo].[{0}] ([ID], [Player1], [Player2]) VALUES (1, '{1}', 'unknown')", lobbyId, setting.userId);
                SqlCommand command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                sql = String.Format("Insert [dbo].[{0}] ([ID], [Player1], [Player2]) VALUES (2, 'waiting', 'waiting')", lobbyId);
                command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                connect.Close();
            }
            catch { Trace.WriteLine("Failed to initialise"); connect.Close(); }
        }

        // Responsible for actions of other users to show on the host's screen + vice versa (with a little desync)
        public async void PingService()
        {
            ping = true;
            while (ping)
            {
                try
                {
                    SqlCommand command;
                    SqlDataReader reader;
                    int opponent = 0;
                    String[] output = new string[2];
                    if (connect.State == ConnectionState.Open) { connect.Close(); }
                    connect.Open();
                    if (player == 1)
                    {
                        opponent = 2;
                    }
                    else
                    {
                        opponent = 1; 
                        try
                        {
                            sql = String.Format("UPDATE [dbo].[{0}] SET Player2 = '{1}' WHERE ID=1", lobbyId, setting.userId);
                            command = new SqlCommand(sql, connect);
                            command.ExecuteNonQuery();
                        }
                        catch { Trace.WriteLine("Updater Broke"); }
                    }
                    connect.Close();

                    connect.Open();
                    sql = String.Format("SELECT Player{0} FROM [dbo].[{1}] WHERE ID=1", opponent, lobbyId);
                    command = new SqlCommand(sql, connect);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        output[0] = reader.GetValue(0).ToString();
                    }
                    connect.Close();
                    connect.Open();
                    sql = String.Format("SELECT Player{0} FROM [dbo].[{1}] WHERE ID=2", opponent, lobbyId);
                    command = new SqlCommand(sql, connect);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        output[1] = reader.GetValue(0).ToString();
                    }
                    connect.Close();
                    if (output[0] != "unknown")
                    {
                        Player2.Visibility = Visibility.Visible;
                    } else { Player2.Visibility = Visibility.Hidden; }
                    if (player == 1)
                    {
                        Player2.Content = output[0];
                    }
                    if (output[1] == "Ready")
                    {
                        otherReady = true;
                        if (opponent == 2)
                        {
                            Player2.Background = Brushes.Green;
                        }
                        else
                        {
                            Player1.Background = Brushes.Green;
                        }
                    }
                    else if (output[1] == "Closing")
                    {
                        MessageBox.Show("Lobby has been closed");
                        NavigationService.Navigate(new OnlineMultiplayer(setting));
                    } else
                    {
                        otherReady = false;
                        if (player == 1)
                        {
                            Player2.Background = Brushes.Red;
                        }
                        else
                        {
                            Player1.Background = Brushes.Red;
                        }
                    }
                } catch { Trace.WriteLine("Ping service broke :/"); try { connect.Close(); } catch { } }
                
                await Task.Delay(1000);
            }
        }

        // Create connect variable which is responsible for connection to the SQL server
        public void GetConnect()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            connect = new SqlConnection(Configuration.GetConnectionString("SQLconnectionstring"));
        }

        // Responsible for readying up and checking if other player is ready
        public async void PlayerReady()
        {
            SqlCommand command;
            if (player == 1)
            {
                if (Ready.Background == Brushes.Green)
                {
                    Ready.Background = Brushes.Red;
                    Player1.Background = Brushes.Red;
                    ready = false;
                    try 
                    {
                        if (connect.State == ConnectionState.Open) { connect.Close(); }
                        connect.Open();
                        sql = String.Format("UPDATE [dbo].[{0}] SET Player1 = 'waiting' WHERE ID=2", lobbyId);
                        command = new SqlCommand(sql, connect);
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                    catch { Trace.WriteLine("Failed to unready up"); connect.Close(); }
                }
                else
                {
                    Ready.Background = Brushes.Green;
                    Player1.Background = Brushes.Green;
                    ready = true;
                    sql = String.Format("UPDATE [dbo].[{0}] SET Player1 = 'Ready' WHERE ID=2", lobbyId);
                    command =new SqlCommand(sql,connect);
                    try
                    {
                        if (connect.State == ConnectionState.Open) { connect.Close(); }
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
                        CountdownTimer(counter);
                        await Task.Delay(1000);
                    }
                }
            }
            else if (player == 2)
            {
                if (Ready.Background == Brushes.Green)
                {
                    Ready.Background = Brushes.Red;
                    Player2.Background = Brushes.Red;
                    ready = false;
                    try
                    {
                        if (connect.State == ConnectionState.Open) { connect.Close(); }
                        connect.Open();
                        string sql = String.Format("UPDATE [dbo].[{0}] SET Player2 = 'waiting' WHERE ID=2", lobbyId);
                        command = new SqlCommand(sql, connect);
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                    catch { Trace.WriteLine("Failed to unready up"); connect.Close(); }
                }
                else
                {
                    Ready.Background = Brushes.Green;
                    Player2.Background = Brushes.Green;
                    ready = true;
                    string sql = String.Format("UPDATE [dbo].[{0}] SET Player2 = 'Ready' WHERE ID=2", lobbyId);
                    command = new SqlCommand(sql, connect);
                    try
                    {
                        if (connect.State == ConnectionState.Open) { connect.Close(); }
                        connect.Open();
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Unable to ready up, Lobby may have been closed, Returning to listing");
                        NavigationService.Navigate(new OnlineMultiplayer(setting));
                        Trace.WriteLine("Failed to ready up");
                        connect.Close();
                    }
                    while (ready)
                    {
                        if (otherReady)
                        {
                            counter++;
                        }
                        CountdownTimer(counter);
                        await Task.Delay(1000);
                    }
                }
            }
            
        }

        // Countdown timer for when both players are ready
        private void CountdownTimer(int count)
        {
            switch (count)
            {
                case 0:
                    Countdown.Visibility = Visibility.Hidden;
                    Ready.Visibility = Visibility.Visible;
                    Ready.Visibility = Visibility.Visible;
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
                    Ready.Visibility = Visibility.Hidden;
                    Ready.Visibility = Visibility.Hidden; ;
                    break;
                case 4:
                    Countdown.Content = "0";
                    ping = false;
                    break;
                default:
                    NavigationService.Navigate(new CheckersRedesigned(setting, lobbyId, player));
                    ready = false;
                    break;
            }
        }

        // If the player leaves the lobby uses either of the two functions below (based on whether host or not)
        private async void CloseLobby()
        {
            sql = String.Format("UPDATE [dbo].[{0}] SET Player1 = 'Closing' WHERE ID=2", lobbyId);
            SqlCommand command = new SqlCommand(sql, connect);
            try
            {
                if (connect.State == ConnectionState.Open) { connect.Close(); }
                connect.Open();
                command.ExecuteNonQuery();
                Trace.WriteLine("Closing Lobby (notifying other player)");
                connect.Close();
            } catch { }
            sql = String.Format("DELETE FROM [dbo].[lobbies] WHERE CONVERT(VARCHAR, Lobby)='{0}'", lobbyId);
            command = new SqlCommand(sql, connect);
            try
            {
                if (connect.State == ConnectionState.Open) { connect.Close(); }
                connect.Open();
                command.ExecuteNonQuery();
                Trace.WriteLine("Deleted listing");
                await Task.Delay(1000);
                sql = "DROP TABLE [dbo].[" + lobbyId + "]";
                command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                Trace.WriteLine("Deleted lobby");
                connect.Close();
            }
            catch { MessageBox.Show("Failed to remove lobby from listing and deleting lobby table"); }
        }
        private void LeaveLobby()
        {
            try 
            {
                if (connect.State == ConnectionState.Open) { connect.Close(); }
                connect.Open();
                sql = String.Format("INSERT [dbo].[lobbies] ([Lobby],[Host]) VALUES ('{0}','{1}')", lobbyId, hostName);
                SqlCommand command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                sql = String.Format("UPDATE [dbo].[{0}] SET Player2 = 'unknown' WHERE ID=1", lobbyId);
                command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                sql = String.Format("UPDATE [dbo].[{0}] SET Player2 = 'waiting' WHERE ID=2", lobbyId);
                command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                connect.Close();
            }
            catch { MessageBox.Show("Failed to relist this lobby"); }
        }

        // Leads to two functions above
        public void Close()
        {
            if (player == 1)
            {
                CloseLobby();
            } else
            {
                LeaveLobby();
            }
            ping = false;
            NavigationService.Navigate(new OnlineMultiplayer(setting));
        }

        // For starting ping service
        private void BtnPingService(object sender, RoutedEventArgs x)
        {
            PingService();
            StartPing.Visibility = Visibility.Hidden;
        }

        // Ready Buttons
        public void BtnReady(object sender, RoutedEventArgs e)
        {
            PlayerReady();
        }

        // Returning to lobby explorer
        private void BtnMultiplayer(object sender, RoutedEventArgs x)
        {
            Close();
            NavigationService.Navigate(new OnlineMultiplayer(setting));
        }

    }
}
