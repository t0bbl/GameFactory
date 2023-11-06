
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
        Stats StatsScreen = new Stats();
        TTT TTTScreen = new TTT();

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = LoginScreen;
            StatsPanel.Children.Add(StatsScreen);
        }

        
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
            if (StatsPanel.Children.Contains(StatsScreen))
            {
                StatsScreen.Visibility = StatsScreen.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private void ToTTT(object sender, RoutedEventArgs e)
        {
            MainContent.Content = TTTScreen;

            //TTT tttWindow = new TTT();
            //tttWindow.ShowDialog();
        }


    }



}
