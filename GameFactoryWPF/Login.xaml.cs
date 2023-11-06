using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameFactoryWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public int Welcomed { get; set; } = 0;
        private string Username { get; set; }
        private string Password { get; set; }
        public ClassLibrary.Player Gamer { get; set; }
        private int p_Ident { get; set; }





        public Login()
        {
            InitializeComponent();
            this.MouseDown += MainWindow_MouseDown;
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Welcomed == 0)
            {
                WelcomeSection.Visibility = Visibility.Collapsed;
                LoginSection.Visibility = Visibility.Visible;
                Welcomed = 1;
            }
            
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string Username = UsernameTextBox.Text;
            string Password = PasswordTextBox.Password;
            string PasswordSave = ClassLibrary.Player.HashPassword(Password);

            ClassLibrary.Player Gamer = new ClassLibrary.Player();

            p_Ident = Gamer.SQLLoginPlayer(Username, PasswordSave);

            if (p_Ident != 0)
            {
                MessageBox.Show("Logged in as " + Username + " !");
            }
            else
            {
                MessageBox.Show("Login Failed! Please try again."); 
            };

            

        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            MessageBox.Show("The button labeled '" + button.Content + "' has been clicked.");
        }


    }
}
