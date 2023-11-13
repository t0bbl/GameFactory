using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for StartGames.xaml
    /// </summary>
    public partial class StartGames : UserControl
    {
        public StartGames()
        {
            InitializeComponent();
        }

        private void OnClickTTT(object sender, RoutedEventArgs e)
        {
            GameWindow TTTScreen = new GameWindow();
        }

        private void OnClick4w(object sender, RoutedEventArgs e)
        {
            GameWindow FourWinsScreen = new GameWindow();
        }

        private void OnClickTwist(object sender, RoutedEventArgs e)
        {
            GameWindow TwistScreen = new GameWindow();
        }

    }
}
