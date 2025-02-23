using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2Snakes
{
    public partial class Form1 : Form
    {
        //Game declarations
        private List<Point> snake1 = new List<Point>();
        private List<Point> snake2 = new List<Point>();
        private Point food;
        private int direction1 = 0;  // Direction for snake 1
        private int direction2 = 0;  // Direction for snake 2
        private int score1 = 0;
        private int score2 = 0;
        private Timer gameTimer = new Timer();
        private Random rand = new Random();
        private const int tileSize = 20;

        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnKeyPress);
            this.Paint += new PaintEventHandler(OnPaint);
            StartGame();
        }

        private void StartGame()
        {
            snake1.Clear();
            snake1.Add(new Point(15, 5));
            snake2.Clear();
            snake2.Add(new Point(15, 15));
            GenerateFood();

            gameTimer.Interval = 100;
            gameTimer.Tick += UpdateGame;
            gameTimer.Start();
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Controls for Snake 1 (Arrow keys)
                case Keys.Right:
                    if (direction1 != 2) direction1 = 0;
                    break;
                case Keys.Down:
                    if (direction1 != 3) direction1 = 1;
                    break;
                case Keys.Left:
                    if (direction1 != 0) direction1 = 2;
                    break;
                case Keys.Up:
                    if (direction1 != 1) direction1 = 3;
                    break;

                // Controls for Snake 2 (WASD keys)
                case Keys.D:
                    if (direction2 != 2) direction2 = 0;
                    break;
                case Keys.S:
                    if (direction2 != 3) direction2 = 1;
                    break;
                case Keys.A:
                    if (direction2 != 0) direction2 = 2;
                    break;
                case Keys.W:
                    if (direction2 != 1) direction2 = 3;
                    break;
            }
        }


        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush snake1color = Brushes.Green;
            Brush snake2color = Brushes.Cyan;
            Brush foodcolor = Brushes.Red;

            foreach (Point p in snake1)
            {
                g.FillRectangle(snake1color, p.X * tileSize, p.Y * tileSize, tileSize, tileSize);
            }

            foreach (Point p in snake2)
            {
                g.FillRectangle(snake2color, p.X * tileSize, p.Y * tileSize, tileSize, tileSize);
            }

            g.FillRectangle(foodcolor, food.X * tileSize, food.Y * tileSize, tileSize, tileSize);
        }

        private void GenerateFood()
        {
            int maxX = this.ClientSize.Width / tileSize;
            int maxY = this.ClientSize.Height / tileSize;
            food = new Point(rand.Next(maxX), rand.Next(maxY));
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            // Move snake1
            Point newHead1 = snake1[0];
            switch (direction1)
            {
                case 0: newHead1.X += 1; break; // Move right
                case 1: newHead1.Y += 1; break; // Move down
                case 2: newHead1.X -= 1; break; // Move left
                case 3: newHead1.Y -= 1; break; // Move up
            }

            // Move snake2
            Point newHead2 = snake2[0];
            switch (direction2)
            {
                case 0: newHead2.X += 1; break; // Move right
                case 1: newHead2.Y += 1; break; // Move down
                case 2: newHead2.X -= 1; break; // Move left
                case 3: newHead2.Y -= 1; break; // Move up
            }

            // Collision check for both snakes
            if (newHead1.X < 0 || newHead1.Y < 0 || newHead1.X >= this.ClientSize.Width / tileSize || newHead1.Y >= this.ClientSize.Height / tileSize || snake1.Contains(newHead1) ||
                newHead2.X < 0 || newHead2.Y < 0 || newHead2.X >= this.ClientSize.Width / tileSize || newHead2.Y >= this.ClientSize.Height / tileSize || snake2.Contains(newHead2))
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over! Score1: " + score1 + " Score2: " + score2);
                return;
            }

            // Check if snake1 eats food
            if (newHead1 == food)
            {
                score1 += 10;
                GenerateFood();
            }
            else
            {
                snake1.RemoveAt(snake1.Count - 1);
            }

            // Check if snake2 eats food
            if (newHead2 == food)
            {
                score2 += 10;
                GenerateFood();
            }
            else
            {
                snake2.RemoveAt(snake2.Count - 1);
            }

            // Update snake positions
            snake1.Insert(0, newHead1);
            snake2.Insert(0, newHead2);

            label1.Text = "Score: " + score1;
            label3.Text = "Score: " + score2;
            this.Invalidate();
        }

    }
}
