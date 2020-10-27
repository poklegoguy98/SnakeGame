using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;

namespace SnakeGame
{
    //Structure for defining the coordinates in the game
    struct Coordinate
    {
        public int row;
        public int column;
        public Coordinate(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }

    class Program
    {
        // method to declare an array to store the directions of the snake
        public void Direction(Coordinate[] direction)
        {
            direction[0] = new Coordinate(0,1); //right
            direction[1] = new Coordinate(0,-1); //left
            direction[2] = new Coordinate(1,0); //down
            direction[3] = new Coordinate(-1,0); //up
        }

        static void Main(string[] args)
        {
            // start game
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // display this char on the console during the game
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed

            // location info & display
            int x = 0, y = 2; // y is 2 to allow the top row for directions & space
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            byte right = 0, left = 1, down = 2, up = 3;
            int direc = right;
            Program program = new Program();
            Coordinate[] direction = new Coordinate[4];
            program.Direction(direction);

            // initialise and generate the snake body
            // set the snake to start moving from the top left corner by default
            Queue<Coordinate> theSnek = new Queue<Coordinate>();
            int i;
            for(i = 0; i <=3; i++)
            {
                theSnek.Enqueue(new Coordinate(2, i));
            }
            foreach(Coordinate coordinate in theSnek)
            {
                Console.SetCursorPosition(coordinate.column, coordinate.row);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("*");
            }

            // clear to color
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 50;

            // whether to keep trails
            bool trail = false;

            //generate a food when the game start
            int foodX = 0;
            int foodY = 0;
            Random randomNum = new Random();
            char foodS = '$';
            foodX = randomNum.Next(1, consoleWidthLimit);
            foodY = randomNum.Next(2, consoleHeightLimit);
            Console.SetCursorPosition(foodX, foodY);
            Console.Write(foodS);

            //Generate random obstacle when the game starts
            int obstacles1X, obstacles1Y, obstacles2X, obstacles2Y, obstacles3X, obstacles3Y, obstacles4X, obstacles4Y = 0;
            string obs = "||";
            obstacles1X = randomNum.Next(1, consoleWidthLimit);
            obstacles1Y = randomNum.Next(2, consoleHeightLimit);

            obstacles2X = randomNum.Next(1, consoleWidthLimit);
            obstacles2Y = randomNum.Next(2, consoleHeightLimit);


            obstacles3X = randomNum.Next(1, consoleWidthLimit);
            obstacles3Y = randomNum.Next(2, consoleHeightLimit);

            obstacles4X = randomNum.Next(1, consoleWidthLimit);
            obstacles4Y = randomNum.Next(2, consoleHeightLimit);

            Console.SetCursorPosition(obstacles1X, obstacles1Y);
            Console.Write(obs);

            Console.SetCursorPosition(obstacles2X, obstacles2Y);
            Console.Write(obs);

            Console.SetCursorPosition(obstacles3X, obstacles3Y);
            Console.Write(obs);

            Console.SetCursorPosition(obstacles4X, obstacles4Y);
            Console.Write(obs);

            //time span variables
            int timer = 0;
            bool five = true;
            bool stop = true; 

            //score variable
            int gameScore = 0;

            string snakeL = " ";

            do // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit. Reach 5 points to win the game.");
                Console.WriteLine("Your score: " + gameScore);
                Console.ForegroundColor = cc;
                timer++;

                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {

                        case ConsoleKey.UpArrow: //UP
                            if(direc != down)
                            {
                                direc = up;
                            }
                            Console.ForegroundColor = ConsoleColor.Red;              
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            if(direc != up)
                            {
                                direc = down;
                            }
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            if(direc != right)
                            {
                                direc = left;
                            }
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            if(direc != left)
                            {
                                direc = right;
                            }    
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case ConsoleKey.Escape: //END
                            gameLive = false;
                            break;
                    }
                }

                // initialise and indicate the head of the snake
                Coordinate snakeH = theSnek.Last();
                Coordinate direcN = direction[direc];
                int snakeNR = snakeH.row + direcN.row;
                int snakeNC = snakeH.column + direcN.column;
                Coordinate snakeHN = new Coordinate(snakeNR, snakeNC);

                // set conditions when the snake went out of the game window
                if (snakeHN.column < 0)
                {
                    snakeHN.column = consoleWidthLimit - 1;
                }
                if (snakeHN.row < 2)
                {
                    snakeHN.row = consoleHeightLimit - 1;
                }
                if (snakeHN.column >= consoleWidthLimit)
                {
                    snakeHN.column = 0;
                }
                if (snakeHN.row >= consoleHeightLimit)
                {
                    snakeHN.row = 2;
                }
                

                // calculate the new position
                // note x set to 0 because we use the whole width, but y set to 1 because we use top row for instructions
                x += dx;
                if (x > consoleWidthLimit)
                    x = 0;
                if (x < 0)
                    x = consoleWidthLimit;

                y += dy;
                if (y > consoleHeightLimit)
                    y = 2; // 2 due to top spaces used for directions
                if (y < 2)
                    y = consoleHeightLimit;

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

                Console.SetCursorPosition(snakeH.column, snakeH.row);
                Console.Write("*");
                theSnek.Enqueue(snakeHN);

                //set a timer to change the position of the food every 5 seconds
                if (timer == 100)
                {
                    Console.SetCursorPosition(foodX, foodY);
                    if(five == true)
                    {
                        Console.Write(snakeL);
                    }
                    foodX = randomNum.Next(1, consoleWidthLimit);
                    foodY = randomNum.Next(2, consoleHeightLimit);
                    do
                    {
                        if (obstacles1X != foodX && obstacles1Y != foodY ||
                            obstacles2X != foodX && obstacles2Y != foodY ||
                            obstacles3X != foodX && obstacles3Y != foodY ||
                            obstacles4X != foodX && obstacles4Y != foodY)
                        {
                            Console.SetCursorPosition(foodX, foodY);
                        }
                        else
                        {
                            foodX = randomNum.Next(1, consoleWidthLimit);
                            foodY = randomNum.Next(2, consoleHeightLimit);
                            Console.SetCursorPosition(foodX, foodY);
                        }
                    } while (obstacles1X == foodX && obstacles1Y == foodY ||
                    obstacles2X == foodX && obstacles2Y == foodY ||
                    obstacles3X == foodX && obstacles3Y == foodY ||
                    obstacles4X == foodX && obstacles4Y == foodY);
                    Console.Write(foodS);
                    timer = 0;
                }

                //Increase score when the player ate a food
                if(snakeHN.column == foodX && snakeHN.row == foodY)
                {
                    gameScore++;
                    foodX = randomNum.Next(1, consoleWidthLimit);
                    foodY = randomNum.Next(2, consoleHeightLimit);
                    do
                    {
                        if (obstacles1X != foodX && obstacles1Y != foodY ||
                            obstacles2X != foodX && obstacles2Y != foodY ||
                            obstacles3X != foodX && obstacles3Y != foodY ||
                            obstacles4X != foodX && obstacles4Y != foodY)
                        {
                            Console.SetCursorPosition(foodX, foodY);
                        }
                        else
                        {
                            foodX = randomNum.Next(1, consoleWidthLimit);
                            foodY = randomNum.Next(2, consoleHeightLimit);
                            Console.SetCursorPosition(foodX, foodY);
                        }
                    } while (obstacles1X == foodX && obstacles1Y == foodY ||
                    obstacles2X == foodX && obstacles2Y == foodY ||
                    obstacles3X == foodX && obstacles3Y == foodY ||
                    obstacles4X == foodX && obstacles4Y == foodY);
                    Console.Write(foodS);
                    timer = 0;
                }
                else
                {
                    // find the coordinate and erase the trail behind the snake body
                    Coordinate trailL = theSnek.Dequeue();
                    Console.SetCursorPosition(trailL.column, trailL.row);
                    Console.Write(snakeL);
                }

                //Winning requirement: Snake eats 5 food
                if (gameScore == 5)
                {
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("GAME CLEAR!!\n                                                   YOU WIN!!!\n                                               PRESS ENTER TO EXIT");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    return;
                }

                //The game ends when the snake hits the obstacles
                if (snakeHN.column == obstacles1X && snakeHN.row == obstacles1Y)
                {
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("GAME OVER!!\n                                               PRESS ENTER TO EXIT");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    return;
                }
               
                if (snakeHN.column == obstacles2X && snakeHN.row == obstacles2Y)
                {
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("GAME OVER!!\n                                               PRESS ENTER TO EXIT");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    return;
                }

                if (snakeHN.column == obstacles3X && snakeHN.row == obstacles3Y)
                {
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("GAME OVER!!\n                                               PRESS ENTER TO EXIT");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    return;
                }

                if (snakeHN.column == obstacles4X && snakeHN.row == obstacles4Y)
                {
                    Console.Clear();
                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("GAME OVER!!\n                                               PRESS ENTER TO EXIT");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    return;
                }

            } while (gameLive);
        }
    }
}
