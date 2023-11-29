using ClassLibrary;
using System.Windows;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Login LoginScreen = new Login();
        Stats StatsScreen;
        History HistoryScreen;
        GameWindow GameScreen;
        Player HomePlayer;

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = LoginScreen;
            LoginScreen.PlayerLoggedIn += LoginScreen_PlayerLoggedIn;
        }

        public void LoginScreen_PlayerLoggedIn(Player p_Player)
        {
            LoadPlayerHome(p_Player);
        }

        public void LoadPlayerHome(Player p_Player)
        {

            StatsPanel.Children.Clear();
            StatsScreen = new Stats(p_Player);
            StatsPanel.Children.Add(StatsScreen);

            HistoryPanel.Children.Clear();
            HistoryScreen = new History(p_Player);
            HistoryScreen.LoadHistory(p_Player);

            HomePlayer = p_Player;

            StatsScreen.Visibility = Visibility.Visible;

            InitializeGameScreen(HomePlayer);

            MainContent.Content = HistoryScreen;
        }




        #region UI Event Handlers
        private void ToMainScreen(object sender, RoutedEventArgs e)
        {
            if (GameScreen != null)
            {
                GameScreen.Visibility = Visibility.Collapsed;
            }
            if (StatsScreen != null)
            {
                StatsScreen.Visibility = Visibility.Collapsed;
            }

            MainContent.Content = LoginScreen;
        }
        private void ToLeaderboard(object sender, RoutedEventArgs e)
        {
            Leaderboard LeaderBoardScreen = new Leaderboard();

            if (GameScreen != null)
            {
                GameScreen.Visibility = Visibility.Collapsed;
            }

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
            GameScreen.Visibility = Visibility.Visible;
            StatsScreen.Visibility = Visibility.Visible;
        }
        #endregion

        private void InitializeGameScreen(Player p_Player)
        {
            GameScreen = new GameWindow(this, p_Player, StatsScreen);
            GameScreen.StartGamePanel.Visibility = Visibility.Visible;
            GamesPanel.Children.Clear();
            GamesPanel.Children.Add(GameScreen);
            GameScreen.Visibility = Visibility.Visible;
        }
    }



}
