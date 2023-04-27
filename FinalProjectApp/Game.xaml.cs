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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FinalProjectApp
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        bool moveLeft, moveRight;
        
        List<Rectangle> itemstoremove = new List<Rectangle>();
        
        Random rand = new Random();
        int enemySpriteCounter; // int to help change enemy images
        int enemyCounter = 100; // enemy spawn time
        int playerSpeed = 10; // player movement speed
        int limit = 50; // limit of enemy spawns
        int score = 0; // default score
        int damage = 0; // default damage
        Rect playerHitBox; // player hit box to check for collision against enemy
        public Game()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += gameEngine;
            gameTimer.Start();
            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/purple.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;

            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
            player.Fill = playerImage;
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }

            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }

            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                MyCanvas.Children.Add(newBullet);
            }
        }
        private void makeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush();
            enemySpriteCounter = rand.Next(1, 5);
            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
                    break;
                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));
                    break;
                default:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
                    break;
            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };

            Canvas.SetTop(newEnemy, -100); // set the top position of the enemy to -100
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
            MyCanvas.Children.Add(newEnemy);
            GC.Collect();
        }

        private void gameEngine(object sender, EventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source = PC; Initial Catalog = FinalProject; Integrated Security = True");

            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            enemyCounter--;
            scoreText.Content = "Score: " + score; 
            damageText.Content = "Damage " + damage;
            if (enemyCounter < 0)
            {
                makeEnemies();
                enemyCounter = limit; 
            }

            if (moveLeft && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }

            if (moveRight && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>()) //enemy behavior
            {
                if (x is Rectangle && (string)x.Tag == "bullet") //enemy collision
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Canvas.GetTop(x) < 10)
                    {
                        itemstoremove.Add(x);
                    }
                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                            if (bullet.IntersectsWith(enemy))
                            {
                                itemstoremove.Add(x); 
                                itemstoremove.Add(y); 
                                score++; 
                            }
                        }
                    }
                }
                if (x is Rectangle && (string)x.Tag == "enemy") //enemy passes
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10);
                    Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (Canvas.GetTop(x) + 150 > 700)
                    {
                        itemstoremove.Add(x);
                        damage += 10;
                    }
                    if (playerHitBox.IntersectsWith(enemy))
                    {
                        damage += 5; 
                        itemstoremove.Add(x); 
                    }
                }
            }

            if (score > 5)
            {
                limit = 20;
            }


            if (damage > 99)
            {
                gameTimer.Stop();
                damageText.Content = "Damaged: 100";
                damageText.Foreground = Brushes.Red;

                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                    string queryScore = "UPDATE users SET lastScore=@Score WHERE id=@Id;";
                    string queryBestScore = "SELECT bestScore FROM users WHERE id=@Id;";
                    SqlCommand sqlCmdScore = new SqlCommand(queryScore, sqlCon);
                    SqlCommand sqlCmdBestScore = new SqlCommand(queryBestScore, sqlCon);
                    sqlCmdScore.Parameters.AddWithValue("@Score", score);
                    sqlCmdScore.Parameters.AddWithValue("@Id", GlobalVariables.GlobalID);
                    sqlCmdBestScore.Parameters.AddWithValue("@Id", GlobalVariables.GlobalID);
                    int v = sqlCmdScore.ExecuteNonQuery();
                    v = (int)sqlCmdBestScore.ExecuteScalar();
                    if (score > v)
                    {
                        MessageBox.Show("Well Done! You beat your old best score!");
                        string queryNewRecord = "UPDATE users SET bestScore=@Score WHERE id=@Id;";
                        SqlCommand sqlCmdRecord = new SqlCommand(queryNewRecord, sqlCon);
                        sqlCmdRecord.Parameters.AddWithValue("@Score", score);
                        sqlCmdRecord.Parameters.AddWithValue("@Id", GlobalVariables.GlobalID);
                        v = sqlCmdRecord.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("You didn't beat your best score!");
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
                MessageBox.Show("You have destroyed " + score + " Alien ships");
                this.Close();
            }

            foreach (Rectangle y in itemstoremove)
            {
                MyCanvas.Children.Remove(y);
            }
        }
    }
}
