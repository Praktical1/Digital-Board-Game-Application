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
            String connectionString = "Data Source=sql8.freesqldatabase.com,3306;Initial Catalog=sql8504638;User Id=sql8504638;Password=2E2n5mxEpT;Authentication=Sql Password";
            connect = new SqlConnection(connectionString);
            SQLConnectionOpen();
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

        }
    }
}
