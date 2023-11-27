using ClassLibrary;
using CoreGameFactory.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl, ICellControlContainer
    {
        private MainWindow p_MainWindow;

        public event EventHandler<CellClickedEventArgs> cellButtonCellClicked;

        public List<Player> PlayerList;
        
        private List<CellControl> CellControls = new List<CellControl>();


        public event EventHandler GameStarted;
        public ICellControlContainer GameScreenContainer { get; set; }


        public GameWindow(MainWindow p_MainWindow, List<Player> p_PlayerList)
        {
            InitializeComponent();
            this.p_MainWindow = p_MainWindow;
            PlayerList = p_PlayerList;
        }

        event EventHandler<CellClickedEventArgs> ICellControlContainer.CellButton_CellClicked
        {
            add
            {
                cellButtonCellClicked += value;
            }

            remove
            {
                cellButtonCellClicked -= value;
            }
        }

        public void StartGame(Match p_Game)
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
            CreatePlayboard(p_Game.Rows, p_Game.Columns, p_Game.WinningLength);

        }

        private void CreatePlayboard(int p_Rows, int p_Columns, int p_WinningLength)
        {
            var mainContent = new Grid();
            mainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
            mainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(512, GridUnitType.Star) });
            mainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
            var playboard = new Grid();


            for (int row = 0; row < p_Rows; row++)
            {
                playboard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int col = 0; col < p_Columns; col++)
            {
                playboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int row = 0; row < p_Rows; row++)
            {
                for (int col = 0; col < p_Columns; col++)
                {
                    var cellButton = new CellControl();
                    cellButton.Row = row;
                    cellButton.Column = col;
                    cellButton.CellClicked += CellButton_CellClicked;
                    Grid.SetRow(cellButton, row);
                    Grid.SetColumn(cellButton, col);

                    playboard.Children.Add(cellButton);
                    CellControls.Add(cellButton);

                }
            }
            Grid.SetColumn(playboard, 1);
            mainContent.Children.Add(playboard);

            p_MainWindow.MainContent.Content = mainContent;
        }

        private void CellButton_CellClicked(object? sender, CellClickedEventArgs e)
        {
            string message = $"Clicked cell: Row {e.Row}, Column {e.Column}";
            MessageBox.Show(message);
        }

        private void OnClickTTT(object sender, RoutedEventArgs e)
        {
            var TTTGame = new TTT() { p_Player = PlayerList };
            GetAllCellControls();
            StartGame(TTTGame);
            GameWindow TTTScreen = new GameWindow(p_MainWindow, PlayerList);
            GameStarted?.Invoke(this, EventArgs.Empty);
            TTTGame.StartMatch();

            GameScreenContainer = GameScreenContainer;
            if (GameScreenContainer != null)
            {
                GameScreenContainer.CellButton_CellClicked += TTTGame.CellControl_CellClicked;
            }
        }

        private void OnClick4w(object sender, RoutedEventArgs e)
        {
            var FourWGame = new FourW();
            StartGame(FourWGame);
            GameWindow FourWinsScreen = new GameWindow(p_MainWindow, PlayerList);
            GameStarted?.Invoke(this, EventArgs.Empty);

        }

        private void OnClickTwist(object sender, RoutedEventArgs e)
        {
            var CostumTTTGame = new CustomTTT(true);
            StartGame(CostumTTTGame);
            GameWindow TwistScreen = new GameWindow(p_MainWindow, PlayerList);
            GameStarted?.Invoke(this, EventArgs.Empty);

        }

        public IEnumerable<CellControl> GetAllCellControls()
        {
            return CellControls;
        }

    }
}
