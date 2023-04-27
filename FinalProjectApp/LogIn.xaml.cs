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
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source = PC; Initial Catalog = FinalProject; Integrated Security = True");

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                string query = "SELECT COUNT(1) FROM users WHERE Username=@Username AND Password=@Password;";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count >= 1)
                {
                    MessageBox.Show("Account Exists!");
                    string queryID = "SELECT id FROM users WHERE Username=@Username AND Password=@Password;";
                    SqlCommand sqlCmdID = new SqlCommand(queryID, sqlCon);
                    sqlCmdID.Parameters.AddWithValue("@Username", txtUsername.Text);
                    sqlCmdID.Parameters.AddWithValue("@Password", txtPassword.Text);
                    int id = Convert.ToInt32(sqlCmdID.ExecuteScalar());
                    GlobalVariables.GlobalID = id;
                    StartGame x = new StartGame();
                    x.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Something is Wrong!");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();
            }
        }
        private void no_acc_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow x = new MainWindow();
            x.Show();
            this.Close();
        }
    }
}
