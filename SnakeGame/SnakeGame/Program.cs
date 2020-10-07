using System;
using System.Diagnostics;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // start game
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // display this char on the console during the game
            char ch = '*';
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed

            // location info & display
            int x = 0, y = 2; // y is 2 to allow the top row for directions & space
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            
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
            //obstacles5 = randomNum.Next(5, consoleWidthLimit);

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

            do // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit.");
                Console.SetCursorPosition(x, y);
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
                            dx = 0;
                            dy = -1;
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            dx = 0;
                            dy = 1;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            dx = -1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            dx = 1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case ConsoleKey.Escape: //END
                            gameLive = false;
                            break;
                    }
                }

                // find the current position in the console grid & erase the character there if don't want to see the trail
                Console.SetCursorPosition(x, y);
                if (trail == false)
                    Console.Write(' ');

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

                // write the character in the new position
                Console.SetCursorPosition(x, y);
                Console.Write(ch);

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

                //generate a food when the snake ate it
                    if (obstacles1X != foodX && obstacles1Y != foodY || obstacles2X != foodX && obstacles2Y != foodY && obstacles3X != foodX && obstacles3Y != foodY || obstacles4X != foodX && obstacles4Y != foodY)
                    {
                    Console.SetCursorPosition(foodX, foodY);
                    Console.Write(foodS);
                    }
                    else
                    {
                        foodX = randomNum.Next(1, consoleWidthLimit);
                        foodY = randomNum.Next(2, consoleHeightLimit);
                        Console.SetCursorPosition(foodX, foodY);
                        Console.Write(foodS);
                    }

                
                //set a timer to change the position of the food every 5 seconds
                if(timer == 100)
                {
                    Console.SetCursorPosition(foodX, foodY);
                    if(five == true)
                    {
                        Console.Write(" ");
                    }
                    foodX = randomNum.Next(1, consoleWidthLimit);
                    foodY = randomNum.Next(2, consoleHeightLimit);
                    Console.SetCursorPosition(foodX, foodY);
                    Console.Write(foodS);
                    timer = 0;
                }

                //Set a condition to the obstacle when the snake hits it
                if (x == obstacles1X && y == obstacles1Y)
                {
                    gameLive = false;
                }
                else
                {
                    Console.SetCursorPosition(obstacles1X, obstacles1Y);
                    Console.Write(obs);
                }

                if (x == obstacles2X && y == obstacles2Y)
                {
                    gameLive = false;
                }
                else
                {
                    Console.SetCursorPosition(obstacles2X, obstacles2Y);
                    Console.Write(obs);
                }


                if (x == obstacles3X && y == obstacles3Y)
                {
                    gameLive = false;
                }
                else
                {
                    Console.SetCursorPosition(obstacles3X, obstacles3Y);
                    Console.Write(obs);
                }


                if (x == obstacles4X && y == obstacles4Y)
                {
                    gameLive = false;
                }
                else
                {
                    Console.SetCursorPosition(obstacles4X, obstacles4Y);
                    Console.Write(obs);
                }


            } while (gameLive);
        }
    }
}
