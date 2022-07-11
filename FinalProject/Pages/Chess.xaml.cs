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

namespace FinalProject.Pages
{
    public partial class Chess : Page
    {
        //Multi-dimensional jagged array to hold information of all chess pieces
        String[,][] Board = new String[8, 8][] {
            { new string[] { "W", "2" }, new string[] { "W", "3" }, new string[] { "W", "4" }, new string[] { "W", "5" }, new string[] { "W", "6" }, new string[] { "W", "4" }, new string[] { "W", "3" }, new string[] { "W", "2" } },
            { new string[] { "W", "1" }, new string[] { "W", "1" }, new string[] { "W", "1" }, new string[] { "W", "1" }, new string[] { "W", "1" }, new string[] { "W", "1" }, new string[] { "W", "1" }, new string[] { "W", "1" } },
            { new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" } },
            { new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" } },
            { new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" } },
            { new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" }, new string[] { "E", "0" } },
            { new string[] { "B", "1" }, new string[] { "B", "1" }, new string[] { "B", "1" }, new string[] { "B", "1" }, new string[] { "B", "1" }, new string[] { "B", "1" }, new string[] { "B", "1" }, new string[] { "B", "1" } },
            { new string[] { "B", "2" }, new string[] { "B", "3" }, new string[] { "B", "4" }, new string[] { "B", "5" }, new string[] { "B", "6" }, new string[] { "B", "4" }, new string[] { "B", "3" }, new string[] { "B", "2" } }
        };
        //Key (W = White), (B = Black), (E = Empty)
        //Key (0 = Empty), (1 = Pawn), (2 = Rook), (3 = Knight), (4 = Bishop), (5 = Queen), (6 = King)

        //Holds selected variable
        String[] selected = new string[2];
        public test()
        {
            InitializeComponent();
            Turn("W");
        }

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
        private void Turn(String side)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j][0] == side)
                    {
                        foreach (object container in ButtonContainer.Children)
                        {
                            var gridselect = container as Grid;
                            foreach (Button button in gridselect.Children)
                            {
                                string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                if (button.Name == selection)
                                {
                                    button.Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                    if (Board[i, j][1] == "3")
                    {
                        foreach (object container in ButtonContainer.Children)
                        {
                            var gridselect = container as Grid;
                            foreach (Button button in gridselect.Children)
                            {
                                string selection = NumToString(j) + (i + 1).ToString() + "Select";
                                if (button.Name == selection)
                                {
                                    button.Background = new BrushConverter().ConvertFromString("#FFFF00") as SolidColorBrush;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Select(String grid)
        {
            MessageBox.Show("Selected: " + grid);
            foreach (object container in ButtonContainer.Children)
            {
                var gridselect = container as Grid;
                foreach (Button button in gridselect.Children)
                {
                    button.Visibility = Visibility.Visible;
                }
            }
        }

        private void ClearSelection(String side)
        {

        }

        private void Button_A1(object sender, RoutedEventArgs e)
        {
            Select("A1");
        }
        private void Button_A2(object sender, RoutedEventArgs e)
        {
            Select("A2");
        }
        private void Button_A3(object sender, RoutedEventArgs e)
        {
            Select("A3");
        }
        private void Button_A4(object sender, RoutedEventArgs e)
        {
            Select("A4");
        }
        private void Button_A5(object sender, RoutedEventArgs e)
        {
            Select("A5");
        }
        private void Button_A6(object sender, RoutedEventArgs e)
        {
            Select("A6");
        }
        private void Button_A7(object sender, RoutedEventArgs e)
        {
            Select("A7");
        }
        private void Button_A8(object sender, RoutedEventArgs e)
        {
            Select("A8");
        }
        private void Button_B1(object sender, RoutedEventArgs e)
        {
            Select("B1");
        }
        private void Button_B2(object sender, RoutedEventArgs e)
        {
            Select("B2");
        }
        private void Button_B3(object sender, RoutedEventArgs e)
        {
            Select("B3");
        }
        private void Button_B4(object sender, RoutedEventArgs e)
        {
            Select("B4");
        }
        private void Button_B5(object sender, RoutedEventArgs e)
        {
            Select("B5");
        }
        private void Button_B6(object sender, RoutedEventArgs e)
        {
            Select("B6");
        }
        private void Button_B7(object sender, RoutedEventArgs e)
        {
            Select("B7");
        }
        private void Button_B8(object sender, RoutedEventArgs e)
        {
            Select("B8");
        }
        private void Button_C1(object sender, RoutedEventArgs e)
        {
            Select("C1");
        }
        private void Button_C2(object sender, RoutedEventArgs e)
        {
            Select("C2");
        }
        private void Button_C3(object sender, RoutedEventArgs e)
        {
            Select("C3");
        }
        private void Button_C4(object sender, RoutedEventArgs e)
        {
            Select("C4");
        }
        private void Button_C5(object sender, RoutedEventArgs e)
        {
            Select("C5");
        }
        private void Button_C6(object sender, RoutedEventArgs e)
        {
            Select("C6");
        }
        private void Button_C7(object sender, RoutedEventArgs e)
        {
            Select("C7");
        }
        private void Button_C8(object sender, RoutedEventArgs e)
        {
            Select("C8");
        }
        private void Button_D1(object sender, RoutedEventArgs e)
        {
            Select("D1");
        }
        private void Button_D2(object sender, RoutedEventArgs e)
        {
            Select("D2");
        }
        private void Button_D3(object sender, RoutedEventArgs e)
        {
            Select("D3");
        }
        private void Button_D4(object sender, RoutedEventArgs e)
        {
            Select("D4");
        }
        private void Button_D5(object sender, RoutedEventArgs e)
        {
            Select("D5");
        }
        private void Button_D6(object sender, RoutedEventArgs e)
        {
            Select("D6");
        }
        private void Button_D7(object sender, RoutedEventArgs e)
        {
            Select("D7");
        }
        private void Button_D8(object sender, RoutedEventArgs e)
        {
            Select("D8");
        }
        private void Button_E1(object sender, RoutedEventArgs e)
        {
            Select("E1");
        }
        private void Button_E2(object sender, RoutedEventArgs e)
        {
            Select("E2");
        }
        private void Button_E3(object sender, RoutedEventArgs e)
        {
            Select("E3");
        }
        private void Button_E4(object sender, RoutedEventArgs e)
        {
            Select("E4");
        }
        private void Button_E5(object sender, RoutedEventArgs e)
        {
            Select("E5");
        }
        private void Button_E6(object sender, RoutedEventArgs e)
        {
            Select("E6");
        }
        private void Button_E7(object sender, RoutedEventArgs e)
        {
            Select("E7");
        }
        private void Button_E8(object sender, RoutedEventArgs e)
        {
            Select("E8");
        }
        private void Button_F1(object sender, RoutedEventArgs e)
        {
            Select("F1");
        }
        private void Button_F2(object sender, RoutedEventArgs e)
        {
            Select("F2");
        }
        private void Button_F3(object sender, RoutedEventArgs e)
        {
            Select("F3");
        }
        private void Button_F4(object sender, RoutedEventArgs e)
        {
            Select("F4");
        }
        private void Button_F5(object sender, RoutedEventArgs e)
        {
            Select("F5");
        }
        private void Button_F6(object sender, RoutedEventArgs e)
        {
            Select("F6");
        }
        private void Button_F7(object sender, RoutedEventArgs e)
        {
            Select("F7");
        }
        private void Button_F8(object sender, RoutedEventArgs e)
        {
            Select("F8");
        }
        private void Button_G1(object sender, RoutedEventArgs e)
        {
            Select("G1");
        }
        private void Button_G2(object sender, RoutedEventArgs e)
        {
            Select("G2");
        }
        private void Button_G3(object sender, RoutedEventArgs e)
        {
            Select("G3");
        }
        private void Button_G4(object sender, RoutedEventArgs e)
        {
            Select("G4");
        }
        private void Button_G5(object sender, RoutedEventArgs e)
        {
            Select("G5");
        }
        private void Button_G6(object sender, RoutedEventArgs e)
        {
            Select("G6");
        }
        private void Button_G7(object sender, RoutedEventArgs e)
        {
            Select("G7");
        }
        private void Button_G8(object sender, RoutedEventArgs e)
        {
            Select("G8");
        }
        private void Button_H1(object sender, RoutedEventArgs e)
        {
            Select("H1");
        }
        private void Button_H2(object sender, RoutedEventArgs e)
        {
            Select("H2");
        }
        private void Button_H3(object sender, RoutedEventArgs e)
        {
            Select("H3");
        }
        private void Button_H4(object sender, RoutedEventArgs e)
        {
            Select("H4");
        }
        private void Button_H5(object sender, RoutedEventArgs e)
        {
            Select("H5");
        }
        private void Button_H6(object sender, RoutedEventArgs e)
        {
            Select("H6");
        }
        private void Button_H7(object sender, RoutedEventArgs e)
        {
            Select("H7");
        }
        private void Button_H8(object sender, RoutedEventArgs e)
        {
            Select("H8");
        }
    }
}
