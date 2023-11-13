using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClassLibrary;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        public GameWindow()
        {
            InitializeComponent();
            CreatePlayboard(3, 3);

            TTT CurrentGame = new TTT();


        }

        private void CreatePlayboard(int rows, int columns)
        {

            var playboard = new Grid();

            for (int row = 0; row < rows; row++)
            {
                playboard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int col = 0; col < columns; col++)
            {
                playboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var cellButton = new CellControl();

                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);

                    playboard.Children.Add(cellButton);
                }
            }

            MainGrid.Children.Clear();
            MainGrid.Children.Add(playboard);
        }





    }
}
