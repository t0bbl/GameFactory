using ClassLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for Stats.xaml
    /// </summary>
    public partial class Stats : UserControl
    {
        private Player HomePlayer;

        public Stats(Player p_Player)
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            HomePlayer = p_Player;
            var Color = p_Player.Color;
            this.DataContext = HomePlayer;
        }

        #region Change Icon and Color

        private void ApplyTextChange()
        {
            DisplayTextBlock.Text = EditTextBox.Text;
            EditTextBox.Visibility = Visibility.Collapsed;
            DisplayTextBlock.Visibility = Visibility.Visible;
            HomePlayer.SQLSavePlayerVariables(HomePlayer.Ident, HomePlayer.Name, HomePlayer.Icon, HomePlayer.Color);
        }

        private void HandleColorChange()
        {
            if (ColorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var ColorName = selectedItem.Content.ToString();

                var Color = (Color)ColorConverter.ConvertFromString(ColorName);
                ColorComboBox.Background = new SolidColorBrush(Color);

                HomePlayer.Color = ColorName;

                HomePlayer.SQLSavePlayerVariables(HomePlayer.Ident, HomePlayer.Name, HomePlayer.Icon, HomePlayer.Color);
            }
        }

        #endregion
        #region EventArgs
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandleColorChange();
        }
        private void DisplayTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayTextBlock.Visibility = Visibility.Collapsed;
            EditTextBox.Visibility = Visibility.Visible;
            EditTextBox.Text = DisplayTextBlock.Text;
            EditTextBox.SelectAll();
            EditTextBox.Focus();
        }
        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplyTextChange();
        }
        private void EditTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyTextChange();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
