using ClassLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;



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
        public ClassLibrary.Player Player { get; set; }
        private int p_Ident { get; set; }





        public Login()
        {
            InitializeComponent();
            this.MouseDown += MainWindow_MouseDown;

            LoginSection.Visibility = Visibility.Hidden;
            LoginSection.RenderTransform = new TransformGroup()
            {
                Children = new TransformCollection()
        {
            new ScaleTransform() { ScaleX = -1 }
        }
            };
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Welcomed == 0)
            {
                Storyboard welcomeFlipOut = this.Resources["WelcomeFlipOut"] as Storyboard;
                if (welcomeFlipOut != null)
                {
                    welcomeFlipOut.Completed += WelcomeFlipOut_Completed;
                    welcomeFlipOut.Begin();
                }

                Welcomed = 1;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Player Player = new Player();

            Username = UsernameTextBox.Text;
            Password = PasswordTextBox.Password;
            string PasswordSave = ClassLibrary.Player.HashPassword(Password);


            p_Ident = Player.SQLLoginPlayer(Username, PasswordSave);

            if (p_Ident != 0)
            {
                MessageBox.Show("Logged in as " + Username + " !");
                Player = ClassLibrary.DataProvider.GetPlayerVariables(p_Ident);

            }
            else
            {
                MessageBox.Show("Wrong UserName or Password, Please try again or Signup!"); 
            };
        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Storyboard flipAnimation = this.Resources["FlipAnimation"] as Storyboard;
            if (flipAnimation != null)
            {
                flipAnimation.Begin();
            }
        }
        private void WelcomeFlipOut_Completed(object sender, EventArgs e)
        {
            WelcomeSection.Visibility = Visibility.Collapsed;

            LoginSection.Visibility = Visibility.Visible;

            Storyboard loginFlipIn = this.Resources["LoginFlipIn"] as Storyboard;
            if (loginFlipIn != null)
            {
                loginFlipIn.Begin();

            }

        }



    }
}
