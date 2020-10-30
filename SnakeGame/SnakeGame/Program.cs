using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using WMPLib;


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
        private static int indicator = 0;

        // method to declare an array to store the directions of the snake
        public void Direction(Coordinate[] direction)
        {
            direction[0] = new Coordinate(0, 1); //right
            direction[1] = new Coordinate(0, -1); //left
            direction[2] = new Coordinate(1, 0); //down
            direction[3] = new Coordinate(-1, 0); //up
        }

        // method to set up a start menu for the game
        private static string configureStartMenu(List<string> options)
        {
            int i;

            for (i=0; i<options.Count; i++)
            {
                if(i == indicator)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(30, 11 + i);
                    Console.WriteLine(options[i]);
                }
                else
                {
                    Console.SetCursorPosition(30, 11 + i);
                    Console.WriteLine(options[i]);
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo consoleKey = Console.ReadKey();

            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    if(indicator <= 0)
                    {
                        // stay at the option if the indicator is at the topmost options 
                    }
                    else
                    {
                        indicator--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if(indicator == options.Count - 1)
                    {
                        // stay at the option if the indicator is at the bottommost options
                    }
                    else
                    {
                        indicator++;
                    }
                    break;
                case ConsoleKey.Enter:
                    return options[indicator]; // return the selected option
                default:
                    return ""; 
            }
            return "";
        }

        //method to set up a difficulty menu for the game
        private static string configureDifficultyMenu(List<string> options)
        {
            int i;

            for (i = 0; i < options.Count; i++)
            {
                if (i == indicator)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(30, 10 + i);
                    Console.WriteLine(options[i]);
                }
                else
                {
                    Console.SetCursorPosition(30, 10 + i);
                    Console.WriteLine(options[i]);
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo consoleKey = Console.ReadKey();

            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    if (indicator <= 0)
                    {
                        // stay at the option if the indicator is at the topmost options 
                    }
                    else
                    {
                        indicator--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (indicator == options.Count - 1)
                    {
                        // stay at the option if the indicator is at the bottommost options 
                    }
                    else
                    {
                        indicator++;
                    }
                    break;
                case ConsoleKey.Enter:
                    return options[indicator]; // return the selected option
                default:
                    return "";
            }
            return "";
        }

        static void Main(string[] args)
        {

            // display this char on the console during the game
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed
            Console.SetWindowSize(85, 25); // Set a fix size for the program when debug
            Console.SetBufferSize(85, 25); // Use to remove the scroll bar
            // location info & display
            int x = 0, y = 2; // y is 2 to allow the top row for directions & space
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            byte right = 0, left = 1, down = 2, up = 3;
            int direc = right;
            string optionSelected, difficultySelected;
            Program program = new Program();
            Coordinate[] direction = new Coordinate[4];
            program.Direction(direction);

            // start menu logo and description
            Console.SetCursorPosition(28, 5);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("========================");
            Console.SetCursorPosition(28, 6);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("||     SNAKE GAME     ||");
            Console.SetCursorPosition(28, 7);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("========================");
            Console.SetCursorPosition(28, 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to the Snake Game!");
            Console.SetCursorPosition(19, 9);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please select one of the option below and press Enter.");

            // create start menu options
            List<string> menuOptions = new List<string>()
            {
                "Play Game",
                "View Scoreboard",
                "Exit"
            };

            do
            {
                //Obtain the music file from the resource folder
                WindowsMediaPlayer menuMusic = new WindowsMediaPlayer();
                menuMusic.URL = @"C:\Users\Asus\SnakeGame\SnakeGame\SnakeGame\bin\Debug\Game-Menu.mp3";
                menuMusic.controls.play(); // Plays the music on the menu

                //Obtain the music file from the resource folder
                WindowsMediaPlayer hitObstacle = new WindowsMediaPlayer();
                hitObstacle.URL = @"C:\Users\Asus\SnakeGame\SnakeGame\SnakeGame\bin\Debug\teleport.wav";
                hitObstacle.controls.stop();

                //Obtain the music file from the resource folder
                WindowsMediaPlayer eatFood = new WindowsMediaPlayer();
                eatFood.URL = @"C:\Users\Asus\SnakeGame\SnakeGame\SnakeGame\bin\Debug\click.wav";
                eatFood.controls.stop();


                optionSelected = configureStartMenu(menuOptions);
                switch (optionSelected)
                {
                    //Play Game option
                    case "Play Game":
                            Console.Clear();
                            // start game
                            Console.SetCursorPosition(28, 5);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("========================");
                            Console.SetCursorPosition(28, 6);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("||   THE SNAKE GAME   ||");
                            Console.SetCursorPosition(28, 7);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("========================");
                            Console.SetCursorPosition(27, 8);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Please choose a difficulty!");

                            // create difficulty options
                            List<string> difficultyOptions = new List<string>()
                            {
                                "Easy",
                                "Hard"
                            };

                            do
                            {
                            
                            difficultySelected = configureDifficultyMenu(difficultyOptions);
                                switch (difficultySelected)
                                {

                                    //EASY MODE
                                    //Snake speed = 70
                                    //Number of obstacles = 4
                                    //Food timer = 100
                                    //Score to win = 10
                                    case "Easy":
                                    // initialise and generate the snake body
                                    // set the snake to start moving from the top left corner by default
                              
                                    menuMusic.controls.stop(); // stops the menu music when entered a chosen mode
                                    Queue<Coordinate> theSnek = new Queue<Coordinate>();
                                            int i;
                                            for (i = 0; i <= 3; i++)
                                            {
                                                theSnek.Enqueue(new Coordinate(2, i));
                                            }
                                            foreach (Coordinate coordinate in theSnek)
                                            {
                                                Console.SetCursorPosition(coordinate.column, coordinate.row);
                                                Console.ForegroundColor = ConsoleColor.Gray;
                                                Console.Write("*");
                                            }

                                            // clear to color
                                            Console.BackgroundColor = ConsoleColor.Black;
                                            Console.Clear();

                                            // delay to slow down the character movement so you can see it
                                            int delayInMillisecs = 70;

                                            // whether to keep trails
                                            bool trail = false;

                                            //generate a food when the game start
                                            int foodX;
                                            int foodY;
                                            Random randomNum = new Random();
                                            char foodS = '$';
                                            foodX = randomNum.Next(1, consoleWidthLimit);
                                            foodY = randomNum.Next(2, consoleHeightLimit);
                                            Console.SetCursorPosition(foodX, foodY);
                                            Console.Write(foodS);

                                            //Generate random obstacle when the game starts
                                            int obstacles1X, obstacles1Y, obstacles2X, obstacles2Y, obstacles3X, obstacles3Y, obstacles4X, obstacles4Y = 0;
                                            string obs = "█";
                                            obstacles1X = randomNum.Next(1, consoleWidthLimit);
                                            obstacles1Y = randomNum.Next(3, consoleHeightLimit);

                                            obstacles2X = randomNum.Next(1, consoleWidthLimit);
                                            obstacles2Y = randomNum.Next(3, consoleHeightLimit);


                                            obstacles3X = randomNum.Next(1, consoleWidthLimit);
                                            obstacles3Y = randomNum.Next(3, consoleHeightLimit);

                                            obstacles4X = randomNum.Next(1, consoleWidthLimit);
                                            obstacles4Y = randomNum.Next(3, consoleHeightLimit);

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

                                            //score variable
                                            int gameScore = 0;

                                            string snakeL = " ";

                                            do // until escape
                                            {
                                        // print directions at top, then restore position
                                        // save then restore current color
                                                ConsoleColor cc = Console.ForegroundColor;
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.SetCursorPosition(0, 0);
                                                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit. Reach 10 points to win the game.");
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
                                                            if (direc != down)
                                                            {
                                                                direc = up;
                                                            }
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            break;
                                                        case ConsoleKey.DownArrow: // DOWN
                                                            if (direc != up)
                                                            {
                                                                direc = down;
                                                            }
                                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                                            break;
                                                        case ConsoleKey.LeftArrow: //LEFT
                                                            if (direc != right)
                                                            {
                                                                direc = left;
                                                            }
                                                            Console.SetCursorPosition(x, y);
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            break;
                                                        case ConsoleKey.RightArrow: //RIGHT
                                                            if (direc != left)
                                                            {
                                                                direc = right;
                                                            }
                                                            Console.SetCursorPosition(x, y);
                                                            Console.ForegroundColor = ConsoleColor.Yellow;
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

                                                // set a timer to change the position of the food after a time interval
                                                if (timer == 100)
                                                {
                                                    Console.SetCursorPosition(foodX, foodY);
                                                    if (five == true)
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
                                                if (snakeHN.column == foodX && snakeHN.row == foodY)
                                                {
                                                    eatFood.controls.play(); // play this sound effect when a food was ate
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
                                                if (gameScore == 10)
                                                {
                                                    Console.Clear();
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("GAME CLEAR!!\n                                       YOU WIN!!!\n                                    PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                //The game ends when the snake hits the obstacles
                                                if (snakeHN.column == obstacles1X && snakeHN.row == obstacles1Y)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);                                                
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles2X && snakeHN.row == obstacles2Y)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();                                               
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);                                                        
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles3X && snakeHN.row == obstacles3Y)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();                                                    
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles4X && snakeHN.row == obstacles4Y)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();                                                 
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                            } while (gameLive);
                                        break;

                                    //HARD MODE
                                    //Snake speed = 35
                                    //Number of obstacles = 8
                                    //Food timer = 70
                                    //Score to win = 15
                                    case "Hard":
                                    menuMusic.controls.stop();
                                    // initialise and generate the snake body
                                    // set the snake to start moving from the top left corner by default
                                    Queue<Coordinate> theSnek2 = new Queue<Coordinate>();
                                            int j;
                                            for (j = 0; j <= 3; j++)
                                            {
                                                theSnek2.Enqueue(new Coordinate(2, j));
                                            }
                                            foreach (Coordinate coordinate in theSnek2)
                                            {
                                                Console.SetCursorPosition(coordinate.column, coordinate.row);
                                                Console.ForegroundColor = ConsoleColor.Gray;
                                                Console.Write("*");
                                            }

                                            // clear to color
                                            Console.BackgroundColor = ConsoleColor.Black;
                                            Console.Clear();

                                            // delay to slow down the character movement so you can see it
                                            int delayInMS = 35;

                                            // whether to keep trails
                                            bool trail2 = false;

                                            //generate a food when the game start
                                            Random randomNum2 = new Random();
                                            char foodS2 = '$';
                                            foodX = randomNum2.Next(1, consoleWidthLimit);
                                            foodY = randomNum2.Next(2, consoleHeightLimit);
                                            Console.SetCursorPosition(foodX, foodY);
                                            Console.Write(foodS2);

                                            //Generate random obstacle when the game starts
                                            int obstacles1X2, obstacles1Y2, obstacles2X2, obstacles2Y2, obstacles3X2, obstacles3Y2, obstacles4X2, obstacles4Y2;
                                            string obs2 = "█";

                                            obstacles1X2 = randomNum2.Next(1, consoleWidthLimit);
                                            obstacles1Y2 = randomNum2.Next(3, consoleHeightLimit);

                                            obstacles2X2 = randomNum2.Next(1, consoleWidthLimit);
                                            obstacles2Y2 = randomNum2.Next(3, consoleHeightLimit);


                                            obstacles3X2 = randomNum2.Next(1, consoleWidthLimit);
                                            obstacles3Y2 = randomNum2.Next(3, consoleHeightLimit);

                                            obstacles4X2 = randomNum2.Next(1, consoleWidthLimit);
                                            obstacles4Y2 = randomNum2.Next(3, consoleHeightLimit);

                                            Console.SetCursorPosition(obstacles1X2, obstacles1Y2);
                                            Console.Write(obs2);

                                            Console.SetCursorPosition(obstacles2X2, obstacles2Y2);
                                            Console.Write(obs2);

                                            Console.SetCursorPosition(obstacles3X2, obstacles3Y2);
                                            Console.Write(obs2);

                                            Console.SetCursorPosition(obstacles4X2, obstacles4Y2);
                                            Console.Write(obs2);

                                            //extra obstacles for hard mode
                                            int extraObsx, extraObsy;
                                            List<int> extraObsX = new List<int>();
                                            List<int> extraObsY = new List<int>();
                                            int k;

                                            for(k=0; k<4; k++)
                                            {
                                                extraObsx = randomNum2.Next(1, consoleWidthLimit);
                                                extraObsy = randomNum2.Next(3, consoleHeightLimit);
                                                extraObsX.Add(extraObsx);
                                                extraObsY.Add(extraObsy);
                                                Console.SetCursorPosition(extraObsx, extraObsy);
                                                Console.Write(obs2);
                                            }

                                            //time span variables
                                            int timer2 = 0;
                                            bool five2 = true;

                                            //score variable
                                            int gameScore2 = 0;

                                            string snakeL2 = " ";

                                            do // until escape
                                            {
                                                // print directions at top, then restore position
                                                // save then restore current color
                                                ConsoleColor cc = Console.ForegroundColor;
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.SetCursorPosition(0, 0);
                                                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit. Reach 15 points to win the game.");
                                                Console.WriteLine("Your score: " + gameScore2);
                                                Console.ForegroundColor = cc;
                                                timer2++;

                                                // see if a key has been pressed
                                                if (Console.KeyAvailable)
                                                {
                                                    // get key and use it to set options
                                                    consoleKey = Console.ReadKey(true);
                                                    switch (consoleKey.Key)
                                                    {

                                                        case ConsoleKey.UpArrow: //UP
                                                            if (direc != down)
                                                            {
                                                                direc = up;
                                                            }
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            break;
                                                        case ConsoleKey.DownArrow: // DOWN
                                                            if (direc != up)
                                                            {
                                                                direc = down;
                                                            }
                                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                                            break;
                                                        case ConsoleKey.LeftArrow: //LEFT
                                                            if (direc != right)
                                                            {
                                                                direc = left;
                                                            }
                                                            Console.SetCursorPosition(x, y);
                                                            Console.ForegroundColor = ConsoleColor.Green;
                                                            break;
                                                        case ConsoleKey.RightArrow: //RIGHT
                                                            if (direc != left)
                                                            {
                                                                direc = right;
                                                            }
                                                            Console.SetCursorPosition(x, y);
                                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                                            break;
                                                        case ConsoleKey.Escape: //END
                                                            gameLive = false;
                                                            break;
                                                    }
                                                }

                                                // initialise and indicate the head of the snake
                                                Coordinate snakeH = theSnek2.Last();
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
                                                System.Threading.Thread.Sleep(delayInMS);

                                                Console.SetCursorPosition(snakeH.column, snakeH.row);
                                                Console.Write("*");
                                                theSnek2.Enqueue(snakeHN);

                                                // set a timer to change the position of the food after a time interval
                                                if (timer2 == 70)
                                                {
                                                    Console.SetCursorPosition(foodX, foodY);
                                                    if (five2 == true)
                                                    {
                                                        Console.Write(snakeL2);
                                                    }
                                                    foodX = randomNum2.Next(1, consoleWidthLimit);
                                                    foodY = randomNum2.Next(2, consoleHeightLimit);
                                                    for (int l = 0; l < 4; l++)
                                                    {
                                                        do
                                                        {
                                                            if (obstacles1X2 != foodX && obstacles1Y2 != foodY ||
                                                                obstacles2X2 != foodX && obstacles2Y2 != foodY ||
                                                                obstacles3X2 != foodX && obstacles3Y2 != foodY ||
                                                                obstacles4X2 != foodX && obstacles4Y2 != foodY ||
                                                                extraObsX[l] != foodX && extraObsY[l] != foodY)
                                                            {
                                                                Console.SetCursorPosition(foodX, foodY);
                                                            }
                                                            else
                                                            {
                                                                foodX = randomNum2.Next(1, consoleWidthLimit);
                                                                foodY = randomNum2.Next(2, consoleHeightLimit);
                                                                Console.SetCursorPosition(foodX, foodY);
                                                            }
                                                        } while (obstacles1X2 == foodX && obstacles1Y2 == foodY ||
                                                        obstacles2X2 == foodX && obstacles2Y2 == foodY ||
                                                        obstacles3X2 == foodX && obstacles3Y2 == foodY ||
                                                        obstacles4X2 == foodX && obstacles4Y2 == foodY ||
                                                        extraObsX[l] == foodX && extraObsY[l] == foodY);
                                                    }
                                                    Console.Write(foodS2);
                                                    timer2 = 0;
                                                }

                                                //Increase score when the player ate a food
                                                if (snakeHN.column == foodX && snakeHN.row == foodY)
                                                {
                                                    eatFood.controls.play(); // play this sound effect when a food was ate
                                                    gameScore2++;
                                                    foodX = randomNum2.Next(1, consoleWidthLimit);
                                                    foodY = randomNum2.Next(2, consoleHeightLimit);
                                                    for (int l = 0; l < 4; l++)
                                                    {
                                                        do
                                                        {
                                                            if (obstacles1X2 != foodX && obstacles1Y2 != foodY ||
                                                                obstacles2X2 != foodX && obstacles2Y2 != foodY ||
                                                                obstacles3X2 != foodX && obstacles3Y2 != foodY ||
                                                                obstacles4X2 != foodX && obstacles4Y2 != foodY ||
                                                                extraObsX[l] != foodX && extraObsY[l] != foodY)
                                                            {
                                                                Console.SetCursorPosition(foodX, foodY);
                                                            }
                                                            else
                                                            {
                                                                foodX = randomNum2.Next(1, consoleWidthLimit);
                                                                foodY = randomNum2.Next(2, consoleHeightLimit);
                                                                Console.SetCursorPosition(foodX, foodY);
                                                            }
                                                        } while (obstacles1X2 == foodX && obstacles1Y2 == foodY ||
                                                        obstacles2X2 == foodX && obstacles2Y2 == foodY ||
                                                        obstacles3X2 == foodX && obstacles3Y2 == foodY ||
                                                        obstacles4X2 == foodX && obstacles4Y2 == foodY ||
                                                        extraObsX[l] == foodX && extraObsY[l] == foodY);
                                                    }
                                                    Console.Write(foodS2);
                                                    timer2 = 0;
                                                }
                                                else
                                                {
                                                    // find the coordinate and erase the trail behind the snake body
                                                    Coordinate trailL = theSnek2.Dequeue();
                                                    Console.SetCursorPosition(trailL.column, trailL.row);
                                                    Console.Write(snakeL2);
                                                }

                                                //Winning requirement: Snake eats 10 food
                                                if (gameScore2 == 15)
                                                {
                                                    Console.Clear();
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.WriteLine("GAME CLEAR!!\n                                       YOU WIN!!!\n                                    PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                //The game ends when the snake hits the obstacles
                                                if (snakeHN.column == obstacles1X2 && snakeHN.row == obstacles1Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);                                                  
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }                                                   
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles2X2 && snakeHN.row == obstacles2Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);                                                    
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }                                                   
                                                    return;
                                                }
                                                
                                                if (snakeHN.column == obstacles3X2 && snakeHN.row == obstacles3Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);                                                   
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }                                                   
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles4X2 && snakeHN.row == obstacles4Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();
                                                    Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);                                                   
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }                                                
                                                    return;
                                                }

                                                for (int l = 0; l < 4; l++)
                                                {
                                                    if (snakeHN.column == extraObsX[l] && snakeHN.row == extraObsY[l])
                                                    {
                                                        hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                        Console.Clear();
                                                        Console.SetCursorPosition(Console.WindowWidth / 3 + 10, Console.WindowHeight / 3 + 2);                                                       
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("GAME OVER!!\n                                 PRESS ENTER TO EXIT");
                                                        while (Console.ReadKey().Key != ConsoleKey.Enter) { }                                                    
                                                        return;
                                                    }
                                                }
                                            } while (gameLive);
                                        break;
                                }
                            } while (true);

                    //View Scoreboard option
                    case "View Scoreboard":
                            //DISPLAY SCOREBOARD HERE
                        break;

                    //Exit option
                    case "Exit":
                            System.Environment.Exit(0);
                        break;
                }
            } while (true);
        }
    }
}
