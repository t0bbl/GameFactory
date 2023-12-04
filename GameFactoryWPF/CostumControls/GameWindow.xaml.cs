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
        /// <summary>
        /// Initializes a new instance of the GameWindow class with specified main window and home player.
        /// </summary>
        /// <param name="p_MainWindow">The main window hosting this game window.</param>
        /// <param name="p_HomePlayer">The player who is at home at this MAchine.</param>
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


        /// <summary>
        /// Starts a new match with the provided game settings.
        /// </summary>
        /// <param name="p_Match">The match to start.</param>
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
        /// <summary>
        /// Handles cell click events within the game.
        /// Updates game state based on the clicked cell.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments containing cell click details.</param>
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
        /// <summary>
        /// Updates the visual and state properties of a game cell based on the player's attributes.
        /// </summary>
        /// <param name="p_GameCell">The game cell to update.</param>
        /// <param name="p_Player">The player whose properties are used to update the cell.</param>
        private void UpdateGameCellControl(GameCell p_GameCell, Player p_Player)
        {
            p_GameCell.CellContent = p_Player.Icon;
            p_GameCell.CellColor = p_Player.Color;
            p_GameCell.IsClicked = true;
        }
        /// <summary>
        /// Handles the logic for the FourW type game. Updates the game cell based on the player's move.
        /// </summary>
        /// <param name="p_GameCell">The game cell that was clicked.</param>
        /// <param name="p_Player">The player who made the move.</param>
        private void HandleFourWTypeGame(GameCell p_GameCell, Player p_Player)
        {
            GameCell lowestCellControl = FindLowestUnclickedGameCell(p_GameCell.Column, CurrentMatch);
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
        /// <summary>
        /// Finds the lowest unclicked game cell in the specified column for the current match, needed for FourW kind of games.
        /// </summary>
        /// <param name="p_Column">The column to search for the lowest unclicked game cell.</param>
        /// <param name="p_Match">The match context in which to find the cell.</param>
        /// <returns>The lowest unclicked game cell in the specified column, or null if none found.</returns>
        private GameCell FindLowestUnclickedGameCell(int p_Column, Match p_Match)
        {
            for (int row = p_Match.Rows; row >= 0; row--)
            {
                var cell = GameCells.FirstOrDefault(c => c.Column == p_Column && c.Row == row && !c.IsClicked);
                if (cell != null)
                    return cell;
            }
            return null;
        }


        #endregion
        #region StateChanges
        /// <summary>
        /// Handles state changes in the match, such as determining the winner or if the match is a draw.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments containing the game state change details.</param>
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
        /// <summary>
        /// Handles the event triggered when the current player in the match changes.
        /// Updates the display to show the current player.
        /// </summary>
        /// <param name="sender">The source of the event, typically the Match object.</param>
        /// <param name="e">Event arguments containing details about the player change.</param>
        private void Match_PlayerChanged(object sender, Player.PlayerChangedEventArgs e)
        {
            UpdateCurrentPlayerDisplay();
        }
        /// <summary>
        /// Updates the current player display in the game window.
        /// </summary>
        private void UpdateCurrentPlayerDisplay()
        {
            CurrentPlayerDisplay.Text = "Current Player: " + PlayerList[CurrentMatch.CurrentPlayerIndex].Name;
        }
        #endregion
        #region GameTypeStart
        /// <summary>
        /// Starts a Tic-Tac-Toe game when the respective button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnClickTTT(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new TTT() { PlayerList = PlayerList };
            StartMatch(CurrentMatch);
        }
        /// <summary>
        /// Starts a FourW game when the respective button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event arguments.</param>
        private void OnClick4w(object sender, RoutedEventArgs e)
        {
            CurrentMatch = new FourW() { PlayerList = PlayerList };
            StartMatch(CurrentMatch);
        }

        #endregion
        #region GameBoardSetup
        /// <summary>
        /// Creates the game window layout for a given match.
        /// </summary>
        /// <param name="p_Match">The match for which to create the game window.</param>
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
        /// <summary>
        /// Creates a historical playboard showing past moves for a given match.
        /// </summary>
        /// <param name="p_Moves">List of moves to be represented on the playboard.</param>
        /// <param name="p_Match">The match to which the moves belong.</param>
        /// <returns>List of Grids representing each historical state of the game.</returns>
        public List<Grid> CreateHistoryPlayboard(List<Move> p_Moves, Match p_Match)
        {
            List<Grid> HistoricMatch = new List<Grid>();
            Grid CurrentBoard = CreatePlayboard(p_Match);

            foreach (Move historyMove in p_Moves)
            {
                Player currentPlayer = DataProvider.GetPlayerVariables(historyMove.Player);
                GameCell targetCell;

                if (p_Match.GameType == "FourW")
                {
                    targetCell = FindLowestUnclickedGameCell(historyMove.Column, p_Match);
                }
                else
                {
                    targetCell = FindGameCell(CurrentBoard, historyMove.Row, historyMove.Column);
                }

                if (targetCell != null)
                {
                    UpdateGameCellControl(targetCell, currentPlayer);
                }

                Grid clonedBoard = CloneGrid(CurrentBoard);
                HistoricMatch.Add(clonedBoard);
            }

            return HistoricMatch;
        }
        /// <summary>
        /// Finds and returns the GameCell located at the specified row and column in the given board.
        /// </summary>
        /// <param name="board">The grid containing game cells.</param>
        /// <param name="row">The row index of the desired game cell.</param>
        /// <param name="column">The column index of the desired game cell.</param>
        /// <returns>The GameCell at the specified location or null if no cell is found.</returns>
        private GameCell FindGameCell(Grid p_Board, int p_Row, int p_Column)
        {
            foreach (UIElement element in p_Board.Children)
            {
                if (element is GameCell cell && Grid.GetRow(element) == p_Row && Grid.GetColumn(element) == p_Column)
                {
                    return cell;
                }
            }
            return null;
        }
        /// <summary>
        /// Creates a deep copy of the given grid along with its game cell elements.
        /// </summary>
        /// <param name="p_OriginalGrid">The original grid to be cloned.</param>
        /// <returns>A new Grid object that is a clone of the original grid.</returns>
        private Grid CloneGrid(Grid p_OriginalGrid)
        {
            Grid ClonedGrid = new Grid();

            foreach (var rowDef in p_OriginalGrid.RowDefinitions)
                ClonedGrid.RowDefinitions.Add(new RowDefinition { Height = rowDef.Height });

            foreach (var colDef in p_OriginalGrid.ColumnDefinitions)
                ClonedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = colDef.Width });

            foreach (UIElement child in p_OriginalGrid.Children)
            {
                if (child is GameCell OriginalCell)
                {
                    GameCell ClonedCell = new GameCell
                    {
                        Row = OriginalCell.Row,
                        Column = OriginalCell.Column,
                        CellContent = OriginalCell.CellButton.Content,
                        CellColor = OriginalCell.CellColor,
                        IsClicked = OriginalCell.IsClicked
                    };
                    ClonedCell.SetValue(GameCell.CellContentProperty, OriginalCell.CellContent);


                    Grid.SetRow(ClonedCell, Grid.GetRow(OriginalCell));
                    Grid.SetColumn(ClonedCell, Grid.GetColumn(OriginalCell));

                    ClonedGrid.Children.Add(ClonedCell);
                }
            }

            return ClonedGrid;
        }
        /// <summary>
        /// Creates and sets up the game playboard for a given match configuration.
        /// <param name="p_Match">The Match with the Game Parameters.</param>
        /// </summary>
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
        /// <summary>
        /// Creates and adds a display for the current player to the specified grid.
        /// This display shows the name of the current player in the game.
        /// </summary>
        /// <param name="p_MainContent">The grid where the current player display will be added.</param>
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
        /// <summary>
        /// Creates a game cell and adds it to the playboard.
        /// </summary>
        /// <param name="p_SetterRow">Indicates if the row is a setter row like needed for FourW.</param>
        /// <param name="p_Clickable">Indicates if the cell is clickable.</param>
        /// <param name="p_Row">The row index where the cell will be placed.</param>
        /// <param name="p_Col">The column index where the cell will be placed.</param>
        /// <param name="p_Board">The playboard grid where the cell will be added.</param>
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
        /// <summary>
        /// Returns an enumerable of all game cell controls within the game.
        /// </summary>
        /// <returns>An enumerable of GameCell objects.</returns>
        public IEnumerable<GameCell> GetAllGameCellControls()
        {
            return GameCells;
        }

        /// <summary>
        /// Registers event handlers for the current match.
        /// </summary>
        private void EventhandlerRegister()
        {
            CurrentMatch.PlayerChanged += Match_PlayerChanged;
            CurrentMatch.GameStateChanged += Match_GameStateChanged;
        }
        /// <summary>
        /// Unregisters event handlers for the current match.
        /// </summary>
        private void EventhandlerUnregister()
        {
            CurrentMatch.PlayerChanged -= Match_PlayerChanged;
            CurrentMatch.GameStateChanged -= Match_GameStateChanged;
        }

        #endregion

    }
}
