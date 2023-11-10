using ClassLibrary;
using CoreGameFactory.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
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
                string Result = match.p_Winner == p_Player.Ident ? "Win" : match.p_Loser == p_Player.Ident ? "Loss" : "Draw";
                Result = match.p_Draw == 1 ? "Draw" : Result;
                string Opponent = match.p_Winner == p_Player.Ident ? match.p_LoserName : match.p_WinnerName;
                PlayerHistory.Add(new ClassLibrary.Match
                {
                    p_Winner = match.p_Winner,
                    p_Loser = match.p_Loser,
                    p_Draw = match.p_Draw,
                    p_GameTypeIdent = match.p_GameTypeIdent,
                    p_WinningLength = match.p_WinningLength,
                    p_MatchId = match.p_MatchId,
                    p_Rows = match.p_Rows,
                    p_Columns = match.p_Columns,
                    p_GameType = match.p_GameType,
                    p_LoserName = match.p_LoserName,
                    p_WinnerName = match.p_WinnerName,
                    p_Result = Result,
                    p_Opponent = Opponent
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
                var match = listView.SelectedItem as ClassLibrary.Match;
                if (match != null)
                {
                    var MoveList = LoadMoveHistory(match);

                    MoveHistoryScreen = new MatchDetail(MoveList);
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

        public List<Move> LoadMoveHistory(ClassLibrary.Match p_Match)
        {
            var moveHistoryData = DataProvider.DisplayMoveHistory(p_Match.p_MatchId);
            var moveHistory = new List<Move>(moveHistoryData.Select(move => new Move
            {
                Player = move.Player,
                Match = move.Match,
                Input = move.Input,
                Twist = move.Twist
            }));

            return moveHistory;
        }



        private static void OnPlayerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var historyControl = d as History;
            if (historyControl != null && e.NewValue is Player newPlayer)
            {
                historyControl.LoadHistory(newPlayer);
            }
        }
    }
}
