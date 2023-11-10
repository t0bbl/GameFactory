using ClassLibrary;
using System.Windows;
using System.Windows.Controls;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for Stats.xaml
    /// </summary>
    public partial class Stats : UserControl
    {

        public Stats(Player p_Player)
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            this.DataContext = p_Player;
        }




        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
