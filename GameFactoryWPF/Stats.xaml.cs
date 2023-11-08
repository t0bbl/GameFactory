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
        public Player Player
        {
            get { return (Player)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }
        public static readonly DependencyProperty PlayerProperty =
      DependencyProperty.Register("Player", typeof(Player), typeof(Stats), new PropertyMetadata(null, OnPlayerPropertyChanged));

        public Stats(Player p_Player)
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            this.DataContext = p_Player;
        }

        public void UpdatePlayer(Player p_Player)
        {
            this.DataContext = p_Player;
        }

        private static void OnPlayerPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Stats;
            if (control != null)
            {
                Player newPlayer = e.NewValue as Player;
                control.DataContext = newPlayer; 
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
