using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace CaveExplorers_FirstPlayable__WillB
{
    internal class Program
    {
        // int variables
        //Player
        static int playerPOSy = 10;
        static int playerPOSx = 10;
        static int playerDamage = 2;
        static int playerHealth = 10;
        //Mimic
        static int MimicPOSx = 5;
        static int MimicPOSy = 5;
        static int MaxMimicPOSx;
        static int MaxMimicPOSy;
        static int enemyDamage = 2;
        static int enemyHealth = 10;
        //Collectables
        static int Collectables;
        //Time
        static int milliseconds;
        static int startingStage = 1;
        //Turns
        static int TurnCount = 1;
        //Grunt
        static int GruntHealth = 5;
        static int GruntDamage = 5;
        static int GruntPOSy = 15;
        static int GruntPOSx = 15;
        //string variables
        //Player
        static string userName;
        static string tutorialCheck;
        //Arrays
        static string[] arrayString;
        static char[,] arrayChar;
        //Game
        static string gameStart;
        static string startCheck;
        static string path = @"Map.txt";
        static string Playeraction;
        static string Mimicaction;
        static string Gruntaction;
        static string turn;

        //bool variables
        static bool Playerturn = true;
        static bool MimicDeath = false;
        static bool GruntDeath = false;
        //Enemy
        static bool EnemyInWater = false;
        //Player
        static bool PlayerInWater = false;
        //Gets key input for player movement
        static ConsoleKeyInfo playerControl;
        //Random for Mimic Movement
        static Random rnd = new Random();


        static void Main()
        {
            Console.Write("+-------------------------+\n" +
                          "|Welcome to Cave explorers|\n" +
                          "|Made by Will Boudreau    |\n" +
                          "+-------------------------+\n");
            Console.WriteLine("Would you like to begin? Yes or No or Start to start");
            gameStart = Console.ReadLine();
            if (gameStart == "Yes" | gameStart == "yes")
            {
                Menu();
            }
            if (gameStart == "Skip" | gameStart == "skip")
            {
                stage(startingStage);
            }
            if(gameStart == "Start"| gameStart == "start")
            {
                stage(startingStage);
            }
            else
            {
                annoyPlayer();
            }
        }
        static void annoyPlayer()
        {
            //Method to annoy the player until they quit or play the game
            Console.WriteLine("Are you sure?");
            startCheck = Console.ReadLine();
            if (startCheck == "Yes" | startCheck == "yes")
            {
                annoyPlayer();
            }
            else
            {
                Console.WriteLine("Than lets begin finally!");
                Console.WriteLine();
                Menu();
            }
        }
        static void Menu()
        {
            //MAin menu of the game
            Console.WriteLine("Hello brave user! Please enter your name:");
            userName = Console.ReadLine();
            Console.WriteLine("Hello " + userName + "!");
            Console.WriteLine("Would you like to start off with a tutorial? Yes or No");
            tutorialCheck = Console.ReadLine();
            if (tutorialCheck == "yes" | tutorialCheck == "Yes")
            {
                tutorial();
            }
            else
            {
                Console.WriteLine("Alrighty than " + userName);
                Console.WriteLine("Lets begin");
                stage(startingStage);
            }
            Console.ReadKey();
        }
        static void ShowHUD()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Player Stats\n| Health " + playerHealth + "|" + "Attack " + playerDamage + "|" + "Gold " + Collectables +"\n"+"Enemmy Stats" + "\n"+ "|Mimic health " + enemyHealth +"|" +"Attack " + enemyDamage + "|" +"\n" +"|Grunt Health " + GruntHealth +"|Grunt Damage "+ GruntDamage +"|");
        }
        static void Legend()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Legend");
            Console.Write("|Player: &" + "|Enemy: #" + "|Gold: *" + "|Mountains: ^" + "|Spikes: +" + "|Water: ~|");
        }
        static void UpdateLog()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Turn " + TurnCount +"|"+ Playeraction +"\n" + Mimicaction + "\n" +Gruntaction);
            Console.WriteLine("Turn " + turn);
            Console.ResetColor();
            TurnCount++;
        }
        static void tutorial()
        {
            // Allows the Player to understand the game
            Console.Clear();
            Console.Write("Welcome to the tutorial! Here we will cover the basics to playing the game:" +
                          "\nFirst off is you the player");
            milliseconds = 2000;
            Thread.Sleep(milliseconds);
            Console.WriteLine("\nIn this world, you will face monsters of unimaginable horrors!" +
                              "\nMonsters that want to eat you alive!");
            Thread.Sleep(milliseconds);
            Console.WriteLine("You are the main character of this adventure" +
                              "\n if you die and your lives reach 0" +
                              "\n The journeys over, you died.");
            Thread.Sleep(milliseconds);
            Console.WriteLine("In this game you will use the WASD keys to move" +
                              "\nW-To move up" +
                              "\nA-To move right" +
                              "\nS-To move down" +
                              "\nD-To move Left" +
                              "\nWhen you reach a monster, move into them to do damage. But if they move into you, they do damage to you");
            Thread.Sleep(milliseconds);
            Console.WriteLine("\nGive it a try");
            stage(startingStage);
        }
        static void stage(int stage)
        {
            Console.Clear();
            Console.WriteLine("Welcome to stage " + stage);
            while (Collectables < 3 && playerHealth > 0)
            {
                Map();
                ShowHUD();
                Console.Write("\n");
                Legend();
                PlayerPOS();
                MimicPlacement();
                GruntPlacement();
                Mimic();
                UserInput();
                Mimic();
            }
            if (Collectables >= 3)
            {
                Win();
            }
            else
            {
                gameOver();
            }
        }
        static void Map()
        {
            Console.Clear();
            UpdateLog();
            // while loop shows off the map
            int current = 0;
            arrayString = File.ReadAllLines(path);
            arrayChar = new char[arrayString.Length, arrayString[0].Length];

            for (int k = 0; k < arrayString.Length; k++)
            {
                for (int l = 0; l < arrayString[0].Length; l++)
                {

                    arrayChar[k, l] = arrayString[k][l];

                    if (k == current)
                    {
                        Console.Write("\n");
                        current++;
                    }
                    if (arrayChar[k, l] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (arrayChar[k, l] == '.')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if (arrayChar[k, l] == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    if (arrayChar[k, l] == '~')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(arrayChar[k, l]);
                }
            }
            Console.Write('\n');
        }
        //Mimic MOvement

        static void Mimic()
        {
            int MimicMoveY;
            int MimicMoveX;
            int MimicPOS;
            MimicPOS = rnd.Next(1, 5);
            while (Playerturn == false)
            {
                if (MimicPOS == 1)
                {
                    MimicMoveY = Math.Max(MimicPOSy - 2, 0);
                    if (arrayChar[MimicMoveY, MimicPOSx] == '#')
                    {
                        EnemyInWater = false;
                        enemyDamage = 2;
                        MimicMoveY = MimicPOSy;
                        MimicPOSy = MimicMoveY;
                        if(MimicPOSy > MaxMimicPOSy)
                        {
                            MimicPOSy = MaxMimicPOSy;
                        }
                        Mimicaction = "Mimic moved to a wall";
                        Playerturn = true;
                        return;

                    }
                    if (arrayChar[MimicMoveY, MimicPOSx] == '+')
                    {
                        EnemyInWater = false;
                        playerHealth -= 1;
                        MimicPOSy--;
                        Mimicaction = "Mimic went on to a spike trap";
                        Playerturn = true;
                        return;
                    }
                    if (arrayChar[MimicMoveY, MimicPOSx] == '~')
                    {
                        EnemyInWater = true;
                        while(EnemyInWater== true)
                        {
                            enemyDamage = enemyDamage / 2;
                        }
                        Mimicaction = "Mimic is in water";
                        MimicPOSy--;
                        Playerturn = true;
                        return;
                    }
                    if (MimicMoveY == playerPOSy && playerPOSx == MimicPOSx)
                    {
                        
                        playerHealth -= enemyDamage;
                        if (enemyHealth <= 0)
                        {
                            enemyHealth = 0;
                            MimicPOSx = 0;
                            MimicPOSy = 0;
                        }
                        Mimicaction = "Mimic attacked the player";
                        Playerturn = true;
                        return;
                    }
                    if (MimicMoveY <= 0)
                    {
                        EnemyInWater = false;
                        MimicMoveY = 0;
                        Playerturn = true;
                        return;
                    }
                    else
                    {
                        EnemyInWater = false;
                        Mimicaction = "Mimic moved up";
                        MimicPOSy--;
                        Playerturn = true;
                        return;
                    }

                }
                if (MimicPOS == 2)
                {
                    MimicMoveX = Math.Max(MimicPOSx - 1, 0);

                    if (MimicMoveX <= 0)
                    {
                        EnemyInWater = false;
                        MimicMoveX = 0;
                        Playerturn = true;
                        return;
                    }
                    if (MimicMoveX == playerPOSx && playerPOSy == MimicPOSy)
                    {
                        enemyHealth -= playerDamage;
                        if (enemyHealth <= 0)
                        {
                            MimicPOSx = 0;
                            MimicPOSx = 0;
                        }
                        Playerturn = true;
                        return;
                    }

                    if (arrayChar[MimicPOSy, MimicMoveX] == '+')
                    {
                        EnemyInWater = false;
                        enemyHealth -= 1;
                        MimicPOSx--;
                        Playerturn = true;
                        return;
                    }
                    if (arrayChar[MimicPOSy, MimicMoveX] == '#')
                    {
                        EnemyInWater = false;
                        MimicMoveX = MimicPOSx;
                        MimicPOSx = MimicMoveX;
                        Playerturn = true;
                        return;
                    }
                    if (arrayChar[MimicPOSy,MimicMoveX] == '~')
                    {
                        EnemyInWater = true;
                        while(EnemyInWater == true)
                        {
                            enemyDamage = enemyDamage / 2;
                        }
                        MimicPOSx--;
                        Playerturn = true;
                        return;
                    }
                    else
                    {
                        
                        MimicPOSx = MimicMoveX;
                        if (MimicPOSx <= 0)
                        {
                            MimicPOSx = 0;
                        }
                        Playerturn = true;
                        return;
                    }
                }
                if (MimicPOS == 3)
                {

                    MimicMoveY = Math.Max(MimicPOSy, 0);
                    if (arrayChar[MimicMoveY, playerPOSx] == '#')
                    {
                        EnemyInWater = false;
                        MimicMoveY = playerPOSy;
                        playerPOSy = MimicMoveY;
                        Playerturn = true;
                        return;
                    }
                    if (arrayChar[MimicMoveY, MimicPOSx] == '+')
                    {
                        EnemyInWater = false;
                        enemyHealth -= 1;
                        if (enemyHealth <= 0)
                        {
                            enemyHealth = 0;
                        }
                        MimicPOSy++;
                        Playerturn = true;
                        return;
                    }
                    if (arrayChar[MimicMoveY, MimicPOSx] == '~')
                    {
                        EnemyInWater = true;
                        while(EnemyInWater == true)
                        {
                            enemyDamage = enemyDamage / 2;
                        }
                        MimicPOSy++;
                        Playerturn = true;
                        return;
                    }
                    if (MimicMoveY == MimicPOSy && playerPOSx == MimicPOSx)
                    {
                       
                        enemyHealth -= playerDamage;
                        if (enemyHealth <= 0)
                        {
                            enemyHealth = 0;
                            MimicPOSx = 0;
                            MimicPOSy = 0;
                        }
                        Playerturn = true;
                        return;
                    }
                    if (MimicMoveY <= 0)
                    {
                        
                        MimicMoveY = 0;
                        Playerturn = true;
                    }
                    else
                    {
                        EnemyInWater= false;
                        MimicPOSy++;
                        Playerturn = true;
                    }
                }
                if (MimicPOS == 4)
                {
                            MimicMoveX = Math.Max(MimicPOSx + 1, 0);

                            if (MimicMoveX <= 0)
                            {
                                EnemyInWater = false;
                                MimicMoveX = 0;
                                Playerturn = true;
                                return;
                            }
                            if (MimicMoveX == MimicPOSx && playerPOSy == MimicPOSy)
                            {
                                
                                enemyHealth -= playerDamage;
                                if (enemyHealth <= 0)
                                {
                                    MimicPOSx = 0;
                                    MimicPOSx = 0;
                                }
                                Playerturn = true;
                                return;
                            }

                            if (arrayChar[MimicPOSy, MimicMoveX] == '+')
                            {
                                EnemyInWater = false;
                                enemyHealth -= 1;
                                MimicPOSx++;
                                Playerturn = true;
                                return;
                            }
                            if (arrayChar[MimicPOSy, MimicMoveX] == '#')
                            {
                                EnemyInWater = false;
                                MimicMoveX = MimicPOSx;
                                MimicPOSx = MimicMoveX;
                                Playerturn = true;
                                return;
                            }
                            if (arrayChar[MimicPOSy,MimicMoveX] == '~')
                            {
                                EnemyInWater = true;
                                while(EnemyInWater == true)
                                {
                                    enemyDamage = enemyDamage / 2;
                                }
                                MimicPOSx++;
                                Playerturn = true;
                                return;
                            }
                            else
                            {
                                MimicPOSx = MimicMoveX;
                                if (MimicPOSx <= 0)
                                {
                                    MimicPOSx = 0;
                                }
                                Playerturn = true;
                                return;
                            }
                }

                Console.Clear();

            }
        }
        static void Grunt()
        {
           
        }
        static char TileCheck(int x, int y)
        {
            char[,] tempArrayChar = arrayChar;
            return tempArrayChar[x,y];
        }
        //Player Movement
        static void UserInput()
        {

            //Takes user input
            int moveX;
            int moveY;

            int newPlayerPOSx = playerPOSx;
            int newPlayerPOSy = playerPOSy;
            ConsoleKeyInfo Input = Console.ReadKey();
            switch (Input.KeyChar)
            {
                case 'w':
                    playerPOSy--;
                    if(TileCheck(playerPOSx,playerPOSy) == '#')
                    {
                        playerPOSy++;
                    }
                    break;
                case 'a':
                    playerPOSx--;
                    break;
                case 's':
                    playerPOSy++;
                    break;
                case 'd':
                    playerPOSx++;
                    break;

            }

        }

        //Placements of NPC's and Player

        static void PlayerPOS()
            {
                //Draws Player and sets them in the start position
                Console.SetCursorPosition(playerPOSx, playerPOSy);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("$");
                Console.ResetColor();
            }
        static void MimicPlacement()
        {
            //Draws out the Mimic
            Console.SetCursorPosition(MimicPOSx, MimicPOSy);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("@");
            Console.ResetColor();
        }
        static void GruntPlacement()
        {
            //Draws out the Grunt NPC
            Console.SetCursorPosition(GruntPOSx, GruntPOSy);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("G");
            Console.ResetColor();
        }
        static void Collect()
            {
                arrayChar[playerPOSx, playerPOSy] = '.';
                Collectables++;
                if (Collectables >= 5)
                {
                    Collectables = 5;
                    Win();
                }
            }

        //End game method

        static void gameOver()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Game Over");
                Thread.Sleep(milliseconds);
                Console.WriteLine("Press any key to restart");
                Console.ResetColor();
                Console.ReadKey();
                Main();
            }
        static void Win()
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Adventurer you did it! you collected the gold and ahve survived! Hooray!");
                Console.WriteLine("You win\nPress any key to quit");
                Console.ReadKey();
            }
    }
}