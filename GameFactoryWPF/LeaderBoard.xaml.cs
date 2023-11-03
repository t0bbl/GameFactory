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
            LoadTestData();
        }

        private void LoadTestData()
        {
            var testPlayers = new List<PlayerRecord>
        {
            new PlayerRecord { Place = 1, Name = "Alice", Wins = 50, Losses = 5, Draws = 2, WinPercentage = 89.5 },
            new PlayerRecord { Place = 2, Name = "Bob", Wins = 45, Losses = 7, Draws = 3, WinPercentage = 83.3 },
        };

            this.DataContext = new { Leaderboard = testPlayers };
        }
    }
}
