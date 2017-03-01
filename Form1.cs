using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace Tron
{
    public partial class Form1 : Form
    {
        private Field[,] gameField = new Field[45, 30];
  
        private Motor moto1;
        private Motor moto2;

        Thread mot1;
        Thread mot2;

        Thread Screen;

        public Form1()
        {
            InitializeComponent();
            new Settings();

            Screen = new Thread(new ThreadStart(this.UpdateScreen));
            Screen.Start();

            StartGame();

        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Settings();

            for (int i = 0; i < 45; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Field testf = new Field();
                    gameField[i, j] = testf;
                }
            }

            moto1 = new Motor(ref gameField);
            moto1.X = 10;
            moto1.Y = 5;
            moto1.ID = 0;

            moto2 = new Motor(ref gameField);
            moto2.X = 20;
            moto2.Y = 5;
            moto2.ID = 1;
            moto2.Up = Key.W;
            moto2.Down = Key.S;
            moto2.Left = Key.A;
            moto2.Right = Key.D;

            mot1 = new Thread(new ThreadStart(moto1.Update));
            mot2 = new Thread(new ThreadStart(moto2.Update));

            mot1.Start();
            mot2.Start();

        }

        private void UpdateScreen()
        {
            while (true)
            {
                pbCanvas.Invalidate();
            }
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                Brush BikeColor;

                for (int i = 0; i < 45; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        if (!gameField[i, j].avaible)
                        {
                            if(gameField[i,j].MotorId == 0) BikeColor = Brushes.LightCyan;
                            else BikeColor = Brushes.LightBlue;

                            canvas.FillRectangle(BikeColor,
                            new Rectangle(i* Settings.Width,
                            j * Settings.Height,
                            Settings.Width, Settings.Height));
                        }
                    }
                }
                BikeColor = Brushes.Green;
                canvas.FillRectangle(BikeColor,
                    new Rectangle(moto1.X * Settings.Width,
                                  moto1.Y * Settings.Height,
                                  Settings.Width, Settings.Height));
                BikeColor = Brushes.Blue;
                canvas.FillRectangle(BikeColor,
                    new Rectangle(moto2.X * Settings.Width,
                                  moto2.Y * Settings.Height,
                                  Settings.Width, Settings.Height));
            }
            else
            {
                string gameOver = "Game over \n\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            moto1.CheckInput();
            moto2.CheckInput();
            if (Settings.GameOver)
            {
                mot1.Abort();
                mot2.Abort();
                if (Keyboard.IsKeyDown(Key.Enter))
                {
                    StartGame();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Screen.Abort();
            Screen.Join();
            mot1.Abort();
            mot1.Join();
            mot2.Abort();
            mot2.Join();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Screen.Abort();
            Screen.Join();
            mot1.Abort();
            mot1.Join();
            mot2.Abort();
            mot2.Join();
        }
    }
}
