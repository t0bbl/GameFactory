using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            //LoadTestData();
            LoadLeaderBoard();
        }

        private void LoadLeaderBoard()
        {
            var LeaderboardData = ClassLibrary.DataProvider.DisplayLeaderBoard();

            var players = new List<Player>();
            foreach (var player in LeaderboardData)
            {
                players.Add(new Player
                {
                    Rank = player.Rank,
                    Name = player.Name,
                    Wins = player.Wins,
                    Losses = player.Losses,
                    Draws = player.Draws,
                    WinPercentage = player.WinPercentage
                });
            }

            this.DataContext = new { Leaderboard = players };
        }
     
    }
}
