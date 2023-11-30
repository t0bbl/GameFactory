using ClassLibrary;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class GameCell : UserControl
    {
        public static readonly DependencyProperty CellContentProperty =
            DependencyProperty.Register("CellContent", typeof(string), typeof(GameCell));

        public event EventHandler<GameCellClickedEventArgs> GameCellClicked;


        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsClicked { get; set; } = false;

        public object CellContent
        {
            get { return (string)GetValue(CellContentProperty); }
            set { CellButton.Content = value; }
        }
        public string CellColor
        {
            get { return CellButton.Background.ToString(); }
            set { CellButton.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString(value); }
        }

        public GameCell()
        {
            InitializeComponent();
            CellButton.Click += GameCell_Click;
        }

        private void GameCell_Click(object sender, RoutedEventArgs e)
        {
            GameCellClicked?.Invoke(this, new GameCellClickedEventArgs(Row, Column));
            IsClicked = true;
        }

    }

}
