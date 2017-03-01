using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace Tron
{
    class Motor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ID { get; set; }
        public Direction direction { get; set; }
        public Key Up { get; set; }
        public Key Left { get; set; }
        public Key Right { get; set; }
        public Key Down { get; set; }
        public Field[,] gameField;

        public Motor(ref Field[,] gField)
        {
            X = 0;
            Y = 0;
            ID = 0;
            direction = Direction.Down;
            Up = Key.Up;
            Down = Key.Down;
            Left = Key.Left;
            Right = Key.Right;
            gameField = gField;
        }

        public void CheckInput()
        {
            if (Keyboard.IsKeyDown(Right) && direction != Direction.Left)
                direction = Direction.Right;
            else if (Keyboard.IsKeyDown(Left) && direction != Direction.Right)
                direction = Direction.Left;
            else if (Keyboard.IsKeyDown(Up) && direction != Direction.Down)
                direction = Direction.Up;
            else if (Keyboard.IsKeyDown(Down) && direction != Direction.Up)
                direction = Direction.Down;
        }



        private void MoveMotor()
        {
            switch (direction)
            {
                case Direction.Right:
                    this.X++;
                    break;
                case Direction.Left:
                    this.X--;
                    break;
                case Direction.Up:
                    this.Y--;
                    break;
                case Direction.Down:
                    this.Y++;
                    break;
            }

            int maxXPos = 720 / Settings.Width;
            int maxYPos = 480 / Settings.Height;

            //Detect collission with game borders.
            if (this.X < 0 || this.Y < 0 || this.X >= maxXPos || this.Y >= maxYPos)
            {
                Boom();
                return;
            }
            //check is field availble
            Monitor.Enter(gameField);
            if (!gameField[this.X, this.Y].avaible)
            {
                Boom();
            }
            else
            {
                gameField[this.X, this.Y].avaible = false;
                gameField[this.X, this.Y].MotorId = this.ID;
            }
            Monitor.Exit(gameField);

        }

        private void Boom()
        {
            Settings.GameOver = true;
        }

        public void Update()
        {
            while (true)
            {
                Thread.Sleep(100);
                MoveMotor();
            }
        }

    }
}
