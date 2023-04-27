using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source = PC; Initial Catalog = FinalProject; Integrated Security = True");

            try
            {
                sqlCon.Open();

                string query = "Insert into users(username, password)values('" + this.txtUsername.Text + "','" + this.txtPassword.Text + "')";
                SqlCommand cmd = new SqlCommand(query, sqlCon);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Successfully saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
                LogIn x = new LogIn();
                x.Show();
                this.Close();
            }
        }

        private void alracc_Click(object sender, RoutedEventArgs e)
        {
            LogIn x = new LogIn();
            x.Show();
            this.Close();
        }
    }
}
