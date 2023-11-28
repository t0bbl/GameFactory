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
                if (CurrentMatch.GameTypeIdent == 2)
                {
                    CellControl lowestCellControl = FindLowestUnclickedCellControl(cellControl.Column);
                    if (lowestCellControl != null)
                    {
                        lowestCellControl.CellContent = PlayerList[CurrentMatch.CurrentPlayerIndex].Icon;
                        lowestCellControl.CellColor = PlayerList[CurrentMatch.CurrentPlayerIndex].Color;
                        lowestCellControl.IsClicked = true;
                        if (lowestCellControl.Row == 1)
                        {
                            cellControl.IsClicked = true;
                            cellControl.CellClicked -= CellButton_CellClicked;
                        }
                    }
                }
                else
                {
                    cellControl.CellContent = PlayerList[CurrentMatch.CurrentPlayerIndex].Icon;
                    cellControl.CellColor = PlayerList[CurrentMatch.CurrentPlayerIndex].Color;
                    cellControl.CellClicked -= CellButton_CellClicked;
                    cellControl.IsClicked = true;
                }
            }
            CurrentMatch.CurrentPlayerIndex = (CurrentMatch.CurrentPlayerIndex + 1) % CurrentMatch.PlayerList.Count;

            UpdateCurrentPlayerDisplay();
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
            CurrentMatch = new TTT() { PlayerList = PlayerList };
            StartMatch(CurrentMatch);
        }

        private void OnClick4w(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new FourW() { PlayerList = PlayerList };
            StartMatch(CurrentMatch);
        }

        private void OnClickTwist(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new CustomTTT(true) { PlayerList = PlayerList };
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
                        var CellButton = new CellControl();

                        var Icon = new PackIcon();
                        Icon.Kind = PackIconKind.ArrowDownCircle;

                        CellButton.Row = row;
                        CellButton.Column = col;
                        Grid.SetRow(CellButton, row);
                        Grid.SetColumn(CellButton, col);
                        CellButton.CellClicked += CellButton_CellClicked;
                        CellButton.CellColor = "Gray";
                        CellButton.CellContent = Icon;
                        Playboard.Children.Add(CellButton);
                        CellControls.Add(CellButton);

                    }
                }
                for (int row = 1; row < p_Rows + 1; row++)
                {
                    for (int col = 0; col < p_Columns; col++)
                    {
                        var cellButton = new CellControl();
                        cellButton.Row = row;
                        cellButton.Column = col;
                        Grid.SetRow(cellButton, row);
                        Grid.SetColumn(cellButton, col);
                        Playboard.Children.Add(cellButton);
                        CellControls.Add(cellButton);

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
                        var cellButton = new CellControl();
                        cellButton.Row = row;
                        cellButton.Column = col;
                        Grid.SetRow(cellButton, row);
                        Grid.SetColumn(cellButton, col);
                        cellButton.CellClicked += CellButton_CellClicked;
                        Playboard.Children.Add(cellButton);
                        CellControls.Add(cellButton);

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
        #endregion

    }
}
