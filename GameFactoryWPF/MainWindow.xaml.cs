using ClassLibrary;
using Microsoft.Azure.Amqp.Framing;
using Newtonsoft.Json;
using System.Windows;
using System.IO;

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

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = LoginScreen;
            LoginScreen.PlayerLoggedIn += LoginScreen_PlayerLoggedIn;
            LoadWindowPosition();
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

            GameScreen = new GameWindow(this, p_Player);

            GameScreen.StartGamePanel.Visibility = Visibility.Visible;

            StatsScreen.Visibility = Visibility.Visible;

            InitializeGameScreen(p_Player);

            MainContent.Content = HistoryScreen;
        }

        private void InitializeGameScreen(Player p_Player)
        {
            GameScreen = new GameWindow(this, p_Player);
            GameScreen.StartGamePanel.Visibility = Visibility.Visible;
            GamesPanel.Children.Clear();
            GamesPanel.Children.Add(GameScreen);
            GameScreen.Visibility = Visibility.Visible;
        }

        #region UI Event Handlers
        private void LogOut(object sender, RoutedEventArgs e)
        {
            StatsPanel.Children.Clear();
            HistoryPanel.Children.Clear();
            GamesPanel.Children.Clear();

            HistoryScreen = null;
            StatsScreen = null;
            GameScreen = null;

            LoginScreen = new Login();
            MainContent.Content = LoginScreen;
            LoginScreen.PlayerLoggedIn += LoginScreen_PlayerLoggedIn;
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
        private void ToMain(object sender, RoutedEventArgs e)
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
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveWindowPosition();
        }
        #endregion
        #region Window Positioning
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
