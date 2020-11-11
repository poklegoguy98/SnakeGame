using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Sources;
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
        private static string ConfigureStartMenu(List<string> options)
        {
            int i;

            for (i = 0; i < options.Count; i++)
            {
                if (i == indicator)
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
                    if (indicator > 0)
                    {
                        indicator--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (indicator != options.Count - 1)
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
        private static string ConfigureDifficultyMenu(List<string> options)
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
                    if (indicator > 0)
                    {
                        indicator--;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (indicator != options.Count - 1)
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

        static void Main()
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
            var path = AppDomain.CurrentDomain.BaseDirectory + "score.txt";        

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
            menuMusic.settings.setMode("Loop", true);

            WindowsMediaPlayer wonEffect = new WindowsMediaPlayer();
            string weName = "won.wav";
            wonEffect.URL = AppDomain.CurrentDomain.BaseDirectory + weName;          
            wonEffect.settings.volume = 3;
            wonEffect.controls.stop();

            //Obtain the music file from the resource folder
            WindowsMediaPlayer modeMusic = new WindowsMediaPlayer();
            string modeName = "snake.mp3";
            modeMusic.URL = AppDomain.CurrentDomain.BaseDirectory + modeName;
            modeMusic.settings.setMode("Loop", true);
            modeMusic.settings.volume = 3;
            modeMusic.controls.stop();

            //Obtain the music file from the resource folder
            WindowsMediaPlayer hitObstacle = new WindowsMediaPlayer();
            string hoName = "teleport.wav";
            hitObstacle.URL = AppDomain.CurrentDomain.BaseDirectory + hoName;
            hitObstacle.settings.volume = 2;
            hitObstacle.controls.stop();

            //Obtain the music file from the resource folder
            WindowsMediaPlayer eatFood = new WindowsMediaPlayer();
            string efName = "click.wav";
            eatFood.URL = AppDomain.CurrentDomain.BaseDirectory + efName;
            eatFood.controls.stop();

            // create start menu options
            List<string> menuOptions = new List<string>()
            {
                "Play Game",
                "View Scoreboard",
                "Exit"
            };

            do
            {
                optionSelected = ConfigureStartMenu(menuOptions);
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
                        int delayInMs;
                        int obstaclesNum;
                        int foodTimer;
                        int scoreToWin;
                        int spfoodTimer;

                        // create difficulty options
                        List<string> difficultyOptions = new List<string>()
                            {
                                "Easy",
                                "Hard"
                            };

                        do
                        {
                            difficultySelected = ConfigureDifficultyMenu(difficultyOptions);
                            if (difficultySelected == "Easy")
                            {
                                //EASY MODE
                                //SNAKE SPEED = 70
                                //NUMBER OF OBSTACLES = 4
                                //FOOD TIMER = 100
                                //SCORE TO WIN = 10
                                delayInMs = 70;
                                obstaclesNum = 4;
                                foodTimer = 100;
                                spfoodTimer = 30;
                                scoreToWin = 10;
                                break;
                            }
                            else if (difficultySelected == "Hard")
                            {
                                //HARD MODE
                                //SNAKE SPEED = 35
                                //NUMBER OF OBSTACLES = 8
                                //FOOD TIMER = 70
                                //SCORE TO WIN = 15
                                delayInMs = 35;
                                obstaclesNum = 8;
                                foodTimer = 70;
                                spfoodTimer = 20;
                                scoreToWin = 15;
                                break;
                            }
                        } while (true);

                        menuMusic.controls.stop();
                        modeMusic.controls.play();
                                    // game mode differ according to the difficulty selection
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

                                    //generate a food when the game start
                                    int foodX;
                                    int foodY;
                                    Random randomNum = new Random();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.OutputEncoding = Encoding.Unicode;
                                    foodX = randomNum.Next(1, consoleWidthLimit);
                                    foodY = randomNum.Next(2, consoleHeightLimit);
                                    Console.SetCursorPosition(foodX, foodY);
                                    Console.Write("ó");


                                    //generate the special food when the game start
                                    int spfoodX;
                                    int spfoodY;
                                    Random randomNumSP = new Random();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.OutputEncoding = Encoding.Unicode;
                                    spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                    spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                    Console.SetCursorPosition(spfoodX, spfoodY);
                                    Console.Write("♦");

                                    //generate obstacles for both modes
                                    int Obsx, Obsy;
                                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                    string obs = "█";
                                    List<int> ObsX = new List<int>();
                                    List<int> ObsY = new List<int>();
                                    int k;

                                    for (k = 0; k < obstaclesNum; k++)
                                    {
                                        Obsx = randomNum.Next(1, consoleWidthLimit);
                                        Obsy = randomNum.Next(3, consoleHeightLimit);
                                        ObsX.Add(Obsx);
                                        ObsY.Add(Obsy);
                                        Console.SetCursorPosition(Obsx, Obsy);
                                        Console.Write(obs);
                                    }

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
                                        Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit. Reach " + scoreToWin + " points to win the game.");
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
                                        int snakeNR, snakeNC;
                                        Coordinate snakeH = theSnek.Last();
                                        Coordinate direcN = direction[direc];
                                        snakeNR = snakeH.row + direcN.row;
                                        snakeNC = snakeH.column + direcN.column;
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
                                        System.Threading.Thread.Sleep(delayInMs);

                                        Console.SetCursorPosition(snakeH.column, snakeH.row);
                                        Console.Write("*");
                                        theSnek.Enqueue(snakeHN);

                                        // set a timer to change the position of the food after a time interval
                                        if (timer == foodTimer)
                                        {
                                            Console.SetCursorPosition(foodX, foodY);
                                            if (five == true)
                                            {
                                                Console.Write(snakeL);
                                            }
                                            foodX = randomNum.Next(1, consoleWidthLimit);
                                            foodY = randomNum.Next(2, consoleHeightLimit);
                                            for (int j = 0; j < obstaclesNum; j++)
                                            {
                                                do
                                                {
                                                    if (ObsX[j] != foodX && ObsY[j] != foodY)
                                                    {
                                                        Console.SetCursorPosition(foodX, foodY);
                                                    }
                                                    else
                                                    {
                                                        foodX = randomNum.Next(1, consoleWidthLimit);
                                                        foodY = randomNum.Next(2, consoleHeightLimit);
                                                        Console.SetCursorPosition(foodX, foodY);
                                                    }
                                                } while (ObsX[j] == foodX && ObsY[j] == foodY);
                                            }
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.OutputEncoding = Encoding.Unicode;
                                            Console.Write("ó");
                                            timer = 0;
                                        }

                                        // set a timer to change the position of the special food after a time interval 
                                        if (timerSP == spfoodTimer)
                                        {
                                            Console.SetCursorPosition(spfoodX, spfoodY);
                                            if (five == true)
                                            {
                                                Console.Write(snakeL);
                                            }
                                            spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                            spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                            for (int j = 0; j < obstaclesNum; j++)
                                            {
                                                do
                                                {
                                                    if (ObsX[j] != spfoodX && ObsY[j] != spfoodY)    
                                                    {
                                                        Console.SetCursorPosition(spfoodX, spfoodY);
                                                    }
                                                    else
                                                    {
                                                        spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                                        spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                                        Console.SetCursorPosition(spfoodX, spfoodY);
                                                    }
                                                } while (ObsX[j] == spfoodX && ObsY[j] == spfoodY);
                                            }
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.OutputEncoding = Encoding.Unicode;
                                            Console.Write("♦");
                                            timerSP = 0;
                                        }

                                        //Increase score when the player ate a food
                                        if (snakeHN.column == foodX && snakeHN.row == foodY)
                                        {
                                            eatFood.controls.play(); // play this sound effect when a food was ate
                                            gameScore++;
                                            foodX = randomNum.Next(1, consoleWidthLimit);
                                            foodY = randomNum.Next(2, consoleHeightLimit);
                                            for (int j = 0; j < obstaclesNum; j++)
                                            {
                                                do
                                                {
                                                    if (ObsX[j] != foodX && ObsY[j] != foodY)
                                                        
                                                    {
                                                        Console.SetCursorPosition(foodX, foodY);
                                                    }
                                                    else
                                                    {
                                                        foodX = randomNum.Next(1, consoleWidthLimit);
                                                        foodY = randomNum.Next(2, consoleHeightLimit);
                                                        Console.SetCursorPosition(foodX, foodY);
                                                    }
                                                } while (ObsX[j] == foodX && ObsY[j] == foodY);
                                            }
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.OutputEncoding = Encoding.Unicode;
                                            Console.Write("ó");
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
                                            gameScore += 2;
                                            spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                            spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                            for (int j = 0; j < obstaclesNum; j++)
                                            {
                                                do
                                                {
                                                    if (ObsX[j] != spfoodX && ObsY[j] != spfoodY)
                                                        
                                                    {
                                                        Console.SetCursorPosition(spfoodX, spfoodY);
                                                    }
                                                    else
                                                    {
                                                        spfoodX = randomNumSP.Next(1, consoleWidthLimit);
                                                        spfoodY = randomNumSP.Next(2, consoleHeightLimit);
                                                        Console.SetCursorPosition(spfoodX, spfoodY);
                                                    }
                                                } while (ObsX[j] == spfoodX && ObsY[j] == spfoodY);
                                            }
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.OutputEncoding = Encoding.Unicode;
                                            Console.Write("♦");
                                            timerSP = 0;
                                        }

                                        //Winning requirement: Player reach 10 points in easy mode or 15 points in hard mode
                                        if (gameScore >= scoreToWin)
                                        {
                                            modeMusic.controls.stop();
                                            wonEffect.controls.play();
                                            string username2;
                                            do
                                            {
                                                Console.Clear();
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.SetCursorPosition(38, 9);
                                                Console.WriteLine("GAME CLEAR!!");
                                                Console.SetCursorPosition(39, 10);
                                                Console.WriteLine("YOU WIN!!");
                                                Console.SetCursorPosition(37, 11);
                                                Console.WriteLine("YOUR SCORE: " + gameScore);
                                                Console.SetCursorPosition(23, 12);
                                                //User input to save name
                                                Console.WriteLine("PLEASE ENTER YOUR NAME (maximum 7 characters): ");
                                                Console.SetCursorPosition(40, 13);
                                                username2 = Console.ReadLine();
                                            } while (username2.Length > 7);
                                            sw.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore.ToString());
                                            sw.Close();
                                            Console.SetCursorPosition(34, 14);
                                            Console.WriteLine("PRESS ENTER TO EXIT");
                                            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                            return;
                                        }

                                        // the game ends when the player hits one of the obstacles
                                        for (int j = 0; j < obstaclesNum; j++)
                                        {
                                            if (snakeHN.column == ObsX[j] && snakeHN.row == ObsY[j])
                                            {
                                                modeMusic.controls.stop();
                                                hitObstacle.controls.play(); // Plays the hitObstacle sound effect when the snake hits the obstacle
                                                string username2;
                                                do
                                                {
                                                    Console.Clear();
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.SetCursorPosition(38, 9);
                                                    Console.WriteLine("GAME OVER!!");
                                                    Console.SetCursorPosition(37, 10);
                                                    Console.WriteLine("YOUR SCORE: " + gameScore);
                                                    Console.SetCursorPosition(23, 11);
                                                    Console.WriteLine("PLEASE ENTER YOUR NAME (maximum 7 characters): ");
                                                    Console.SetCursorPosition(40, 12);
                                                    username2 = Console.ReadLine();
                                                } while (username2.Length > 7);
                                                sw.WriteLine(username2 + "\t" + "\t" + "\t" + "\t" + gameScore.ToString());
                                                sw.Close();
                                                Console.SetCursorPosition(34, 13);
                                                Console.WriteLine("PRESS ENTER TO EXIT");
                                                while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                                                return;
                                            }
                                        }
                                    } while (gameLive);
                            break;

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
                        Console.SetCursorPosition(21, 9);
                        Console.WriteLine("\t" + "Name" + "\t" + "\t" + "\t" + "\t" + "Score");
                        using (StreamReader file = new StreamReader(path))
                        {
                            string score;
                            while ((score = file.ReadLine()) != null)
                            {
                                //displays previous name and score of players
                                Console.WriteLine("\t" + "\t" + "\t" + score);
                            }
                        }
                        Console.SetCursorPosition(27, 24);
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
