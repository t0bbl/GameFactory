
using System.Windows;


namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new Login();
        }



        private void ToLeaderboard(object sender, RoutedEventArgs e)
        {
            LeaderBoard LeaderBoardWindow = new LeaderBoard();
            MainContent.Content = LeaderBoardWindow;
        }
        private void ToStats(object sender, RoutedEventArgs e)
        {
            Stats statsWindow = new Stats();
            statsWindow.ShowDialog();
        }
        private void ToTTT(object sender, RoutedEventArgs e)
        {
            TTT tttWindow = new TTT();
            tttWindow.ShowDialog();
        }


    }



}
