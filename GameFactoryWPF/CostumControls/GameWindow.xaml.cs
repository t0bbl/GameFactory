using ClassLibrary;
using CoreGameFactory.Model;
using GameFactoryWPF.Utilities;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl, IGameCellControlContainer
    {
        #region Variables
        private MainWindow MainWindow;
        private Match CurrentMatch;
        private Player HomePlayer;

        private TextBlock CurrentPlayerDisplay;

        public List<Player> PlayerList;

        private List<GameCell> GameCells = new List<GameCell>();

        public event EventHandler GameStarted;
        public event EventHandler<Player.PlayerChangedEventArgs> PlayerChanged;
        public event EventHandler<Match.GameCellClickedEventArgs> CellClicked;
        public event EventHandler<Match.GameCellClickedEventArgs> GameCellClicked;
        #endregion

        public GameWindow(MainWindow p_MainWindow, Player p_HomePlayer)
        {
            InitializeComponent();
            MainWindow = p_MainWindow;
            HomePlayer = p_HomePlayer;
            PlayerList = new List<Player>
            {
                HomePlayer,
                DataProvider.GetPlayerVariables(1)
            };
        }
        public GameWindow()
        {
            InitializeComponent();
        }



        private void StartMatch(Match p_Match)
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
            StartGamePanel.Visibility = Visibility.Collapsed;

            Match CurrentMatch = new Match();
            EventhandlerRegister();

            GetAllGameCellControls();

            CreateGameWindow(p_Match);

            GameWindow MatchScreen = new GameWindow(MainWindow, HomePlayer);

            p_Match.ResetBoard();         
            p_Match.StartGameMechanic(PlayerList);

            UpdateCurrentPlayerDisplay();
        }

        #region HandleCellClick
        private void GameCell_CellClicked(object? sender, Match.GameCellClickedEventArgs e)
        {
            CurrentMatch.GameCellClicked(sender, e);

            if (sender is GameCell gameCellControl)
            {
                if (CurrentMatch.GameTypeIdent == 2)
                {
                    HandleFourWTypeGame(gameCellControl, PlayerList[CurrentMatch.CurrentPlayerIndex]);
                }
                else
                {
                    UpdateGameCellControl(gameCellControl, PlayerList[CurrentMatch.CurrentPlayerIndex]);
                    gameCellControl.GameCellClicked -= GameCell_CellClicked;
                }
            }
            CurrentMatch.CurrentPlayerIndex = (CurrentMatch.CurrentPlayerIndex + 1) % CurrentMatch.PlayerList.Count;

            UpdateCurrentPlayerDisplay();
        }
        private void UpdateGameCellControl(GameCell p_GameCell, Player p_Player)
        {
            p_GameCell.CellContent = p_Player.Icon;
            p_GameCell.CellColor = p_Player.Color;
            p_GameCell.IsClicked = true;
        }
        private void HandleFourWTypeGame(GameCell p_GameCell, Player p_Player)
        {
            GameCell lowestCellControl = FindLowestUnclickedGameCell(p_GameCell.Column);
            if (lowestCellControl != null)
            {
                UpdateGameCellControl(lowestCellControl, p_Player);
                if (lowestCellControl.Row == 1)
                {
                    p_GameCell.IsClicked = true;
                    p_GameCell.GameCellClicked -= GameCell_CellClicked;
                }
            }
        }
        private GameCell FindLowestUnclickedGameCell(int p_Column)
        {
            for (int row = CurrentMatch.Rows; row >= 0; row--)
            {
                var cell = GameCells.FirstOrDefault(c => c.Column == p_Column && c.Row == row && !c.IsClicked);
                if (cell != null)
                    return cell;
            }
            return null;
        }
        #endregion
        #region StateChanges
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

                    EventhandlerUnregister();
                }
            }
        }
        private void Match_PlayerChanged(object sender, Player.PlayerChangedEventArgs e)
        {
            UpdateCurrentPlayerDisplay();
        }
        private void UpdateCurrentPlayerDisplay()
        {
            CurrentPlayerDisplay.Text = "Current Player: " + PlayerList[CurrentMatch.CurrentPlayerIndex].Name;
        }
        #endregion
        #region GameTypeStart
        private void OnClickTTT(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new TTT() { PlayerList = PlayerList };
            StartMatch(CurrentMatch);
        }

        private void OnClick4w(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new FourW() { PlayerList = PlayerList };
            StartMatch(CurrentMatch);
        }

        //private void OnClickTwist(object sender, RoutedEventArgs e)
        //{
        //    CurrentMatch = new CustomTTT(true) { PlayerList = PlayerList };
        //    StartMatch(CurrentMatch);
        //}
        #endregion
        #region GameBoardSetup
        protected void CreateGameWindow(Match p_Match)
        {
            var MainContent = new Grid();
            MainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
            MainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(512, GridUnitType.Star) });
            MainContent.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
            MainContent.Children.Add(CreatePlayboard(p_Match));

            MainWindow.MainContent.Content = MainContent;
            CreateCurrentPlayerDisplay(MainContent);
        }

        public Grid CreatePlayboard(Match p_Match)
        {
            var Playboard = new Grid();

            if (p_Match.GameType == "FourW")
            {

                for (int row = 0; row < p_Match.Rows + 1; row++)
                {
                    Playboard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }

                for (int col = 0; col < p_Match.Columns; col++)
                {
                    Playboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }


                for (int row = 0; row == 0; row++)
                {
                    for (int col = 0; col < p_Match.Columns; col++)
                    {
                        CreateGameCell(true, true, row, col, Playboard);
                    }
                }
                for (int row = 1; row < p_Match.Rows + 1; row++)
                {
                    for (int col = 0; col < p_Match.Columns; col++)
                    {
                        CreateGameCell(false, false, row, col, Playboard);
                    }
                }
            }

            else
            {

                for (int row = 0; row < p_Match.Rows; row++)
                {
                    Playboard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }

                for (int col = 0; col < p_Match.Columns; col++)
                {
                    Playboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }

                for (int row = 0; row < p_Match.Rows; row++)
                {
                    for (int col = 0; col < p_Match.Columns; col++)
                    {
                        CreateGameCell(false, true, row, col, Playboard);
                    }
                }
            }


            Grid.SetColumn(Playboard, 1);

            return Playboard;
            
        }


        private void CreateCurrentPlayerDisplay(Grid p_MainContent)
        {
            var CurrentPlayerDisplay = new TextBlock()
            {
                Text = "Current Player: " + CurrentMatch.CurrentPlayer,
            };
            Grid.SetColumn(CurrentPlayerDisplay, 1);
            Grid.SetRow(CurrentPlayerDisplay, 0);
            CurrentPlayerDisplay.HorizontalAlignment = HorizontalAlignment.Center;
            CurrentPlayerDisplay.VerticalAlignment = VerticalAlignment.Bottom;
            CurrentPlayerDisplay.FontSize = 20;
            CurrentPlayerDisplay.FontWeight = FontWeights.Bold;
            CurrentPlayerDisplay.Foreground = (System.Windows.Media.Brush)FindResource("SecondaryHueMidBrush");
            this.CurrentPlayerDisplay = CurrentPlayerDisplay;

            p_MainContent.Children.Add(CurrentPlayerDisplay);
        }

        private void CreateGameCell(bool p_SetterRow, bool p_Clickable, int p_Row, int p_Col, Grid p_Board)
        {
            var CellButton = new GameCell();

            if (p_SetterRow)
            {
                var Icon = new PackIcon();
                Icon.Kind = PackIconKind.ArrowDownCircle;

                CellButton.CellContent = Icon;
                CellButton.CellColor = "Gray";
            }

            if (p_Clickable)
            {
                CellButton.GameCellClicked += GameCell_CellClicked;
            }

            CellButton.Row = p_Row;
            CellButton.Column = p_Col;
            Grid.SetRow(CellButton, p_Row);
            Grid.SetColumn(CellButton, p_Col);
            p_Board.Children.Add(CellButton);
            GameCells.Add(CellButton);
        }

        public IEnumerable<GameCell> GetAllGameCellControls()
        {
            return GameCells;
        }


        private void EventhandlerRegister()
        {
            CurrentMatch.PlayerChanged += Match_PlayerChanged;
            CurrentMatch.GameStateChanged += Match_GameStateChanged;
        }
        private void EventhandlerUnregister()
        {
            CurrentMatch.PlayerChanged -= Match_PlayerChanged;
            CurrentMatch.GameStateChanged -= Match_GameStateChanged;
        }

        #endregion

    }
}
