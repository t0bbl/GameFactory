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
using ClassLibrary;

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
            this.MouseDown += MainWindow_MouseDown;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WelcomeSection.Visibility = Visibility.Collapsed;
            LoginSection.Visibility = Visibility.Visible;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MessageBox.Show("The button labeled '" + button.Content + "' has been clicked.");

        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("The Login Event was triggerd with a Enter in Login/password.");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MessageBox.Show("The button labeled '" + button.Content + "' has been clicked.");
        }

        private void ToLeaderboard(object sender, RoutedEventArgs e)
        {
            LeaderBoard leaderBoardWindow = new LeaderBoard();
            leaderBoardWindow.ShowDialog();
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
