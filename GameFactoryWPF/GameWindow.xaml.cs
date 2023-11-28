using ClassLibrary;
using CoreGameFactory.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl, ICellControlContainer
    {
        private MainWindow MainWindow;
        private Match CurrentMatch;
        private Stats StatScreen;
        private Player HomePlayer;

        private TextBlock CurrentPlayerDisplay;


        public List<Player> PlayerList;

        private List<CellControl> CellControls = new List<CellControl>();


        public event EventHandler GameStarted;
        public event EventHandler<PlayerChangedEventArgs> PlayerChanged;
        public event EventHandler<CellClickedEventArgs> CellClicked;

        event EventHandler<CellClickedEventArgs> ICellControlContainer.CellClicked
        {
            add
            {
                CellClicked += value;
            }

            remove
            {
                CellClicked -= value;
            }
        }



        public GameWindow(MainWindow p_MainWindow, List<Player> p_PlayerList, Match p_Match, Player p_HomePlayer, Stats p_StatScreen)
        {
            InitializeComponent();
            MainWindow = p_MainWindow;
            PlayerList = p_PlayerList;
            CurrentMatch = p_Match;
            HomePlayer = p_HomePlayer;
            StatScreen = p_StatScreen;
        }


        private void StartMatch(Match p_Match)
        {
            p_Match.PlayerChanged += Match_PlayerChanged;
            p_Match.GameStateChanged += Match_GameStateChanged;

            GetAllCellControls();

            GameStarted?.Invoke(this, EventArgs.Empty);
            CreateGameWindow(p_Match.Rows, p_Match.Columns);
            UpdateCurrentPlayerDisplay();

            GameWindow MatchScreen = new GameWindow(MainWindow, PlayerList, p_Match, HomePlayer, StatScreen);
            GameStarted?.Invoke(this, EventArgs.Empty);

            p_Match.ResetBoard();
            p_Match.Start();
            p_Match.GameMechanic(PlayerList);

            UpdateCurrentPlayerDisplay();

        }




        public IEnumerable<CellControl> GetAllCellControls()
        {
            return CellControls;
        }

        private void CellButton_CellClicked(object? sender, CellClickedEventArgs e)
        {
            CurrentMatch.CellClicked(sender, e);

            if (sender is CellControl cellControl)
            {
                cellControl.CellContent = PlayerList[CurrentMatch.CurrentPlayerIndex].Icon;
                cellControl.CellColor = PlayerList[CurrentMatch.CurrentPlayerIndex].Color;
                cellControl.CellClicked -= CellButton_CellClicked;
            }
            CurrentMatch.CurrentPlayerIndex = (CurrentMatch.CurrentPlayerIndex + 1) % CurrentMatch.p_Player.Count;

            UpdateCurrentPlayerDisplay();
        }


        private void Match_GameStateChanged(object sender, GameStateChangedEventArgs e)
        {
            HomePlayer = DataProvider.GetStatsAndVariables(HomePlayer.Ident);

            if (e.Winner.HasValue)
            {
                if (e.Draw.Value == 1)
                {
                    Login.TextBox("It's a draw!");
                    MainWindow.LoadPlayerHome(HomePlayer);
                }
                else
                {
                    Player Winner = DataProvider.GetPlayerVariables(e.Winner.Value);
                    Login.TextBox("The winner is: " + Winner.Name);
                    MainWindow.LoadPlayerHome(HomePlayer);
                }
            }
        }
        private void Match_PlayerChanged(object sender, PlayerChangedEventArgs e)
        {
            UpdateCurrentPlayerDisplay();
        }

        private void UpdateCurrentPlayerDisplay()
        {
            CurrentPlayerDisplay.Text = "Current Player: " + PlayerList[CurrentMatch.CurrentPlayerIndex].Name;
        }
        #region GameTypeStart
        private void OnClickTTT(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new TTT() { p_Player = PlayerList };
            StartMatch(CurrentMatch);
        }

        private void OnClick4w(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new FourW() { p_Player = PlayerList };
            StartMatch(CurrentMatch);
        }

        private void OnClickTwist(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new CustomTTT(true) { p_Player = PlayerList };
            StartMatch(CurrentMatch);
        }
        #endregion
        #region GameBoardSetup
        private void CreateGameWindow(int p_Rows, int p_Columns)
        {
            var MainContent = new Grid();
            MainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
            MainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(512, GridUnitType.Star) });
            MainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
            CreatePlayboard(p_Rows, p_Columns, MainContent);
            CreateCurrentPlayerDisplay(MainContent);
        }

        private void CreatePlayboard(int p_Rows, int p_Columns, Grid p_MainContent)
        {

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
            p_MainContent.Children.Add(playboard);


            MainWindow.MainContent.Content = p_MainContent;
        }

        private void CreateCurrentPlayerDisplay(Grid p_MainContent)
        {
            var CurrentPlayerDisplay = new TextBlock()
            {
                Text = "Current Player: " + CurrentMatch.CurrentPlayer,
            };
            Grid.SetColumn(CurrentPlayerDisplay, 0);
            Grid.SetRow(CurrentPlayerDisplay, 1);
            CurrentPlayerDisplay.HorizontalAlignment = HorizontalAlignment.Center;
            CurrentPlayerDisplay.VerticalAlignment = VerticalAlignment.Bottom;
            CurrentPlayerDisplay.FontSize = 20;
            CurrentPlayerDisplay.FontWeight = FontWeights.Bold;
            CurrentPlayerDisplay.Foreground = System.Windows.Media.Brushes.White;
            this.CurrentPlayerDisplay = CurrentPlayerDisplay;

            p_MainContent.Children.Add(CurrentPlayerDisplay);
        }
        #endregion

    }
}
