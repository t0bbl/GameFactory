using ClassLibrary;
using Newtonsoft.Json;
using System.IO;
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

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// Sets up the initial content and subscribes to necessary events.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = LoginScreen;
            LoginScreen.PlayerLoggedIn += LoginScreen_PlayerLoggedIn;
            LoadWindowPosition();
        }


        /// <summary>
        /// Handles the PlayerLoggedIn event from the LoginScreen.
        /// Loads the player home screen with the provided player information.
        /// </summary>
        /// <param name="p_Player">The player who logged in.</param>
        public void LoginScreen_PlayerLoggedIn(Player p_Player)
        {
            LoadPlayerHome(p_Player);
        }
        /// <summary>
        /// Loads the home screen for the specified player.
        /// Initializes and displays player's statistics, history, and game screen.
        /// </summary>
        /// <param name="p_Player">The player for whom to load the home screen.</param>
        public void LoadPlayerHome(Player p_Player)
        {
            HomePlayer = p_Player;

            StatsPanel.Children.Clear();
            StatsScreen = new Stats(p_Player);
            StatsPanel.Children.Add(StatsScreen);

            GameDetailPanel.Children.Clear();
            HistoryScreen = new History(p_Player);
            HistoryScreen.LoadHistory(p_Player);

            GameScreen = new GameWindow(this, p_Player);

            GameScreen.GameStartPanel.Visibility = Visibility.Visible;

            StatsScreen.Visibility = Visibility.Visible;

            InitializeHistoryScreen(p_Player);

            MainContent.Content = HistoryScreen;
        }
        /// <summary>
        /// Initializes the game screen for the specified player.
        /// Adds the game window to the GamesPanel.
        /// </summary>
        /// <param name="p_Player">The player for whom to initialize the game screen.</param>
        private void InitializeHistoryScreen(Player p_Player)
        {
            GameScreen = new GameWindow(this, p_Player);
            GameScreen.GameStartPanel.Visibility = Visibility.Visible;
            GameStartPanel.Children.Clear();
            GameStartPanel.Children.Add(GameScreen);
            GameScreen.Visibility = Visibility.Visible;
        }

        #region UI Event Handlers
        /// <summary>
        /// Handles the click event to log out the current user.
        /// Clears all panels and resets the application to the login screen.
        /// </summary>
        private void LogOut(object sender, RoutedEventArgs e)
        {
            StatsPanel.Children.Clear();
            OpponentStatsPanel.Children.Clear();
            GameDetailPanel.Children.Clear();
            GameStartPanel.Children.Clear();

            HistoryScreen = null;
            StatsScreen = null;
            GameScreen = null;

            LoginScreen = new Login();
            MainContent.Content = LoginScreen;
            LoginScreen.PlayerLoggedIn += LoginScreen_PlayerLoggedIn;
        }
        /// <summary>
        /// Handles the click event to navigate to the leaderboard screen.
        /// </summary>
        private void ToLeaderboard(object sender, RoutedEventArgs e)
        {
            Leaderboard LeaderBoardScreen = new Leaderboard();

            if (GameScreen != null)
            {
                GameScreen.Visibility = Visibility.Collapsed;
            }

            MainContent.Content = LeaderBoardScreen;
        }
        /// <summary>
        /// Toggles the visibility of the statistics screen.
        /// Shows an alert if no user is logged in.
        /// </summary>
        private void ToggleStats(object sender, RoutedEventArgs e)
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
        /// <summary>
        /// Navigates to the main content screen, showing the player's history.
        /// Ensures the game screen and stats screen are visible.
        /// </summary>
        private void ToMain(object sender, RoutedEventArgs e)
        {
            if (HistoryScreen == null)
            {
                Login.TextBox("Please log in to view history.");
                return;
            }
            OpponentStatsPanel.Children.Clear();

            LoadPlayerHome(HomePlayer);
        }

        #endregion
        #region Window Positioning
        /// <summary>
        /// Handles the window closing event.
        /// Saves the current window position for future sessions.
        /// </summary>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveWindowPosition();
        }
        /// <summary>
        /// Saves the current window position and size to a JSON file.
        /// </summary>
        private void SaveWindowPosition()
        {
            var windowPosition = new
            {
                Top = this.Top,
                Left = this.Left,
                Width = this.Width,
                Height = this.Height
            };

            File.WriteAllText("windowPosition.json", JsonConvert.SerializeObject(windowPosition));
        }
        /// <summary>
        /// Loads the window position and size from a JSON file if it exists.
        /// Sets the window to the center of the screen if the position is not found.
        /// </summary>
        private void LoadWindowPosition()
        {
            if (File.Exists("windowPosition.json"))
            {
                var windowPositionJson = File.ReadAllText("windowPosition.json");
                var windowPosition = JsonConvert.DeserializeObject<dynamic>(windowPositionJson);

                double top = (double)windowPosition.Top;
                double left = (double)windowPosition.Left;
                double width = (double)windowPosition.Width;
                double height = (double)windowPosition.Height;

                double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
                double virtualScreenHeight = SystemParameters.VirtualScreenHeight;
                double virtualScreenLeft = SystemParameters.VirtualScreenLeft;
                double virtualScreenTop = SystemParameters.VirtualScreenTop;

                double threshold = 0.50;

                bool isWindowVisible =
                    (left + width * threshold > virtualScreenLeft) &&
                    (top + height * threshold > virtualScreenTop) &&
                    (left < virtualScreenWidth - width * threshold) &&
                    (top < virtualScreenHeight - height * threshold);

                if (isWindowVisible)
                {
                    this.Top = top;
                    this.Left = left;
                    this.Width = width;
                    this.Height = height;
                }
                else
                {
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
        #endregion
    }
}
