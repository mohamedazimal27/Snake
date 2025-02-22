using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleSnake
{
    public partial class Form1 : Form
    {
        //Game declarations
        private List<Point> snake = new List<Point>();
        private Point food;
        private int direction = 0;
        private int score = 0;
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
            snake.Clear();
            snake.Add(new Point(15, 5));
            GenerateFood();

            gameTimer.Interval = 100;
            gameTimer.Tick += UpdateGame;
            gameTimer.Start();
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Right:
                    if (direction != 2) direction = 0;
                    break;
                case Keys.Down:
                    if (direction != 3) direction = 1;
                    break;
                case Keys.Left:
                    if (direction != 0) direction = 2;
                    break;
                case Keys.Up:
                    if (direction != 1) direction = 3;
                    break;
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush snakecolor = Brushes.Green;
            Brush foodcolor = Brushes.Red;

            foreach(Point p in snake)
            {
                g.FillRectangle(snakecolor, p.X * tileSize, p.Y * tileSize, tileSize, tileSize);
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
            //Move snake
            Point newHead = snake[0];

            switch(direction)
            {
                case 0: newHead.X += 1; break;
                case 1: newHead.Y += 1; break;
                case 2: newHead.X -= 1; break;
                case 3: newHead.Y -= 1; break;
            }

            //Collision check (body and wall)
            if (newHead.X < 0 || newHead.Y < 0 ||
                newHead.X >= this.ClientSize.Width / tileSize ||
                newHead.Y >= this.ClientSize.Height / tileSize ||
                snake.Contains(newHead))
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over! Score: " + score);
                //StartGame();
                return;
            }

            //Check food is eaten
            if (newHead == food)
            {
                score += 10;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            snake.Insert(0, newHead);
            lblScore.Text = "Score: " + score;
            this.Invalidate();
        }
    }
}
