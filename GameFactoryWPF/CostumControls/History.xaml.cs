using ClassLibrary;
using CoreGameFactory.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : UserControl
    {
        MatchDetail MoveHistoryScreen;

        public History(Player p_Player)
        {
            InitializeComponent();
            this.DataContext = p_Player;
        }

        public void LoadHistory(Player p_Player)
        {
            var HistoryData = DataProvider.DisplayHistory(p_Player.Ident);

            var PlayerHistory = new List<ClassLibrary.Match>();
            foreach (var match in HistoryData)
            {
                string Result = match.Winner == p_Player.Ident ? "Win" : match.Loser == p_Player.Ident ? "Loss" : "Draw";
                Result = match.Draw == 1 ? "Draw" : Result;
                string Opponent = match.Winner == p_Player.Ident ? match.LoserName : match.WinnerName;
                PlayerHistory.Add(new ClassLibrary.Match
                {
                    Winner = match.Winner,
                    Loser = match.Loser,
                    Draw = match.Draw,
                    GameTypeIdent = match.GameTypeIdent,
                    WinningLength = match.WinningLength,
                    MatchId = match.MatchId,
                    Rows = match.Rows,
                    Columns = match.Columns,
                    GameType = match.GameType,
                    LoserName = match.LoserName,
                    WinnerName = match.WinnerName,
                    Result = Result,
                    Opponent = Opponent
                });
            }

            this.DataContext = PlayerHistory;
            CommandManager.InvalidateRequerySuggested();
        }

        public void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null && listView.SelectedItem != null)
            {
                var match = listView.SelectedItem as Match;
                if (match != null)
                {
                    var MoveList = LoadMoveHistory(match);

                    MoveHistoryScreen = new MatchDetail(MoveList, match);
                    MoveHistoryScreen.DataContext = MoveList;

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null)
                    {

                        mainWindow.HistoryPanel.Children.Clear();

                        mainWindow.HistoryPanel.Children.Add(MoveHistoryScreen);
                        MoveHistoryScreen.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        public List<Move> LoadMoveHistory(Match p_Match)
        {
            var moveHistoryData = DataProvider.DisplayMoveHistory(p_Match.MatchId);
            var moveHistory = new List<Move>(moveHistoryData.Select(move => new Move
            {
                Player = move.Player,
                Match = move.Match,
                Input = move.Input,
                Twist = move.Twist,
                PlayerName = move.PlayerName
            }));

            return moveHistory;
        }
    }
}
