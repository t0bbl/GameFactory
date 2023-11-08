using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : UserControl
    {
        public Player Player
        {
            get { return (Player)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        public static readonly DependencyProperty PlayerProperty =
               DependencyProperty.Register("Player", typeof(Player), typeof(History), new PropertyMetadata(null, OnPlayerPropertyChanged));

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
