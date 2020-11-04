using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
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
                    Console.SetCursorPosition(35, 11 + i);
                    Console.WriteLine(options[i]);
                }
                else
                {
                    Console.SetCursorPosition(35, 11 + i);
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
                    Console.SetCursorPosition(37, 10 + i);
                    Console.WriteLine(options[i]);
                }
                else
                {
                    Console.SetCursorPosition(37, 10 + i);
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
            var path = "C:/Users/dunca/SnakeGame/SnakeGame/SnakeGame/bin/Debug/netcoreapp3.1/score.txt";

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
            Console.SetCursorPosition(27, 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to the Snake Game!");
            Console.SetCursorPosition(15, 9);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please select one of the option below and press Enter.");

            //Obtain the music file from the resource folder
            WindowsMediaPlayer menuMusic = new WindowsMediaPlayer();
            string mmName = "Game-Menu.mp3";
            menuMusic.URL = AppDomain.CurrentDomain.BaseDirectory + mmName;

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
                WindowsMediaPlayer hitObstacle = new WindowsMediaPlayer();
                string hoName = "teleport.wav";
                hitObstacle.URL = AppDomain.CurrentDomain.BaseDirectory + hoName;            
                hitObstacle.controls.stop();

                //Obtain the music file from the resource folder
                WindowsMediaPlayer eatFood = new WindowsMediaPlayer();
                string efName = "click.wav";
                eatFood.URL = AppDomain.CurrentDomain.BaseDirectory + efName;                
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
                                    
                                    menuMusic.controls.stop(); // stops the menu music when entered a chosen mode
                                                               
                                    // initialise and generate the snake body
                                    // set the snake to start moving from the top left corner by default
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

                                            //generate the special food when the game start
                                            int spfoodX;
                                            int spfoodY;
                                            Random randomNumSP = new Random();
                                            char spfoodS = '&';
                                            spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                            spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                            Console.SetCursorPosition(spfoodX, spfoodY);
                                            Console.Write(spfoodS);

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
                                            int timerSP = 0;
                                            bool five = true;

                                            //score variable
                                            int gameScore = 0;

                                            string snakeL = " ";

                                            StreamWriter sw = File.AppendText(path);

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
                                                timerSP++;

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

                                                // set a timer to change the position of the special food after a time interval 
                                                if (timerSP == 150)
                                                {
                                                    Console.SetCursorPosition(spfoodX, spfoodY);
                                                    if (five == true)
                                                    {
                                                        Console.Write(snakeL);
                                                    }
                                                    spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                                    spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                                    do
                                                    {
                                                        if (obstacles1X != spfoodX && obstacles1Y != spfoodY ||
                                                            obstacles2X != spfoodX && obstacles2Y != spfoodY ||
                                                            obstacles3X != spfoodX && obstacles3Y != spfoodY ||
                                                            obstacles4X != spfoodX && obstacles4Y != spfoodY)
                                                        {
                                                            Console.SetCursorPosition(spfoodX, spfoodY);
                                                        }
                                                        else
                                                        {
                                                            spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                                            spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                                            Console.SetCursorPosition(spfoodX, spfoodY);
                                                        }
                                                    } while (obstacles1X == spfoodX && obstacles1Y == spfoodY ||
                                                    obstacles2X == spfoodX && obstacles2Y == spfoodY ||
                                                    obstacles3X == spfoodX && obstacles3Y == spfoodY ||
                                                    obstacles4X == spfoodX && obstacles4Y == spfoodY);
                                                    Console.Write(spfoodS);
                                                    timerSP = 0;
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

                                                //Increase score when the player ate a special food
                                                if (snakeHN.column == spfoodX && snakeHN.row == spfoodY)
                                                {
                                                    eatFood.controls.play(); // play this sound effect when a food was ate
                                                    gameScore+=2;
                                                    do
                                                    {
                                                        if (obstacles1X != spfoodX && obstacles1Y != spfoodY ||
                                                            obstacles2X != spfoodX && obstacles2Y != spfoodY ||
                                                            obstacles3X != spfoodX && obstacles3Y != spfoodY ||
                                                            obstacles4X != spfoodX && obstacles4Y != spfoodY)
                                                        {
                                                            Console.SetCursorPosition(spfoodX, spfoodY);
                                                        }
                                                        else
                                                        {
                                                            spfoodX = randomNum.Next(1, consoleWidthLimit);
                                                            spfoodY = randomNum.Next(2, consoleHeightLimit);
                                                            Console.SetCursorPosition(spfoodX, spfoodY);
                                                        }
                                                    } while (obstacles1X == spfoodX && obstacles1Y == spfoodY ||
                                                    obstacles2X == spfoodX && obstacles2Y == spfoodY ||
                                                    obstacles3X == spfoodX && obstacles3Y == spfoodY ||
                                                    obstacles4X == spfoodX && obstacles4Y == spfoodY);
                                                    Console.Write(spfoodS);
                                                    timerSP = 0;
                                                } 
                                             					

                                                //Winning requirement: Snake eats 10 food
                                                if (gameScore >= 10)
                                                {
                                                    Console.Clear();
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME CLEAR!!");
                                                    Console.SetCursorPosition(39,10);
                                                    Console.WriteLine("YOU WIN!!");
                                                    Console.SetCursorPosition(37,11);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore);
                                                    Console.SetCursorPosition(33,12);
                                                    //User input to save name
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 13);
                                                    string username = Console.ReadLine();
                                                    //Saves name and score into text file
                                                    sw.WriteLine(username + "\t" + "\t" + "\t" + "\t" + gameScore.ToString());
                                                    sw.Close();
                                                    Console.SetCursorPosition(34,14);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                //The game ends when the snake hits the obstacles
                                                if (snakeHN.column == obstacles1X && snakeHN.row == obstacles1Y)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username = Console.ReadLine();
                                                    sw.WriteLine(username + "\t" + "\t" + "\t" + "\t" + gameScore.ToString());
                                                    sw.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles2X && snakeHN.row == obstacles2Y)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username = Console.ReadLine();
                                                    sw.WriteLine(username + "\t" + "\t" + "\t" + "\t" + gameScore.ToString());
                                                    sw.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles3X && snakeHN.row == obstacles3Y)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username = Console.ReadLine();
                                                    sw.WriteLine(username + "\t" + "\t" + "\t" + "\t" + gameScore.ToString());
                                                    sw.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles4X && snakeHN.row == obstacles4Y)
                                                {
                                                   hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username = Console.ReadLine();
                                                    sw.WriteLine(username + "\t" + "\t" + "\t" + "\t" + gameScore.ToString());
                                                    sw.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
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
                                            int foodX2;
                                            int foodY2;
                                            Random randomNum2 = new Random();
                                            char foodS2 = '$';
                                            foodX2 = randomNum2.Next(1, consoleWidthLimit);
                                            foodY2 = randomNum2.Next(2, consoleHeightLimit);
                                            Console.SetCursorPosition(foodX2, foodY2);
                                            Console.Write(foodS2);

                                            //generate the special food when the game start
                                            int spfoodX2;
                                            int spfoodY2;
                                            Random randomNumSP2 = new Random();
                                            char spfoodS2 = '&';
                                            spfoodX2 = randomNumSP2.Next(1, consoleWidthLimit);
                                            spfoodY2 = randomNumSP2.Next(2, consoleHeightLimit);
                                            Console.SetCursorPosition(spfoodX2, spfoodY2);
                                            Console.Write(spfoodS2);

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
                                            int timerSP2 =0;
                                            bool five2 = true;

                                            //score variable
                                            int gameScore2 = 0;

                                            string snakeL2 = " ";
                                            
                                            StreamWriter sw2 = File.AppendText(path);

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
                                                timerSP2++;

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
                                                    Console.SetCursorPosition(foodX2, foodY2);
                                                    if (five2 == true)
                                                    {
                                                        Console.Write(snakeL2);
                                                    }
                                                    foodX2 = randomNum2.Next(1, consoleWidthLimit);
                                                    foodY2 = randomNum2.Next(2, consoleHeightLimit);
                                                    for (int l = 0; l < 4; l++)
                                                    {
                                                        do
                                                        {
                                                            if (obstacles1X2 != foodX2 && obstacles1Y2 != foodY2 ||
                                                                obstacles2X2 != foodX2 && obstacles2Y2 != foodY2 ||
                                                                obstacles3X2 != foodX2 && obstacles3Y2 != foodY2 ||
                                                                obstacles4X2 != foodX2 && obstacles4Y2 != foodY2 ||
                                                                extraObsX[l] != foodX2 && extraObsY[l] != foodY2)
                                                            {
                                                                Console.SetCursorPosition(foodX2, foodY2);
                                                            }
                                                            else
                                                            {
                                                                foodX2 = randomNum2.Next(1, consoleWidthLimit);
                                                                foodY2 = randomNum2.Next(2, consoleHeightLimit);
                                                                Console.SetCursorPosition(foodX2, foodY2);
                                                            }
                                                        } while (obstacles1X2 == foodX2 && obstacles1Y2 == foodY2 ||
                                                        obstacles2X2 == foodX2 && obstacles2Y2 == foodY2 ||
                                                        obstacles3X2 == foodX2 && obstacles3Y2 == foodY2 ||
                                                        obstacles4X2 == foodX2 && obstacles4Y2 == foodY2 ||
                                                        extraObsX[l] == foodX2 && extraObsY[l] == foodY2);
                                                    }
                                                    Console.Write(foodS2);
                                                    timer2 = 0;
                                                }

                                                 // set a timer to change the position of the special food after a time interval 
                                                if (timerSP2 == 120)
                                                {
                                                    Console.SetCursorPosition(spfoodX2, spfoodY2);
                                                    if (five2 == true)
                                                    {
                                                        Console.Write(snakeL2);
                                                    }
                                                    spfoodX2 = randomNumSP2.Next(1, consoleWidthLimit);
                                                    spfoodY2 = randomNumSP2.Next(2, consoleHeightLimit);
                                                    for (int l = 0; l < 4; l++)
                                                    { 
                                                        do
                                                        {
                                                            if (obstacles1X2 != spfoodX2 && obstacles1Y2 != spfoodY2 ||
                                                                obstacles2X2 != spfoodX2 && obstacles2Y2 != spfoodY2 ||
                                                                obstacles3X2 != spfoodX2 && obstacles3Y2 != spfoodY2 ||
                                                                obstacles4X2 != spfoodX2 && obstacles4Y2 != spfoodY2 ||
                                                                extraObsX[1] != spfoodX2 && extraObsY[1] != spfoodY2)
                                                            {
                                                                Console.SetCursorPosition(spfoodX2, spfoodY2);
                                                            }
                                                            else
                                                            {
                                                                spfoodX2 = randomNumSP2.Next(1, consoleWidthLimit);
                                                                spfoodY2 = randomNumSP2.Next(2, consoleHeightLimit);
                                                                Console.SetCursorPosition(spfoodX2, spfoodY2);
                                                            }
                                                        } while (obstacles1X2 == foodX2 && obstacles1Y2 == spfoodY2 ||
                                                        obstacles2X2 == spfoodX2 && obstacles2Y2 == spfoodY2 ||
                                                        obstacles3X2 == spfoodX2 && obstacles3Y2 == spfoodY2 ||
                                                        obstacles4X2 == spfoodX2 && obstacles4Y2 == spfoodY2 ||
                                                        extraObsX[1] == spfoodX2 && extraObsY[1] == spfoodY2);
                                                    }
                                                    Console.Write(spfoodS2);
                                                    timerSP2 = 0;
                                                }

                                                //Increase score when the player ate a food
                                                if (snakeHN.column == foodX2 && snakeHN.row == foodY2)
                                                {
                                                    eatFood.controls.play(); // play this sound effect when a food was ate
                                                    gameScore2++;
                                                    foodX2 = randomNum2.Next(1, consoleWidthLimit);
                                                    foodY2 = randomNum2.Next(2, consoleHeightLimit);
                                                    for (int l = 0; l < 4; l++)
                                                    {
                                                        do
                                                        {
                                                            if (obstacles1X2 != foodX2 && obstacles1Y2 != foodY2 ||
                                                                obstacles2X2 != foodX2 && obstacles2Y2 != foodY2 ||
                                                                obstacles3X2 != foodX2 && obstacles3Y2 != foodY2 ||
                                                                obstacles4X2 != foodX2 && obstacles4Y2 != foodY2 ||
                                                                extraObsX[l] != foodX2 && extraObsY[l] != foodY2)
                                                            {
                                                                Console.SetCursorPosition(foodX2, foodY2);
                                                            }
                                                            else
                                                            {
                                                                foodX2 = randomNum2.Next(1, consoleWidthLimit);
                                                                foodY2 = randomNum2.Next(2, consoleHeightLimit);
                                                                Console.SetCursorPosition(foodX2, foodY2);
                                                            }
                                                        } while (obstacles1X2 == foodX2 && obstacles1Y2 == foodY2 ||
                                                        obstacles2X2 == foodX2 && obstacles2Y2 == foodY2 ||
                                                        obstacles3X2 == foodX2 && obstacles3Y2 == foodY2 ||
                                                        obstacles4X2 == foodX2 && obstacles4Y2 == foodY2 ||
                                                        extraObsX[l] == foodX2 && extraObsY[l] == foodY2);
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

                                                 //Increase score when the player ate a special food
                                                if (snakeHN.column == spfoodX2 && snakeHN.row == spfoodY2)
                                                {
                                                    eatFood.controls.play(); // play this sound effect when a food was ate
                                                    gameScore2+=2;
                                                    for (int l = 0; l < 4; l++)
                                                    { 
                                                        do
                                                        {
                                                            if (obstacles1X2 != spfoodX2 && obstacles1Y2 != spfoodY2 ||
                                                                obstacles2X2 != spfoodX2 && obstacles2Y2 != spfoodY2 ||
                                                                obstacles3X2 != spfoodX2 && obstacles3Y2 != spfoodY2 ||
                                                                obstacles4X2 != spfoodX2 && obstacles4Y2 != spfoodY2 ||
                                                                extraObsX[1] != spfoodX2 && extraObsY[1] != spfoodY2)
                                                            {
                                                                Console.SetCursorPosition(spfoodX2, spfoodY2);
                                                            }
                                                            else
                                                            {
                                                                spfoodX2 = randomNumSP2.Next(1, consoleWidthLimit);
                                                                spfoodY2 = randomNumSP2.Next(2, consoleHeightLimit);
                                                                Console.SetCursorPosition(spfoodX2, spfoodY2);
                                                            }
                                                        } while (obstacles1X2 == foodX2 && obstacles1Y2 == spfoodY2 ||
                                                        obstacles2X2 == spfoodX2 && obstacles2Y2 == spfoodY2 ||
                                                        obstacles3X2 == spfoodX2 && obstacles3Y2 == spfoodY2 ||
                                                        obstacles4X2 == spfoodX2 && obstacles4Y2 == spfoodY2 ||
                                                        extraObsX[1] == spfoodX2 && extraObsY[1] == spfoodY2);
                                                    }
                                                    Console.Write(spfoodS2);
                                                    timerSP2 = 0;
                                                } 

                                                //Winning requirement: Snake eats 15 food
                                                if (gameScore2 >= 15)
                                                {
                                                   Console.Clear();
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME CLEAR!!");
                                                    Console.SetCursorPosition(39,10);
                                                    Console.WriteLine("YOU WIN!!");
                                                    Console.SetCursorPosition(37,11);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore2);
                                                    Console.SetCursorPosition(33,12);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 13);
                                                    string username2 = Console.ReadLine();
                                                    sw2.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore2.ToString());
                                                    sw2.Close();
                                                    Console.SetCursorPosition(34,14);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                //The game ends when the snake hits the obstacles
                                                if (snakeHN.column == obstacles1X2 && snakeHN.row == obstacles1Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore2);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username2 = Console.ReadLine();
                                                    sw2.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore2.ToString());
                                                    sw2.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles2X2 && snakeHN.row == obstacles2Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore2);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username2 = Console.ReadLine();
                                                    sw2.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore2.ToString());
                                                    sw2.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }
                                                
                                                if (snakeHN.column == obstacles3X2 && snakeHN.row == obstacles3Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore2);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username2 = Console.ReadLine();
                                                    sw2.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore2.ToString());
                                                    sw2.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                if (snakeHN.column == obstacles4X2 && snakeHN.row == obstacles4Y2)
                                                {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore2);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username2 = Console.ReadLine();
                                                    sw2.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore2.ToString());
                                                    sw2.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
                                                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                    return;
                                                }

                                                for (int l = 0; l < 4; l++)
                                                {
                                                    if (snakeHN.column == extraObsX[l] && snakeHN.row == extraObsY[l])
                                                    {
                                                    hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                    Console.Clear();         
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38,9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37,10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore2);
                                                    Console.SetCursorPosition(33,11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME: ");
                                                    Console.SetCursorPosition(40, 12);
                                                    string username2 = Console.ReadLine();
                                                    sw2.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore2.ToString());
                                                    sw2.Close();
                                                    Console.SetCursorPosition(34,13);
                                                    Console.WriteLine("PRESS ENTER TO EXIT");
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
                        Console.Clear();
                        Console.SetCursorPosition(28, 5);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("========================");
                        Console.SetCursorPosition(28, 6);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("||   THE SNAKE GAME   ||");
                        Console.SetCursorPosition(28, 7);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("========================");
                        Console.SetCursorPosition(32, 8);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Player Scoreboard");
                        using (StreamReader file = new StreamReader(path))
                        {
                            int i = 0;
                            string score;
                            while ((score = file.ReadLine()) != null)
                            {
                                //displays previous name and score of players
                                Console.SetCursorPosition(21, 9);
                                Console.WriteLine("Name" + "\t" + "\t" + "\t" + "\t" + "Score");
                                Console.SetCursorPosition(21, 10);
                                Console.WriteLine(score);
                            }
                        }
                        Console.SetCursorPosition(27, 12);
                        Console.WriteLine("Press ESC to exit scoreboard");
                        if (Console.ReadKey().Key == ConsoleKey.Escape)
                        {
                            Console.Clear();
                            Console.SetCursorPosition(28, 5);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("========================");
                            Console.SetCursorPosition(28, 6);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("||     SNAKE GAME     ||");
                            Console.SetCursorPosition(28, 7);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("========================");
                            Console.SetCursorPosition(27, 8);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Welcome to the Snake Game!");
                            Console.SetCursorPosition(15, 9);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Please select one of the option below and press Enter.");
                        }
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
