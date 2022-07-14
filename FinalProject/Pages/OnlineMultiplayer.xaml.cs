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
        private List<string[]> lobbies = new List<string[]>();

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
                Trace.WriteLine("Connection Open");

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
            String sql = "DELETE FROM [dbo].[lobbies] WHERE CONVERT(VARCHAR, Lobby)='" + lobbyId + "'";
            command = new SqlCommand(sql, connect);
            try
            {
                connect.Open();
                command.ExecuteNonQuery();
                connect.Close();
            } catch { Trace.WriteLine("Failed to delete lobby on entering"); }
            NavigationService.Navigate(new Lobby(setting, lobbyId, 2, hostName));
        }

        private void ListVisualUpdate(int lobbyCount)
        {
            switch (lobbyCount)
            {
                case 0:
                    Lobby1.Visibility = Visibility.Hidden;
                    Lobby2.Visibility = Visibility.Hidden;
                    Lobby3.Visibility = Visibility.Hidden;
                    Lobby4.Visibility = Visibility.Hidden;
                    Lobby5.Visibility = Visibility.Hidden;
                    Lobby6.Visibility = Visibility.Hidden;
                    Lobby7.Visibility = Visibility.Hidden;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Hidden;
                    Lobby3.Visibility = Visibility.Hidden;
                    Lobby4.Visibility = Visibility.Hidden;
                    Lobby5.Visibility = Visibility.Hidden;
                    Lobby6.Visibility = Visibility.Hidden;
                    Lobby7.Visibility = Visibility.Hidden;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Hidden;
                    Lobby4.Visibility = Visibility.Hidden;
                    Lobby5.Visibility = Visibility.Hidden;
                    Lobby6.Visibility = Visibility.Hidden;
                    Lobby7.Visibility = Visibility.Hidden;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Hidden;
                    Lobby5.Visibility = Visibility.Hidden;
                    Lobby6.Visibility = Visibility.Hidden;
                    Lobby7.Visibility = Visibility.Hidden;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 4:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby4.Text = lobbies[3][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Visible;
                    Lobby5.Visibility = Visibility.Hidden;
                    Lobby6.Visibility = Visibility.Hidden;
                    Lobby7.Visibility = Visibility.Hidden;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 5:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby4.Text = lobbies[3][1] + "'s Lobby";
                    Lobby5.Text = lobbies[4][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Visible;
                    Lobby5.Visibility = Visibility.Visible;
                    Lobby6.Visibility = Visibility.Hidden;
                    Lobby7.Visibility = Visibility.Hidden;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 6:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby4.Text = lobbies[3][1] + "'s Lobby";
                    Lobby5.Text = lobbies[4][1] + "'s Lobby";
                    Lobby6.Text = lobbies[5][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Visible;
                    Lobby5.Visibility = Visibility.Visible;
                    Lobby6.Visibility = Visibility.Visible;
                    Lobby7.Visibility = Visibility.Hidden;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 7:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby4.Text = lobbies[3][1] + "'s Lobby";
                    Lobby5.Text = lobbies[4][1] + "'s Lobby";
                    Lobby6.Text = lobbies[5][1] + "'s Lobby";
                    Lobby7.Text = lobbies[6][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Visible;
                    Lobby5.Visibility = Visibility.Visible;
                    Lobby6.Visibility = Visibility.Visible;
                    Lobby7.Visibility = Visibility.Visible;
                    Lobby8.Visibility = Visibility.Hidden;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 8:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby4.Text = lobbies[3][1] + "'s Lobby";
                    Lobby5.Text = lobbies[4][1] + "'s Lobby";
                    Lobby6.Text = lobbies[5][1] + "'s Lobby";
                    Lobby7.Text = lobbies[6][1] + "'s Lobby";
                    Lobby8.Text = lobbies[7][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Visible;
                    Lobby5.Visibility = Visibility.Visible;
                    Lobby6.Visibility = Visibility.Visible;
                    Lobby7.Visibility = Visibility.Visible;
                    Lobby8.Visibility = Visibility.Visible;
                    Lobby9.Visibility = Visibility.Hidden;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                case 9:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby4.Text = lobbies[3][1] + "'s Lobby";
                    Lobby5.Text = lobbies[4][1] + "'s Lobby";
                    Lobby6.Text = lobbies[5][1] + "'s Lobby";
                    Lobby7.Text = lobbies[6][1] + "'s Lobby";
                    Lobby8.Text = lobbies[7][1] + "'s Lobby";
                    Lobby9.Text = lobbies[8][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Visible;
                    Lobby5.Visibility = Visibility.Visible;
                    Lobby6.Visibility = Visibility.Visible;
                    Lobby7.Visibility = Visibility.Visible;
                    Lobby8.Visibility = Visibility.Visible;
                    Lobby9.Visibility = Visibility.Visible;
                    Lobby10.Visibility = Visibility.Hidden;
                    break;
                default:
                    Lobby1.Text = lobbies[0][1] + "'s Lobby";
                    Lobby2.Text = lobbies[1][1] + "'s Lobby";
                    Lobby3.Text = lobbies[2][1] + "'s Lobby";
                    Lobby4.Text = lobbies[3][1] + "'s Lobby";
                    Lobby5.Text = lobbies[4][1] + "'s Lobby";
                    Lobby6.Text = lobbies[5][1] + "'s Lobby";
                    Lobby7.Text = lobbies[6][1] + "'s Lobby";
                    Lobby8.Text = lobbies[7][1] + "'s Lobby";
                    Lobby9.Text = lobbies[8][1] + "'s Lobby";
                    Lobby10.Text = lobbies[9][1] + "'s Lobby";
                    Lobby1.Visibility = Visibility.Visible;
                    Lobby2.Visibility = Visibility.Visible;
                    Lobby3.Visibility = Visibility.Visible;
                    Lobby4.Visibility = Visibility.Visible;
                    Lobby5.Visibility = Visibility.Visible;
                    Lobby6.Visibility = Visibility.Visible;
                    Lobby7.Visibility = Visibility.Visible;
                    Lobby8.Visibility = Visibility.Visible;
                    Lobby9.Visibility = Visibility.Visible;
                    Lobby10.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void BtnRefreshList(object sender, RoutedEventArgs x)
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
                NavigationService.Navigate(new Lobby(setting, lobbyId.ToString(), 1));
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
