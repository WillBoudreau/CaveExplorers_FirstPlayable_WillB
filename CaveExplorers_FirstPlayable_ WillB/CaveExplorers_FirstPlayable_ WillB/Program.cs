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
        //Enemy
        static int MimicPOSx = 5;
        static int MimicPOSy = 5;
        static int MaxMimicPOSx = 10;
        static int MaxMimicPOSy = 10;
        static int MimicMove;
        //Collectables
        static int Collectables;
        //Time
        static int milliseconds;
        static int startingStage = 1;
        //float variables
        //Player
        static float playerHealth = 10;
        static float playerDamage = 2;
        //Enemy
        static float enemyHealth = 10;
        static float enemyDamage = 2;
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

        //bool variables
        static bool Playerturn = true;
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
            Console.WriteLine("Would you like to begin? Yes or No");
            gameStart = Console.ReadLine();
            if (gameStart == "Yes" | gameStart == "yes")
            {
                Menu();
            }
            if (gameStart == "Skip" | gameStart == "skip")
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
            Console.Write("| Health " + playerHealth + "|" +"Attack " + playerDamage +"|" + "Gold "+ Collectables);
        }
        static void Legend()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("|Player: &" + "|Enemy: #" + "|Gold: *" +"|Mountains: ^" + "|Spikes: +" + "|Water: ~|");
        }
        static void tutorial()
        {
            // Allows the Player to understand the game
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
            while (Collectables < 3)
            {
                Map();
                ShowHUD();
                Console.Write("\n");
                Legend();
                PlayerPOS();
                MimicPlacement();
                Mimic();
                UserInput();
                Mimic();
            }
            if (Collectables >= 3)
            {
                Win();
            }
        }
        static void Map()
        {
            Console.Clear();
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
                    if (arrayChar[k, l] == '+' && arrayChar[k, l] == '-' && arrayChar[k, l] == '|')
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
                    if (arrayChar[k,l] == '~')
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
            while (Playerturn == false)
            {
                MimicMove = rnd.Next(1, 5);
                //Enemy stats
                enemyDamage = 2;
                //Takes user input
                int mimicmoveX;
                int mimicmoveY;

                int newMimicPOSx = MimicPOSx;
                int newMimicPOSy = MimicPOSy;
                if (MimicMove == 1)
                {
                    mimicmoveY = Math.Max(MimicPOSy - 1, 0);
                    if (arrayChar[mimicmoveY, MimicPOSx] == '-' || arrayChar[mimicmoveY, MimicPOSx] == '|' || arrayChar[mimicmoveY, MimicPOSx] == '^')
                    {
                        mimicmoveY = MimicPOSy;
                        MimicPOSy = mimicmoveY;
                        Playerturn = true;


                    }
                    if (arrayChar[mimicmoveY, MimicPOSx] == '~')
                    {
                        enemyDamage = enemyDamage / 2;
                        MimicPOSy--;
                        Playerturn = true;
                    }
                    if (mimicmoveY == playerPOSy && MimicPOSx == playerPOSx)
                    {
                        playerHealth = playerHealth - 1;
                        if (playerHealth <= 0)
                        {
                            gameOver();
                        }
                        Playerturn = true;
                        return;
                    }
                    if (mimicmoveY <= 0)
                    {
                        mimicmoveY = 0;
                        Playerturn = true;
                    }
                    else
                    {
                        MimicPOSy--;
                        Playerturn = true;
                    }

                }
                if (MimicMove == 2)
                {
                    mimicmoveX = Math.Max(MimicPOSx - 1, 0);
                    if (arrayChar[mimicmoveX, MimicPOSy] == '-' || arrayChar[mimicmoveX, MimicPOSy] == '|' || arrayChar[mimicmoveX, MimicPOSy] == '^')
                    {
                        mimicmoveX = MimicPOSx;
                        MimicPOSx = mimicmoveX;
                        Playerturn = true;

                    }
                    if (arrayChar[mimicmoveX, MimicPOSy] == '~')
                    {
                        enemyDamage = enemyDamage / 2;
                        MimicPOSx--;
                        Playerturn = true;
                    }
                    if (mimicmoveX == playerPOSy && MimicPOSx == playerPOSx)
                    {
                        playerHealth = playerHealth - 1;
                        if (playerHealth <= 0)
                        {
                            gameOver();
                        }
                        Playerturn = true;
                        return;
                    }
                    if (mimicmoveX <= 0)
                    {
                        mimicmoveX = 0;
                        Playerturn = true;
                    }
                    else
                    {
                        MimicPOSx--;
                        Playerturn = true;
                    }
                }
                if (MimicMove == 3)
                {

                    mimicmoveY = Math.Max(MimicPOSy + 1, 0);
                    if (arrayChar[mimicmoveY, MimicPOSx] == '-' || arrayChar[mimicmoveY, MimicPOSx] == '|' || arrayChar[mimicmoveY, MimicPOSx] == '^')
                    {
                        mimicmoveY = MimicPOSy;
                        MimicPOSy = mimicmoveY;
                        Playerturn = true;

                    }
                    if (arrayChar[mimicmoveY, MimicPOSx] == '~')
                    {
                        enemyDamage = enemyDamage / 2;
                        MimicPOSy--;
                        Playerturn = true;
                    }
                    if (mimicmoveY == playerPOSy && MimicPOSx == playerPOSx)
                    {
                        playerHealth = playerHealth - 1;
                        if (playerHealth <= 0)
                        {
                            gameOver();
                        }
                        Playerturn = true;
                        return;
                    }
                    if (mimicmoveY <= 0)
                    {
                        mimicmoveY = 0;
                        Playerturn = true;
                    }
                    else
                    {
                        MimicPOSy--;
                        Playerturn = true;

                    }

                }
                if (MimicMove == 4)
                {
                    mimicmoveX = Math.Max(MimicPOSx + 1, 0);

                    if (arrayChar[mimicmoveX, MimicPOSy] == '-' || arrayChar[mimicmoveX, MimicPOSy] == '|' || arrayChar[mimicmoveX, MimicPOSy] == '^')
                    {
                        mimicmoveX = MimicPOSx;
                        MimicPOSx = mimicmoveX;
                        Playerturn = true;

                    }
                    if (arrayChar[mimicmoveX, MimicPOSy] == '~')
                    {
                        enemyDamage = enemyDamage / 2;
                        MimicPOSx++;
                        Playerturn = true;
                    }
                    if (mimicmoveX == playerPOSy && MimicPOSx == playerPOSx)
                    {
                        playerHealth = playerHealth - 1;
                        if (playerHealth <= 0)
                        {
                            gameOver();
                        }
                        Playerturn = true;
                        return;
                    }
                    if (mimicmoveX <= 0)
                    {
                        mimicmoveX = 0;
                        Playerturn = true;
                    }
                    else
                    {
                        MimicPOSx++;
                        Playerturn = true;
                    }
                }
            }
        }

        //Player Movement 

        static void UserInput()
        {

            //Takes user input
            int moveX;
            int moveY;

            int newPlayerPOSx = playerPOSx;
            int newPlayerPOSy = playerPOSy;


            while (Playerturn == true)
            {
                playerControl = Console.ReadKey(true);
                //Player pushes W
                if (playerControl.Key == ConsoleKey.W)
                {
                    moveY = Math.Max(playerPOSy - 1, 0);
                    if (arrayChar[moveY, playerPOSx] == '-' || arrayChar[moveY, playerPOSx] == '|' || arrayChar[moveY, playerPOSx] == '^')
                    {
                        moveY = playerPOSy;
                        playerPOSy = moveY;
                        Playerturn = false;
                        return;

                    }
                    if (arrayChar[moveY, playerPOSx] == '+')
                    {
                        playerHealth -= 1;
                        playerPOSy--;
                        if (playerHealth <= 0)
                        {
                            gameOver();
                        }
                        Playerturn = false;
                    }
                    if (arrayChar[moveY, playerPOSx] == '~')
                    {
                        playerDamage = playerDamage / 2;
                        playerPOSy--;
                        Playerturn = false;
                    }
                    if (arrayChar[moveY, playerPOSx] == '*')
                    {
                        Console.Write("You got Gold!");
                        Collect();
                        playerPOSy--;
                        Playerturn = false;
                    }
                    if (moveY == MimicPOSy && playerPOSx == MimicPOSx)
                    {
                        enemyHealth = enemyHealth - 1;
                        if (enemyHealth <= 0)
                        {
                            enemyHealth = 0;
                            MimicPOSx = 0;
                            MimicPOSy = 0;

                        }
                        Playerturn = false;
                        return;
                    }
                    if (moveY <= 0)
                    {
                        moveY = 0;
                        Playerturn = false;
                    }
                    else
                    {
                        playerPOSy--;
                        Playerturn = false;

                    }

                }
                //Player Pushes A
                if (playerControl.Key == ConsoleKey.A)
                {
                    moveX = Math.Max(playerPOSx - 1, 0);

                    if (moveX <= 0)
                    {
                        moveX = 0;
                        Playerturn = false;
                    }
                    if (moveX == MimicPOSx && playerPOSy == MimicPOSy)
                    {
                        enemyHealth -= 1;
                        if (enemyHealth <= 0)
                        {
                            MimicPOSx = 0;
                            MimicPOSx = 0;
                        }
                        Playerturn = false;
                        return;
                    }

                    if (arrayChar[playerPOSy, moveX] == '+')
                    {
                        playerHealth -= 1;
                        playerPOSx--;
                        if (playerHealth <= 0)
                        {
                            gameOver();
                        }
                        Playerturn = false;
                    }
                    if (arrayChar[moveX, playerPOSy] == '*')
                    {
                        Console.Write("You got Gold!");
                        Collect();
                        playerPOSx--;
                        Playerturn = false;
                    }

                    if (arrayChar[playerPOSy, moveX] == '|' || arrayChar[playerPOSy, moveX] == '-')
                    {
                        moveX = playerPOSx;
                        playerPOSx = moveX;
                        Playerturn = false;
                        return;
                    }


                    else
                    {
                        playerPOSx = moveX;
                        if (playerPOSx <= 0)
                        {
                            playerPOSx = 0;
                        }
                        Playerturn = false;
                    }


                }

                //Player pushes S 
                if (playerControl.Key == ConsoleKey.S)
                {

                    moveY = Math.Max(playerPOSy + 1, 0);
                    if (arrayChar[moveY, playerPOSx] == '-' || arrayChar[moveY, playerPOSx] == '|' || arrayChar[moveY, playerPOSx] == '^')
                    {
                        moveY = playerPOSy;
                        playerPOSy = moveY;
                        Playerturn = false;
                        return;
                    }
                    if (arrayChar[moveY, playerPOSx] == '+')
                    {
                        playerHealth -= 1;
                        playerPOSy++;
                        Playerturn = false;
                        if (playerHealth <= 0)
                        {
                            gameOver();
                        }
                    }
                    if (arrayChar[moveY, playerPOSx] == '~')
                    {
                        playerDamage = playerDamage / 2;
                        playerPOSy++;
                        Playerturn = false;
                    }
                    if (arrayChar[moveY, playerPOSx] == '*')
                    {
                        Console.Write("You got Gold!");
                        arrayChar[moveY, playerPOSx] = '.';
                        Collect();
                        playerPOSy++;
                        Playerturn = false;
                    }
                    if (moveY == MimicPOSy && playerPOSx == MimicPOSx)
                    {
                        enemyHealth = enemyHealth - 1;
                        if (enemyHealth <= 0)
                        {
                            enemyHealth = 0;
                            MimicPOSx = 0;
                            MimicPOSy = 0;

                        }
                        Playerturn = false;
                        return;
                    }
                    if (moveY <= 0)
                    {
                        moveY = 0;
                        Playerturn = false;
                    }
                    else
                    {
                        playerPOSy++;
                        Playerturn = false;
                    }

                }
                //Player pushes D
                {
                    if (playerControl.Key == ConsoleKey.D)
                    {
                        moveX = Math.Max(playerPOSx + 1, 0);

                        if (moveX <= 0)
                        {
                            moveX = 0;
                            Playerturn = false;
                        }
                        if (moveX == MimicPOSx && playerPOSy == MimicPOSy)
                        {
                            enemyHealth -= 1;
                            if (enemyHealth <= 0)
                            {
                                MimicPOSx = 0;
                                MimicPOSx = 0;
                            }
                            Playerturn = false;
                            return;
                        }

                        if (arrayChar[playerPOSy, moveX] == '+')
                        {
                            playerHealth -= 1;
                            playerPOSx++;
                            if (playerHealth <= 0)
                            {
                                gameOver();
                            }
                            Playerturn = false;
                        }
                        if (arrayChar[playerPOSy, moveX] == '*')
                        {
                            Console.Write("You got Gold!");
                            arrayChar[playerPOSy, moveX] = '.';
                            Collect();
                            playerPOSy--;
                            Playerturn = false;
                        }

                        if (arrayChar[playerPOSy, moveX] == '|' || arrayChar[playerPOSy, moveX] == '-')
                        {
                            moveX = playerPOSx;
                            playerPOSx = moveX;
                            Playerturn = false;
                            return;
                        }


                        else
                        {
                            playerPOSx = moveX;
                            if (playerPOSx <= 0)
                            {
                                playerPOSx = 0;
                            }
                            Playerturn = false;
                        }


                    }
                }
                Console.Clear();
            }
        }

        //Placements of NPC's and Player

        static void PlayerPOS()
        {
            //Draws Player and sets them in the start position
            Console.SetCursorPosition(playerPOSx, playerPOSy);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("&");
            Console.ResetColor();
        }
        static void MimicPlacement()
        {
            //Draws out the Mimic
            Console.SetCursorPosition(MimicPOSx, MimicPOSy);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("#");
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