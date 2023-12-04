using ClassLibrary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for Stats.xaml.
    /// This class provides the user interface for displaying and editing player statistics, such as wins, losses, draws, and customizations like icon and color.
    /// </summary>
    public partial class Stats : UserControl
    {
        private Player HomePlayer;
        /// <summary>
        /// Initializes a new instance of the Stats class for a given player.
        /// Sets up UI elements and binds event handlers for interaction.
        /// </summary>
        /// <param name="p_Player">The player whose statistics are to be displayed and edited.</param>
        public Stats(Player p_Player)
        {
            InitializeComponent();
            CloseButton.Click += CloseButton_Click;
            HomePlayer = p_Player;
            this.DataContext = HomePlayer;
        }

        #region Change Icon and Color
        /// <summary>
        /// Applies changes to the player's icon text and updates the database with the new values.
        /// </summary>
        private void ApplyTextChange()
        {
            DisplayTextBlock.Text = EditTextBox.Text;
            EditTextBox.Visibility = Visibility.Collapsed;
            DisplayTextBlock.Visibility = Visibility.Visible;
            HomePlayer.SQLSavePlayerVariables(HomePlayer.Ident, HomePlayer.Name, HomePlayer.Icon, HomePlayer.Color);
        }
        /// <summary>
        /// Handles changes to the player's color selection and updates the database with the new color value.
        /// </summary>
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
        /// <summary>
        /// Handles the selection change in the color combo box and updates the player's color.
        /// </summary>
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandleColorChange();
        }
        /// <summary>
        /// Handles mouse click events on the icon display text block, switching to edit mode.
        /// </summary>
        private void DisplayTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayTextBlock.Visibility = Visibility.Collapsed;
            EditTextBox.Visibility = Visibility.Visible;
            EditTextBox.Text = DisplayTextBlock.Text;
            EditTextBox.SelectAll();
            EditTextBox.Focus();
        }
        /// <summary>
        /// Applies changes when focus is lost from the icon edit text box.
        /// </summary>
        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ApplyTextChange();
        }
        /// <summary>
        /// Handles key up events in the icon edit text box, applying changes when the Enter key is pressed.
        /// </summary>
        private void EditTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyTextChange();
            }
        }
        /// <summary>
        /// Closes the statistics view when the close button is clicked.
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
