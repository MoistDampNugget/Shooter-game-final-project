using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Window
    {
        public Leaderboard()
        {
            InitializeComponent();
        }

        private void Return(object sender, RoutedEventArgs e)
        {
            StartGame x = new StartGame();
            x.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source = PC; Initial Catalog = FinalProject; Integrated Security = True");
            try
            {
                sqlCon.Open();
                String query = "SELECT username, bestScore FROM users ORDER BY bestScore DESC;";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                SqlDataAdapter word = new SqlDataAdapter();
                word.SelectCommand = sqlCmd;

                DataTable dt = new DataTable(" ");
                word.Fill(dt);
                DataGrid1.ItemsSource = dt.DefaultView;
                word.Update(dt);
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
