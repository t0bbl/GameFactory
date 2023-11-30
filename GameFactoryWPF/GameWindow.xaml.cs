using ClassLibrary;
using CoreGameFactory.Model;
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
    public partial class GameWindow : UserControl, ICellControlContainer
    {
        #region Variables
        private MainWindow MainWindow;
        private Match CurrentMatch;
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

        private void StartMatch(Match p_Match)
        {
            GameStarted?.Invoke(this, EventArgs.Empty);

            Match CurrentMatch = new Match();
            EventhandlerRegister();

            GetAllCellControls();

            CreateGameWindow(p_Match.Rows, p_Match.Columns);

            GameWindow MatchScreen = new GameWindow(MainWindow, HomePlayer);

            p_Match.ResetBoard();
            p_Match.Start();
            p_Match.GameMechanic(PlayerList);

            UpdateCurrentPlayerDisplay();
        }

        #region HandleCellClick
        private void CellButton_CellClicked(object? sender, CellClickedEventArgs e)
        {
            CurrentMatch.CellClicked(sender, e);

            if (sender is CellControl cellControl)
            {
                if (CurrentMatch.GameTypeIdent == 2)
                {
                    HandleFourWTypeGame(cellControl, PlayerList[CurrentMatch.CurrentPlayerIndex]);
                }
                else
                {
                    UpdateCellControl(cellControl, PlayerList[CurrentMatch.CurrentPlayerIndex]);
                    cellControl.CellClicked -= CellButton_CellClicked;
                }
            }
            CurrentMatch.CurrentPlayerIndex = (CurrentMatch.CurrentPlayerIndex + 1) % CurrentMatch.PlayerList.Count;

            UpdateCurrentPlayerDisplay();
        }
        private void UpdateCellControl(CellControl p_CellControl, Player p_Player)
        {
            p_CellControl.CellContent = p_Player.Icon;
            p_CellControl.CellColor = p_Player.Color;
            p_CellControl.IsClicked = true;
        }
        private void HandleFourWTypeGame(CellControl p_CellControl, Player p_Player)
        {
            CellControl lowestCellControl = FindLowestUnclickedCellControl(p_CellControl.Column);
            if (lowestCellControl != null)
            {
                UpdateCellControl(lowestCellControl, p_Player);
                if (lowestCellControl.Row == 1)
                {
                    p_CellControl.IsClicked = true;
                    p_CellControl.CellClicked -= CellButton_CellClicked;
                }
            }
        }
        private CellControl FindLowestUnclickedCellControl(int p_Column)
        {
            for (int row = CurrentMatch.Rows; row >= 0; row--)
            {
                var cell = CellControls.FirstOrDefault(c => c.Column == p_Column && c.Row == row && !c.IsClicked);
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
        private void Match_PlayerChanged(object sender, PlayerChangedEventArgs e)
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
            var Playboard = new Grid();

            if (CurrentMatch.GameType == "FourW")
            {

                for (int row = 0; row < p_Rows + 1; row++)
                {
                    Playboard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }

                for (int col = 0; col < p_Columns; col++)
                {
                    Playboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }


                for (int row = 0; row == 0; row++)
                {
                    for (int col = 0; col < p_Columns; col++)
                    {
                        CreateCellButton(true, true, row, col, Playboard);
                    }
                }
                for (int row = 1; row < p_Rows + 1; row++)
                {
                    for (int col = 0; col < p_Columns; col++)
                    {
                        CreateCellButton(false, false, row, col, Playboard);
                    }
                }
            }

            else
            {

                for (int row = 0; row < p_Rows; row++)
                {
                    Playboard.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }

                for (int col = 0; col < p_Columns; col++)
                {
                    Playboard.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }

                for (int row = 0; row < p_Rows; row++)
                {
                    for (int col = 0; col < p_Columns; col++)
                    {
                       CreateCellButton(false, true, row, col, Playboard);
                    }
                }
            }


            Grid.SetColumn(Playboard, 1);
            p_MainContent.Children.Add(Playboard);


            MainWindow.MainContent.Content = p_MainContent;
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

        private void CreateCellButton(bool p_SetterRow, bool p_Clickable, int p_Row, int p_Col, Grid p_Board)
        {
            var CellButton = new CellControl();

            if (p_SetterRow)
            {
                var Icon = new PackIcon();
                Icon.Kind = PackIconKind.ArrowDownCircle;

                CellButton.CellContent = Icon;
                CellButton.CellColor = "Gray";
            }

            if (p_Clickable)
            {
                CellButton.CellClicked += CellButton_CellClicked;
            }

            CellButton.Row = p_Row;
            CellButton.Column = p_Col;
            Grid.SetRow(CellButton, p_Row);
            Grid.SetColumn(CellButton, p_Col);
            p_Board.Children.Add(CellButton);
            CellControls.Add(CellButton);
        }

        public IEnumerable<CellControl> GetAllCellControls()
        {
            return CellControls;
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
