using FinalProject.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class Checkers : Page
    {
        Settings setting;
        private SqlConnection connect;
        Boolean online = false;
        Boolean yourTurn = true;
        Boolean jump = false;
        Boolean endJump = false;
        int surrenderCounter = 0;
        string Lobby = "";

        //Multi-dimensional jagged array to hold information of all checker pieces
        String[,][] Board = new String[8, 8][] {
            { new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " } },
            { new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" } },
            { new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " }, new string[] { "R", "1" }, new string[] { " ", " " } },
            { new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " } },
            { new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " }, new string[] { " ", " " } },
            { new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" } },
            { new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " } },
            { new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" }, new string[] { " ", " " }, new string[] { "B", "1" } }
        };
        //Key (R = Red), (B = Black), (  = Empty)
        //Key (  = Empty), (1 = Pawn), (2 = King)

        //Holds selected variable
        String[] selected = new string[3];
        
        public Checkers(Settings setting, String Lobby, int player)
        {
            this.setting = setting;
            online = true;
            this.Lobby = Lobby;
            InitializeComponent();
            if (player == 2)
            {
                yourTurn = false;
                RedTurn.Content = "Your Turn (Red)";
                BlackTurn.Content = "Opponents Turn (Black)";
                RedWon.Content = "You've Won (Red wins)";
                BlackWon.Content = "You've Lost (Black wins)";
            }
            else
            {
                RedTurn.Content = "Opponents Turn (Red)";
                BlackTurn.Content = "Your Turn (Black)";
                RedWon.Content = "You've Lost (Red wins)";
                BlackWon.Content = "You've Won (Black wins)";
            }
            Turn("B");
        }
        public Checkers(Settings setting)
        {
            this.setting = setting;
            InitializeComponent();
            Turn("B");
        }

        
        //Two conversion functions from index of array to board and vice versa for horizontal axis
        private string NumToString(int loopReference)
        {
            switch (loopReference)
            {
                case 0: return "A";
                case 1: return "B";
                case 2: return "C";
                case 3: return "D";
                case 4: return "E";
                case 5: return "F";
                case 6: return "G";
                case 7: return "H";
                default: MessageBox.Show("Exception occured in NumToString, result will be incorrect"); return "A"; //Error message and makes sure program doesn't break for identification of error
            }
        }

        private int StringToNum(string boardReference)
        {
            switch (boardReference)
            {
                case "A": return 0;
                case "B": return 1;
                case "C": return 2;
                case "D": return 3;
                case "E": return 4;
                case "F": return 5;
                case "G": return 6;
                case "H": return 7;
                default: MessageBox.Show("Exception occured in StringToNum, result will be incorrect"); return 0; //Error message and makes sure program doesn't break for identification of error
            }
        }

        //Code store - button.Background = new BrushConverter().ConvertFromString("#4CFF33") as SolidColorBrush;
        //Green - #00FF00
        //Yellow - #FFFF00
        //Blue - #0000FF
        //Red - #FF0000
        //White - #FFFFFF

        //Responsible for a new turn
        private void Turn(String side)
        {
            if (side == "R")
            {
                RedTurn.Visibility = Visibility.Visible;
                BlackTurn.Visibility = Visibility.Hidden;
            }
            else if (side == "B")
            {
                RedTurn.Visibility = Visibility.Hidden;
                BlackTurn.Visibility = Visibility.Visible;
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j][0] == side)
                    {
                        string selection = NumToString(j) + (i + 1).ToString() + "Select";
                        ButtonShow(selection);
                    }
                }
            }
            if (!jump)
            {
                ForcedMove(side);
            }
        }

        //Responsible for Pawn movement
        private void Pawn(String grid)
        {
            //change grid to index on board array
            int[] index = new int[] { Int32.Parse(grid[1].ToString()) - 1, StringToNum(grid[0].ToString()) };
            string[] piece = Board[index[0], index[1]];
            Boolean buttons = false;
            if (!jump)
            {
                string selection = NumToString(index[1]) + (index[0] + 1).ToString() + "Select";
                ButtonShow(selection, "#00FF00");
            }
            if (piece[0] == "B")
            {
                //Attack movement
                try
                {
                    if (Board[index[0] - 1, index[1] - 1][0] == "R" && Board[index[0] - 2, index[1] - 2][0] == " ")
                    {
                        string selection = NumToString(index[1] - 2) + (index[0] - 1).ToString() + "Select";
                        ButtonShow(selection, "#0000FF");
                        buttons = true;
                    }
                } catch { Trace.WriteLine("Exception B1 attack left"); }
                try {
                    if (Board[index[0] - 1, index[1] + 1][0] == "R" && Board[index[0] - 2, index[1] + 2][0] == " ")
                    {
                        string selection = NumToString(index[1] + 2) + (index[0] - 1).ToString() + "Select";
                        ButtonShow(selection, "#0000FF");
                        buttons = true;
                    }
                }
                catch { Trace.WriteLine("Exception B1 attack right"); }
                //Normal movement
                if (!jump && !buttons)
                {
                    try
                    {
                        if (Board[index[0] - 1, index[1] - 1][0] == " ")
                        {
                            string selection = NumToString(index[1] - 1) + (index[0]).ToString() + "Select";
                            ButtonShow(selection, "#FFFF00");
                            buttons = true;
                        }
                    }
                    catch { Trace.WriteLine("Exception B1 move left"); }
                    try
                    {
                        if (Board[index[0] - 1, index[1] + 1][0] == " ")
                        {
                            string selection = NumToString(index[1] + 1) + (index[0]).ToString() + "Select";
                            ButtonShow(selection, "#FFFF00");
                            buttons = true;
                        }
                    }
                    catch { Trace.WriteLine("Exception B1 move left"); }
                }
            } else if (piece[0] == "R") {
                //Attack Movement
                try
                {
                    if (Board[index[0] + 1, index[1] - 1][0] == "B" && Board[index[0] + 2, index[1] - 2][0] == " ")
                    {
                        string selection = NumToString(index[1] - 2) + (index[0] + 3).ToString() + "Select";
                        ButtonShow(selection, "#0000FF");
                        buttons = true;
                    }
                } catch { Trace.WriteLine("Exception R1 attack left"); }
                try {
                    if (Board[index[0] + 1, index[1] + 1][0] == "B" && Board[index[0] + 2, index[1] + 2][0] == " ")
                    {
                        string selection = NumToString(index[1] + 2) + (index[0] + 3).ToString() + "Select";
                        ButtonShow(selection, "#0000FF");
                        buttons = true;
                    }
                } catch { Trace.WriteLine("Exception R1 attack right"); }
                //Normal movement
                if (!jump && !buttons)
                {
                    try
                    {
                        if (Board[index[0] + 1, index[1] - 1][0] == " ")
                        {
                            string selection = NumToString(index[1] - 1) + (index[0] + 2).ToString() + "Select";
                            ButtonShow(selection, "#FFFF00");
                            buttons = true;
                        }
                    } catch { Trace.WriteLine("Exception R1 move left"); }
                    try
                    {
                        if (Board[index[0] + 1, index[1] + 1][0] == " ")
                        {
                            string selection = NumToString(index[1] + 1) + (index[0] + 2).ToString() + "Select";
                            ButtonShow(selection, "#FFFF00");
                            buttons = true;
                        }
                    } catch { Trace.WriteLine("Exception R1 move right"); }
                }
            }
            if (!buttons && jump)
            {
                selected = new string[3];
                Trace.WriteLine("Cleared piece: " + selected[0] + "-" + selected[1] + "-" + selected[2]);
                Trace.WriteLine("no possible routes found");
                jump = false;
                endJump = true;
                if (piece[0] == "R")
                {
                    Turn("B");
                }
                else if (piece[0] == "B")
                {
                    Turn("R");
                }
                yourTurn = false;
                Trace.WriteLine("Stored piece post jump: " + selected[0] + "-" + selected[1] + "-" + selected[2]);
            }
        }

        //Responsible for King movement
        private void King(String grid)
        {
            //change grid to index on board array
            int[] index = new int[] { Int32.Parse(grid[1].ToString()) - 1, StringToNum(grid[0].ToString()) };
            string[] piece = Board[index[0], index[1]];
            Boolean buttons = false;
            String Enemy;
            if (piece[0] == "B")
            {
                Enemy = "R";
            } else
            {
                Enemy = "B";
            }
            if (!jump)
            {
                string selection = NumToString(index[1]) + (index[0] + 1).ToString() + "Select";
                ButtonShow(selection, "#00FF00");
            }
            //Attack movement
            try
            {
                if (Board[index[0] - 1, index[1] - 1][0] == Enemy && Board[index[0] - 2, index[1] - 2][0] == " ")
                {
                    string selection = NumToString(index[1] - 2) + (index[0] - 1).ToString() + "Select";
                    ButtonShow(selection, "0000FF");
                    buttons = true;
                }
            }
            catch { Trace.WriteLine("Exception King attack bottom-left"); }
            try
            {
                if (Board[index[0] - 1, index[1] + 1][0] == Enemy && Board[index[0] - 2, index[1] + 2][0] == " ")
                {
                    string selection = NumToString(index[1] + 2) + (index[0] - 1).ToString() + "Select";
                    ButtonShow(selection, "0000FF");
                    buttons = true;
                }
            }
            catch { Trace.WriteLine("Exception King attack bottom-right"); }
            try
            {
                if (Board[index[0] + 1, index[1] - 1][0] == Enemy && Board[index[0] + 2, index[1] - 2][0] == " ")
                {
                    string selection = NumToString(index[1] - 2) + (index[0] + 3).ToString() + "Select";
                    ButtonShow(selection, "0000FF");
                    buttons = true;
                }
            }
            catch { Trace.WriteLine("Exception King attack top-left"); }
            try
            {
                if (Board[index[0] + 1, index[1] + 1][0] == Enemy && Board[index[0] + 2, index[1] + 2][0] == " ")
                {
                    string selection = NumToString(index[1] + 2) + (index[0] + 3).ToString() + "Select";
                    ButtonShow(selection, "0000FF");
                    buttons = true;
                }
            }
            catch { Trace.WriteLine("Exception king attack top-right"); }
            //Normal movement
            if (!jump && !buttons)
            {
                try
                {
                    if (Board[index[0] - 1, index[1] - 1][0] == " ")
                    {
                        string selection = NumToString(index[1] - 1) + (index[0]).ToString() + "Select";
                        ButtonShow(selection, "FFFF00");
                        buttons = true;
                    }
                }
                catch { Trace.WriteLine("Exception King move bottom-left"); }
                try
                {
                    if (Board[index[0] - 1, index[1] + 1][0] == " ")
                    {
                        string selection = NumToString(index[1] + 1) + (index[0]).ToString() + "Select";
                        ButtonShow(selection, "FFFF00");
                        buttons = true;
                    }
                }
                catch { Trace.WriteLine("Exception King move bottom-right"); }
                try
                {
                    if (Board[index[0] + 1, index[1] - 1][0] == " ")
                    {
                        string selection = NumToString(index[1] - 1) + (index[0] + 2).ToString() + "Select";
                        ButtonShow(selection, "FFFF00");
                        buttons = true;
                    }
                }
                catch { Trace.WriteLine("Exception King move top-left"); }
                try
                {
                    if (Board[index[0] + 1, index[1] + 1][0] == " ")
                    {
                        string selection = NumToString(index[1] + 1) + (index[0] + 2).ToString() + "Select";
                        ButtonShow(selection, "FFFF00");
                        buttons = true;
                    }
                }
                catch { Trace.WriteLine("Exception King move top-right"); }
            }
            if (!buttons && jump)
            {
                selected = new string[3];
                Trace.WriteLine("Cleared selected");
                Trace.WriteLine("no possible routes found");
                jump = false;
                endJump = true;
                if (piece[0] == "R")
                {
                    Turn("B");
                }
                else if (piece[0] == "B")
                {
                    Turn("R");
                }
                yourTurn = false;
                Trace.WriteLine("Stored piece post jump: " + selected[0] + "-" + selected[1] + "-" + selected[2]);
            }
        }

        //Function called when a listener is triggered, responsible for players actions - can be tweaked for online multiplayer and AI functionality
        private void Select(String grid)
        {
            string[] temp = Board[Int32.Parse(grid[1].ToString()) - 1, StringToNum(grid[0].ToString())];
            Trace.WriteLine(StringToNum(grid[0].ToString()) + ", " + (Int32.Parse(grid[1].ToString()) - 1));
            string[] blank = new string[3];
            ClearButtons();
            if (selected[0] == null)
            {
                ChooseSelection(grid, temp);
            }
            else if (selected[1] == temp[0] && selected[2] == temp[1])                    //When you want to undo selection
            {
                ClearSelection(grid, temp);
                surrenderCounter++;
                if (surrenderCounter > 3)
                {
                    if (temp[0] == "B")
                        BlackSurrender.Visibility = Visibility.Visible;
                    else
                    {
                        RedSurrender.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                MoveSelection(grid, temp);
                surrenderCounter = 0;
                BlackSurrender.Visibility = Visibility.Hidden;
                RedSurrender.Visibility = Visibility.Hidden;
            }
            if (!yourTurn && online)
            {
                ClearButtons();
                onlinePlayer();
            } else
            {
                yourTurn = true;
            }
            Trace.WriteLine("Stored piece: " + selected[0] + "-" + selected[1] + "-" + selected[2]);
        }

        //Responsible for receiving moves of player
        private async void onlinePlayer()
        {
            int timeoutCounter = 0;
            while (!yourTurn) {
                
                await Task.Delay(1000);


                SqlCommand command;
                SqlDataReader dataReader;
                String sql, Output = "";
                sql = "Select Username,Password from " + Lobby;
                connect = new SqlConnection(connectionString);
                command = new SqlCommand(sql, connect);
                dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    timeoutCounter = 0;

                    Output = dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + "\n";
                }

                if (timeoutCounter = 3)
                {
                    MessageBox.Show("Conection")
                }

                if (timeoutCounter = 20)
                {

                }
                MessageBox.Show(Output);
            }
        }

        //Responsible for upgrading pawn to king when conditions met
        private void UpgradeToKing()
        {
            for (int i = 0; i < 8; i++)
            {
                if (Board[7, i][0] == "R")
                {
                    Board[7, i][1] = "2"; 
                    foreach (object container in ImageContainer.Children)
                    {
                        var gridSelect = container as Grid;
                        foreach (Image image in gridSelect.Children)
                        {
                            if (image.Name == NumToString(i) + "8")
                            {
                                image.Source = new BitmapImage(new Uri(@"../../../Images/CheckerPieces/Red_King.png", UriKind.Relative));
                            }
                        }
                    }
                }
                if (Board[0, i][0] == "B")
                {
                    Board[0, i][1] = "2";
                    foreach (object container in ImageContainer.Children)
                    {
                        var gridSelect = container as Grid;
                        foreach (Image image in gridSelect.Children)
                        {
                            if (image.Name == NumToString(i) + "1")
                            {
                                image.Source = new BitmapImage(new Uri(@"../../../Images/CheckerPieces/Black_King.png", UriKind.Relative));
                            }
                        }
                    }
                }
            }

        }

        //Responsible for checking if a player meets victory conditions
        private void Won()
        {
            int counterRed = 0;
            int counterBlack = 0;
            for (int i = 7; i > -1; i--)
            {
                for (int j = 0; j < 8; j++)
                {
                    Trace.Write(Board[i, j][0]);
                    if (Board[i, j][0] == "R")
                    {
                        counterRed++;
                    } else if (Board[i, j][0] == "B")
                    {
                        counterBlack++;
                    }
                }
                Trace.Write("\n");
            }
            if (counterRed == 0)
            {
                EndScreen.Visibility = Visibility.Visible;
                BlackWon.Visibility = Visibility.Visible;
            } else if (counterBlack == 0)
            {
                EndScreen.Visibility = Visibility.Visible;
                RedWon.Visibility = Visibility.Visible;
            }
        }

        //Responsible for returning image string based on unit colour and type
        private string ImageSelector(string colour, string type)
        {
            switch (colour)
            {
                case "R":
                    switch (type)
                    {
                        case "1":
                            return "../../../Images/CheckerPieces/Red_Pawn.png";
                        case "2":
                            return "../../../Images/CheckerPieces/Red_King.png";
                        default: return "../../../Images/error-icon.png";
                    }
                case "B":
                    switch (type)
                    {
                        case "1":
                            return "../../../Images/CheckerPieces/Black_Pawn.png";
                        case "2":
                            return "../../../Images/CheckerPieces/Black_King.png";
                        default: return "../../../Images/error-icon.png";
                    }
                default: return "../../../Images/error-icon.png";

            }
        }

        //Responsible for clearing all the buttons on the board
        private void ClearButtons()
        {
            foreach (object container in ButtonContainer.Children)
            {
                var gridSelect = container as Grid;
                foreach (Button button in gridSelect.Children)
                {
                    button.Visibility = Visibility.Hidden; //Final should be hidden
                    button.Background = new BrushConverter().ConvertFromString("#FFFFFF") as SolidColorBrush;
                }
            }
        }

        //Responbile for selecting a unit and showing appropiate actions for the unit
        private void ChooseSelection(String grid, string[] temp)
        {
            Trace.WriteLine("Selecting: " + grid);                                      //To select a unit

            if (temp[1] == "1")
            {
                Pawn(grid);
            }
            else if (temp[1] == "2")
            {
                King(grid);
            }
            //Storing of data
            selected[0] = grid;
            selected[1] = temp[0].ToString();
            selected[2] = temp[1].ToString();
            Trace.WriteLine("Storing piece: " + selected[0] + "-" + selected[1] + "-" + selected[2]);
            if (endJump)
            {
                selected = new string[3];
                endJump = false;
            }
        }

        //Responsible for clearing selection and allowing player to choose another unit instead
        private void ClearSelection(String grid, string[] temp)
        {
            Trace.WriteLine("Deselected: " + grid);
            Turn(temp[0]);
            foreach (object container in ButtonContainer.Children)
            {
                var gridSelect = container as Grid;
                foreach (Button button in gridSelect.Children)
                {
                    button.Background = new BrushConverter().ConvertFromString("#FFFFFF") as SolidColorBrush;
                }
            }
            selected = new string[3];
            Trace.WriteLine("Cleared selected");
        }

        //Responsible for moving unit and also removing captured units from the board as well as determining if player continues turn (as a unit can capture more than once in a player turn)
        private void MoveSelection(String grid, string[] temp)
        {
            Trace.WriteLine("Changing to selected: " + grid);
            //Grab image
            foreach (object container in ImageContainer.Children)
            {
                var gridSelect = container as Grid;
                foreach (Image image in gridSelect.Children)
                {
                    if (image.Name == selected[0])
                    {
                        image.Source = new BitmapImage(new Uri(@"../../../Images/Blank.png", UriKind.Relative));
                    }

                    //Place piece
                    if (image.Name == grid)
                    {
                        string piece = ImageSelector(selected[1], selected[2]);
                        image.Source = new BitmapImage(new Uri(@piece, UriKind.Relative));
                    }
                }
            }
            Board[Int32.Parse(grid[1].ToString()) - 1, StringToNum(grid[0].ToString())][0] = selected[1];
            Board[Int32.Parse(grid[1].ToString()) - 1, StringToNum(grid[0].ToString())][1] = selected[2];
            Board[Int32.Parse(selected[0][1].ToString()) - 1, StringToNum(selected[0][0].ToString())][0] = " ";
            Board[Int32.Parse(selected[0][1].ToString()) - 1, StringToNum(selected[0][0].ToString())][1] = " ";
            //Responsible for deleting attacked units
            if (StringToNum(grid[0].ToString()) - StringToNum(selected[0][0].ToString()) == 2 || StringToNum(grid[0].ToString()) - StringToNum(selected[0][0].ToString()) == -2)
            {
                jump = true;
                string selection = NumToString((StringToNum(grid[0].ToString()) + StringToNum(selected[0][0].ToString())) / 2) + ((Int32.Parse(grid[1].ToString()) + Int32.Parse(selected[0][1].ToString())) / 2).ToString();
                Trace.WriteLine("Deleted:" + selection);
                foreach (object container in ImageContainer.Children)
                {
                    var gridSelect = container as Grid;
                    foreach (Image image in gridSelect.Children)
                    {
                        if (image.Name == selection)
                        {
                            image.Source = new BitmapImage(new Uri(@"../../../Images/Blank.png", UriKind.Relative));
                        }
                    }
                }
                Board[(Int32.Parse(grid[1].ToString()) + Int32.Parse(selected[0][1].ToString())) / 2 - 1, (StringToNum(grid[0].ToString()) + StringToNum(selected[0][0].ToString())) / 2][0] = " ";
                Board[(Int32.Parse(grid[1].ToString()) + Int32.Parse(selected[0][1].ToString())) / 2 - 1, (StringToNum(grid[0].ToString()) + StringToNum(selected[0][0].ToString())) / 2][1] = " ";
                Trace.WriteLine("X:" + (StringToNum(grid[0].ToString()) + StringToNum(selected[0][0].ToString())) / 2);
                Trace.WriteLine("Y:" + ((Int32.Parse(grid[1].ToString()) + Int32.Parse(selected[0][1].ToString())) / 2 - 1));
                Trace.WriteLine(Board[(Int32.Parse(grid[1].ToString()) + Int32.Parse(selected[0][1].ToString())) / 2 - 1, (StringToNum(grid[0].ToString()) + StringToNum(selected[0][0].ToString())) / 2][0]);
                Won();
            }
            selected = new string[3];
            Trace.WriteLine("Cleared selected");
            if (!jump)
            {
                if (temp[0] == "R")
                {
                    Turn("B");
                }
                else if (temp[0] == "B")
                {
                    Turn("R");
                }
                yourTurn = false;
            }
            else
            {
                Select(grid);
            }
            UpgradeToKing();
        }

        private void ForcedMove(String side)
        {
            if (setting.forcedMove)
            {
                String Enemy = "B";
                Boolean Forced = false;
                int Modifier = 1;
                if (side == "B")
                {
                    Enemy = "R";
                    Modifier = -1;
                }
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Board[i, j][0] == side)
                        {
                            if (Board[i, j][1] == "1") //Checks if Pawns need to be forced
                            {
                                try
                                {
                                    if (Board[i + Modifier, j + 1][0] == Enemy && Board[i + Modifier * 2, j + 2][0] == " ")
                                    {
                                        if (!Forced) { ClearButtons(); }
                                        Forced = true;
                                        string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                        ButtonShow(selection);
                                    }
                                }
                                catch { Trace.WriteLine("Forced pass 1 right"); }
                                try
                                {
                                    if (Board[i + Modifier, j - 1][0] == Enemy && Board[i + Modifier * 2, j - 2][0] == " ")
                                    {
                                        if (!Forced) { ClearButtons(); }
                                        Forced = true;
                                        string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                        ButtonShow(selection);
                                    }
                                }
                                catch { Trace.WriteLine("Forced pass 1 Left"); }

                            }
                            else //Checks if Kings need to be forced
                            {
                                try
                                {
                                    if (Board[i + Modifier, j + 1][0] == Enemy && Board[i + Modifier * 2, j + 2][0] == " ")
                                    {
                                        if (!Forced) { ClearButtons(); }
                                        Forced = true;
                                        string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                        ButtonShow(selection);
                                    }
                                }
                                catch { Trace.WriteLine("Forced pass 2 right"); }
                                try
                                {
                                    if (Board[i + Modifier, j - 1][0] == Enemy && Board[i + Modifier * 2, j - 2][0] == " ")
                                    {
                                        if (!Forced) { ClearButtons(); }
                                        Forced = true;
                                        string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                        ButtonShow(selection);
                                    }
                                }
                                catch { Trace.WriteLine("Forced pass 2 left"); }
                                try
                                {
                                    if (Board[i - Modifier, j + 1][0] == Enemy && Board[i - Modifier * 2, j + 2][0] == " ")
                                    {
                                        if (!Forced) { ClearButtons(); }
                                        Forced = true;
                                        string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                        ButtonShow(selection);
                                    }
                                }
                                catch { Trace.WriteLine("Forced pass 2 right 2"); }
                                try
                                {
                                    if (Board[i - Modifier, j - 1][0] == Enemy && Board[i - Modifier * 2, j - 2][0] == " ")
                                    {
                                        if (!Forced) { ClearButtons(); }
                                        Forced = true;
                                        string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                        ButtonShow(selection);
                                    }
                                }
                                catch { Trace.WriteLine("Forced pass 2 left 2"); }
                            }
                        }
                    }
                }
            }
        }
        private void ButtonShow(String gridReference)
        {
            foreach (object container in ButtonContainer.Children)
            {
                var gridSelect = container as Grid;
                foreach (Button button in gridSelect.Children)
                {
                    if (button.Name == gridReference)
                    {
                        button.Visibility = Visibility.Visible;
                        button.Background = new BrushConverter().ConvertFromString("#FFFFFF") as SolidColorBrush;
                    }
                }
            }
        }
        private void ButtonShow(String gridReference, String colour)
        {
            foreach (object container in ButtonContainer.Children)
            {
                var gridSelect = container as Grid;
                foreach (Button button in gridSelect.Children)
                {
                    if (button.Name == gridReference)
                    {
                        button.Visibility = Visibility.Visible;
                        button.Background = new BrushConverter().ConvertFromString(colour) as SolidColorBrush;
                    }
                }
            }
        }

        //For returning to Menu
        private void BtnMainMenu(object sender, RoutedEventArgs x)
        {
            NavigationService.Navigate(new MainMenu(setting));
        }

        //Surrender Buttons
        private void BtnBlackSurrender(object sender, RoutedEventArgs x) {
            EndScreen.Visibility = Visibility.Visible;
            RedWon.Visibility = Visibility.Visible;
        }
        private void BtnRedSurrender(object sender, RoutedEventArgs x)
        {
            EndScreen.Visibility = Visibility.Visible;
            BlackWon.Visibility = Visibility.Visible;
        }
        //Listeners responsible for the buttons on the board
        private void Button_A1(object sender, RoutedEventArgs e)
        {
            Select("A1");
        }
        private void Button_A3(object sender, RoutedEventArgs e)
        {
            Select("A3");
        }
        private void Button_A5(object sender, RoutedEventArgs e)
        {
            Select("A5");
        }
        private void Button_A7(object sender, RoutedEventArgs e)
        {
            Select("A7");
        }
        private void Button_B2(object sender, RoutedEventArgs e)
        {
            Select("B2");
        }
        private void Button_B4(object sender, RoutedEventArgs e)
        {
            Select("B4");
        }
        private void Button_B6(object sender, RoutedEventArgs e)
        {
            Select("B6");
        }
        private void Button_B8(object sender, RoutedEventArgs e)
        {
            Select("B8");
        }
        private void Button_C1(object sender, RoutedEventArgs e)
        {
            Select("C1");
        }
        private void Button_C3(object sender, RoutedEventArgs e)
        {
            Select("C3");
        }
        private void Button_C5(object sender, RoutedEventArgs e)
        {
            Select("C5");
        }
        private void Button_C7(object sender, RoutedEventArgs e)
        {
            Select("C7");
        }
        private void Button_D2(object sender, RoutedEventArgs e)
        {
            Select("D2");
        }
        private void Button_D4(object sender, RoutedEventArgs e)
        {
            Select("D4");
        }
        private void Button_D6(object sender, RoutedEventArgs e)
        {
            Select("D6");
        }
        private void Button_D8(object sender, RoutedEventArgs e)
        {
            Select("D8");
        }
        private void Button_E1(object sender, RoutedEventArgs e)
        {
            Select("E1");
        }
        private void Button_E3(object sender, RoutedEventArgs e)
        {
            Select("E3");
        }
        private void Button_E5(object sender, RoutedEventArgs e)
        {
            Select("E5");
        }
        private void Button_E7(object sender, RoutedEventArgs e)
        {
            Select("E7");
        }
        private void Button_F2(object sender, RoutedEventArgs e)
        {
            Select("F2");
        }
        private void Button_F4(object sender, RoutedEventArgs e)
        {
            Select("F4");
        }
        private void Button_F6(object sender, RoutedEventArgs e)
        {
            Select("F6");
        }
        private void Button_F8(object sender, RoutedEventArgs e)
        {
            Select("F8");
        }
        private void Button_G1(object sender, RoutedEventArgs e)
        {
            Select("G1");
        }
        private void Button_G3(object sender, RoutedEventArgs e)
        {
            Select("G3");
        }
        private void Button_G5(object sender, RoutedEventArgs e)
        {
            Select("G5");
        }
        private void Button_G7(object sender, RoutedEventArgs e)
        {
            Select("G7");
        }
        private void Button_H2(object sender, RoutedEventArgs e)
        {
            Select("H2");
        }
        private void Button_H4(object sender, RoutedEventArgs e)
        {
            Select("H4");
        }
        private void Button_H6(object sender, RoutedEventArgs e)
        {
            Select("H6");
        }
        private void Button_H8(object sender, RoutedEventArgs e)
        {
            Select("H8");
        }
    }
}
