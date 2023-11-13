using ClassLibrary;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for StartGames.xaml
    /// </summary>
    public partial class StartGames : UserControl
    {
        private MainWindow p_MainWindow;
        public StartGames(MainWindow p_MainWindow)
        {
            InitializeComponent();
            this.p_MainWindow = p_MainWindow;
        }

        private void OnClickTTT(object sender, RoutedEventArgs e)
        {
            var TTTGame = new TTT();
            GameWindow TTTScreen = new GameWindow(TTTGame, p_MainWindow);
        }

        private void OnClick4w(object sender, RoutedEventArgs e)
        {
            var FourWGame = new FourW();
            GameWindow FourWinsScreen = new GameWindow(FourWGame, p_MainWindow);
        }

        private void OnClickTwist(object sender, RoutedEventArgs e)
        {
            var CostumTTTGame = new CustomTTT(true);
            GameWindow TwistScreen = new GameWindow(CostumTTTGame, p_MainWindow);
        }

    }
}
