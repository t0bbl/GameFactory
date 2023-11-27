using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Login LoginScreen = new Login();
        Leaderboard LeaderBoardScreen = new Leaderboard();
        Stats StatsScreen;
        History HistoryScreen;
        GameWindow GameScreen;
        List<Player> PlayerList = new List<Player>();
        Match Match;
        Player HomePlayer;

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = LoginScreen;
            LoginScreen.PlayerLoggedIn += LoginScreen_PlayerLoggedIn;
        }
        #region UI Event Handlers
        private void ToMainScreen(object sender, RoutedEventArgs e)
        {
            MainContent.Content = LoginScreen;
        }
        private void ToLeaderboard(object sender, RoutedEventArgs e)
        {
            MainContent.Content = LeaderBoardScreen;
        }
        private void ToStats(object sender, RoutedEventArgs e)
        {
            if (StatsScreen == null)
            {
                Login.TextBox("Please log in to view stats.");
                return;
            }

            StatsScreen.Visibility = StatsScreen.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void ToHistory(object sender, RoutedEventArgs e)
        {
            if (HistoryScreen == null)
            {
                Login.TextBox("Please log in to view history.");
                return;
            }
            MainContent.Content = HistoryScreen;

            GameScreen.StartGamePanel.Visibility = Visibility.Visible;
        }

        #endregion
        public void LoginScreen_PlayerLoggedIn(Player p_Player)
        {
            StatsPanel.Children.Clear();
            StatsScreen = new Stats(p_Player);
            StatsPanel.Children.Add(StatsScreen);

            HistoryPanel.Children.Clear();
            HistoryScreen = new History(p_Player);
            HistoryScreen.LoadHistory(p_Player);

            HomePlayer = p_Player;

            GameScreen = new GameWindow(this, PlayerList, Match, HomePlayer, StatsScreen);
            GameScreen.GameStarted += GameLogic_GameStarted;

            GamesPanel.Children.Clear();
            GamesPanel.Children.Add(GameScreen);

            PlayerList.Add(p_Player);

            var GuestVariables = DataProvider.GetPlayerVariables(1);
            PlayerList.Add(GuestVariables);

            StatsScreen.Visibility = Visibility.Visible;
            GameScreen.Visibility = Visibility.Visible;

            MainContent.Content = HistoryScreen;
        }

 

        private void GameLogic_GameStarted(object sender, EventArgs e)
        {
            GameScreen.StartGamePanel.Visibility = Visibility.Collapsed;
        }
    }



}
