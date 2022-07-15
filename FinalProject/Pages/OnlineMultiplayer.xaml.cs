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
    /// <summary>
    /// Interaction logic for OnlineMultiplayer.xaml
    /// </summary>
    public partial class OnlineMultiplayer : Page
    {
        private Settings setting;
        private IConfiguration Configuration;
        private SqlConnection connect;
        private List<string[]> lobbies;

        public OnlineMultiplayer(Settings setting)
        {
            InitializeComponent();
            GetConnect();
            this.setting = setting;
            Title.Content = "Lobbies - Logged in as " + setting.userId;
            RefreshLobbies();
        }
        public void GetConnect()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            connect = new SqlConnection(Configuration.GetConnectionString("SQLconnectionstring"));
        }

        private void RefreshLobbies()
        {
            try
            {
                connect.Open();
                lobbies = new List<string[]>();
                SqlCommand command;
                SqlDataReader dataReader;
                String sql = "";

                sql = "Select Lobby,Host from lobbies";
                command = new SqlCommand(sql, connect);
                try
                {
                    dataReader = command.ExecuteReader();
                }
                catch
                {
                    sql = "CREATE TABLE [dbo].[lobbies]([Lobby] [text] NOT NULL,[Host] [text] NOT NULL)";
                    command = new SqlCommand(sql, connect);
                    command.ExecuteNonQuery();
                    sql = "Select Lobby,Host from lobbies";
                    command = new SqlCommand(sql, connect);
                    dataReader = command.ExecuteReader();
                }

                while (dataReader.Read())
                {
                    lobbies.Add(new string[2] { dataReader.GetValue(0).ToString(), dataReader.GetValue(1).ToString() });
                }
                connect.Close();
                try
                {
                    ListVisualUpdate(lobbies.Count);
                    Trace.WriteLine("Lobbies Updated");
                }
                catch { MessageBox.Show("Failed to update list"); }
            }
            catch { MessageBox.Show("Failed to connect to database"); NavigationService.Navigate(new MainMenu(setting)); }
        }
        

        private void LobbyJoin(int lobby)
        {
            string lobbyId = lobbies[lobby][0];
            string hostName = lobbies[lobby][1];
            SqlCommand command;
            String sql = String.Format("DELETE FROM [dbo].[lobbies] WHERE CONVERT(VARCHAR, Lobby)='{0}'", lobbyId);
            command = new SqlCommand(sql, connect);
            try
            {
                connect.Open();
                command.ExecuteNonQuery();
                connect.Close();
            } catch { Trace.WriteLine("Failed to delete lobby on entering"); }
            NavigationService.Navigate(new Lobby(setting, lobbyId, hostName));
        }

        private void ListVisualUpdate(int lobbyCount)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    String lobbyReference = "Lobby" + (i+1);
                    foreach (Button button in ListingContainer.Children)
                    {
                        TextBlock textBlock = (TextBlock)button.Content;
                        if (textBlock.Name == lobbyReference)
                        {
                            if (i < lobbyCount)
                            {
                                textBlock.Text = lobbies[i][1] + "'s Lobby";
                                textBlock.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                textBlock.Visibility = Visibility.Hidden;
                            }
                        }
                    }
                }
            }
            catch { Trace.WriteLine("Lobby listing exception"); }
        }

        private void BtnRefresh(object sender, RoutedEventArgs x)
        {
            RefreshLobbies();
        }
        private void BtnCreateLobby(object sender, RoutedEventArgs x)
        {
            Random rng = new Random();
            int lobbyId = rng.Next(999999999);
            string sql = "Insert [dbo].[lobbies] ([Lobby],[Host]) VALUES ('" + lobbyId.ToString() + "','" + setting.userId + "')";
            SqlCommand command = new SqlCommand(sql, connect);
            try
            {
                connect.Open();
                command.ExecuteNonQuery(); 
                sql = String.Format("CREATE TABLE [dbo].[{0}]([ID] [int] NOT NULL,[Player1] [text] NULL,[Player2] [text] NULL)",lobbyId);
                command = new SqlCommand(sql, connect);
                command.ExecuteNonQuery();
                connect.Close();
                NavigationService.Navigate(new Lobby(setting, lobbyId.ToString()));
            } catch { MessageBox.Show("Failed to connect to database to create lobby"); }
        }
        private void BtnMainMenu(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new MainMenu(setting));
        }
        public void BtnLobby1(object sender, RoutedEventArgs e)
        {
            LobbyJoin(0);
        }
        public void BtnLobby2(object sender, RoutedEventArgs e)
        {
            LobbyJoin(1);
        }
        public void BtnLobby3(object sender, RoutedEventArgs e)
        {
            LobbyJoin(2);
        }
        public void BtnLobby4(object sender, RoutedEventArgs e)
        {
            LobbyJoin(3);
        }
        public void BtnLobby5(object sender, RoutedEventArgs e)
        {
            LobbyJoin(4);
        }
        public void BtnLobby6(object sender, RoutedEventArgs e)
        {
            LobbyJoin(5);
        }
        public void BtnLobby7(object sender, RoutedEventArgs e)
        {
            LobbyJoin(6);
        }
        public void BtnLobby8(object sender, RoutedEventArgs e)
        {
            LobbyJoin(7);
        }
        public void BtnLobby9(object sender, RoutedEventArgs e)
        {
            LobbyJoin(8);
        }
        public void BtnLobby10(object sender, RoutedEventArgs e)
        {
            LobbyJoin(9);
        }
    }
}
