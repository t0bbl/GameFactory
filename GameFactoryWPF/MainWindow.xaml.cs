
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
        }


        private void ToMainScreen(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Login();

        }

        private void ToLeaderboard(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Leaderboard();

        }
        private void ToStats(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Stats();

            //Stats statsWindow = new Stats();
            //statsWindow.ShowDialog();
        }
        private void ToTTT(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new TTT();

            //TTT tttWindow = new TTT();
            //tttWindow.ShowDialog();
        }


    }



}
