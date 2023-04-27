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
using System.Windows.Shapes;

namespace FinalProjectApp
{
    /// <summary>
    /// Interaction logic for StartGame.xaml
    /// </summary>
    public partial class StartGame : Window
    {
        public StartGame()
        {
            InitializeComponent();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Starting Game");
            Game x = new Game();
            x.Show();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Scoreboard(object sender, RoutedEventArgs e)
        {
            Leaderboard x = new Leaderboard();
            x.Show();
            this.Close();
        }
    }
}
