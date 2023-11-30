using ClassLibrary;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : UserControl
    {
        public Leaderboard()
        {
            InitializeComponent();
            LoadLeaderBoard();
        }

        private void LoadLeaderBoard()
        {
            var LeaderboardData = ClassLibrary.DataProvider.DisplayLeaderBoard();

            var PlayerBoard = new List<Player>();
            foreach (var player in LeaderboardData)
            {
                PlayerBoard.Add(new Player
                {
                    Rank = player.Rank,
                    Name = player.Name,
                    Wins = player.Wins,
                    Losses = player.Losses,
                    Draws = player.Draws,
                    WinPercentage = player.WinPercentage
                });
            }
            this.DataContext = new { Leaderboard = PlayerBoard };
        }
    }
}
