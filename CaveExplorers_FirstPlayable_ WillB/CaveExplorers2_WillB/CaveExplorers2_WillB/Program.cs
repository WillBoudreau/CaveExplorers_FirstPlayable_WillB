using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace CaveExplorers2_WillB
{
    internal class Program
    {

        //Map Variables
        static string[] MapStr;
        static char[][] MapChar;
        static string path;
        //Player Variables 
        static int PlayerPOSX;
        static int PlayerPOSY;
        static int PlayerHealth;
        static int PlayerCollectables;
        static int PlayerAttack;
        //Map variables
        //Enemy 1 Variables
        static int EnemyOneHealth;
        static int EnemyOneAttack;
        static int EnemyOnePOSX;
        static int EnemyOnePOSY;
        static bool EnemyOneAlive;
        //Enemy 2 Variables
        static int EnemyTwoHealth;
        static int EnemyTwoAttack;
        static int EnemyTwoPOSX;
        static int EnemyTwoPOSY;
        static bool EnemyTwoAlive;
        //Random Number Selec
        static Random rnd = new Random();
        //Game
        static bool EndGame;
        static bool PlayerTurn;
        static int EnemyCount;
        static int CollectMax;
        static bool Collactables;
        static float score;
        static string annoyPlayer;
        //Collectables
        static bool Collect1;
        static int Collect1X;
        static int Collect1Y;
        static bool Collect2;
        static int Collect2X;
        static int Collect2Y;
        static bool Collect3;
        static int Collect3X;
        static int Collect3Y;
        static bool Collect4;
        static int Collect4X;
        static int Collect4Y;
        static bool Collect5;
        static int Collect5X;
        static int Collect5Y;
        static void Main()
        {
           
                Console.Clear();
                OnStartUp();
                while (EnemyCount > 0)
                {

                    MapArray();
                    Console.Write("\n");
                    ShowHUD();
                    ShowPlayer();
                    ShowEnemy1();
                    ShowEnemy2();
                    PlayerPOSMove();
                    if (EnemyOneAlive)
                    {
                        EnemyOnePOSMove();

                    }
                    if (EnemyTwoAlive)
                    {
                        EnemytwoPOSMove();
                    }
                    PlayerTurn = true;
                    


                }
                Win();
            
        }
        static void AnnyingThePlayer()
        {
            Console.WriteLine("Are you sure?");
            annoyPlayer = Console.ReadLine();
            if (annoyPlayer == "Yes" | annoyPlayer == "yes")
            {
                AnnyingThePlayer();
            }
            else
            {   Console.Clear();
                Console.WriteLine("Alright then lets start, this time from the beginning...");
                Main();

            }

        }//<-- Annoys the player into starting the game
            
        static void OnStartUp()
        {
            Console.CursorVisible = false;
            //Player initialization
            PlayerPOSX = 3;
            PlayerPOSY = 3;
            PlayerHealth = 3;
            PlayerCollectables = 0;
            PlayerAttack = 3;
            //Enemy 1 initialization
            EnemyOnePOSX = 15;
            EnemyOnePOSY = 15;
            EnemyOneHealth = 3;
            EnemyOneAttack = 1;
            // Enemy 2 initialization
            EnemyTwoPOSX = 6;
            EnemyTwoPOSY = 7;
            EnemyTwoAttack = 1;
            EnemyTwoHealth = 6;
            //Game initialization
            EndGame = false;
            PlayerTurn = true;
            EnemyOneAlive = true;
            EnemyTwoAlive = true;
            EnemyCount = 2;
            CollectMax = 5;
            //Collectables
            Collect1 = true;
            Collect2 = true;
            Collect3 = true;
            Collect4 = true;
            Collect5 = true;
        }//<--Set up the starting variables
        static char PlayerInput()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.KeyChar == 'w')
            {
                return 'w';
            }
            else if (key.KeyChar == 'a')
            {
                return 'a';
            }
            else if (key.KeyChar == 's')
            {
                return 's';
            }
            else if (key.KeyChar == 'd')
            {
                return 'd';
            }
            else
            {
                return 'e';
            }
        }//<-- Takes in Player input
        static void PlayerPOSMove()
        {
            if(PlayerTurn == true)
            {
                switch (PlayerInput())
                {
                    case 'w':
                        PlayerPOS(0,-1);
                        
                        break;
                    case 'a':
                        PlayerPOS(-1,0);
                        
                        break;
                    case 's':
                        PlayerPOS(0,1);
                        
                        break;
                    case 'd':
                        PlayerPOS(1, 0);
                        
                        break;
                }

            }
        }//<-- Player moves based off of the input
        static void PlayerPOS(int x, int y)
        {
            PlayerPOSX += x;
            PlayerPOSY += y;
            PlayerTurn = true;
            Combat(x,y);
            switch (IsTileValid(PlayerPOSX, PlayerPOSY))
            {
                case '.':
                    break;
                case '#':
                    PlayerPOSX -= x;
                    PlayerPOSY -= y;
                    break;
                case '+':
                    PlayerHealth -= 1;
                    if(PlayerHealth <= 0)
                    {
                        PlayerHealth = 0;
                        GameOver();
                    }
                    break;
                case '*':
                    CollectCheck(PlayerPOSX,PlayerPOSY);
                    break;
            }
        }//<-- Checks the tiles on the map before the player moves on to them
        static int EnemyOneInput()
        { if(EnemyOneAlive == true)
            {
                int Move = rnd.Next(1, 5);
                if (Move == 1)
                {
                    return 1;
                }
                else if (Move == 2)
                {
                    return 2;
                }
                else if (Move == 3)
                {
                    return 3;
                }
                else if (Move == 4)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                return 0;
            }
            
        }//<-- Checks where the Enemy1 moves based off of a RNG generator between 1 and 4
        static void EnemyOnePOSMove()
        {
            switch (EnemyOneInput())
            {
                case 1:
                    EnemyOnePOS(0, -1);

                    break;
                case 2:
                    EnemyOnePOS(-1, 0);

                    break;
                case 3:
                    EnemyOnePOS(0, 1);

                    break;
                case 4:
                    EnemyOnePOS(1, 0);

                    break;
            }

        }//<--Actually moves the Enemy1 based off of the RNG Generator
        static void EnemyOnePOS( int x, int y)
        {
            if(EnemyOneHealth > 0)
            {
                EnemyOnePOSX += x;
                EnemyOnePOSY += y;
                PlayerTurn = false;
                Combat(x, y);
                switch (IsTileValid(EnemyOnePOSX, EnemyOnePOSY))
                {
                    case '.':
                        break;
                    case '#':
                        EnemyOnePOSX -= x;
                        EnemyOnePOSY -= y;
                        break;
                    case '+':
                        EnemyOnePOSX -= x;
                        EnemyOnePOSY -= y;
                        break;
                    case '*':
                        break;
                }
            }
            
        }//<-- Checks the tiles before the Enemy 1 moves on to them
        static int EnemyTwoInput()
        {
            if (PlayerPOSY < EnemyTwoPOSY)
            {
                return 1;
            }
            else if (PlayerPOSX < EnemyTwoPOSX)
            {
                return 2;
            }
            else if (PlayerPOSY > EnemyTwoPOSY)
            {
                return 3;
            }
            else if (PlayerPOSX > EnemyTwoPOSX)
            {
                return 4;
            }
            else
            {
                return 5;
            }

        }//<-- Uses the Players position to determine the Enemy2's position
        static void EnemytwoPOSMove()
        {
            switch(EnemyTwoInput())
            {
                case 1:
                    EnemyTwoPOS(0, -1);

                    break;
                case 2:
                    EnemyTwoPOS(-1, 0);

                    break;
                case 3:
                    EnemyTwoPOS(0, 1);

                    break;
                case 4:
                    EnemyTwoPOS(1, 0);

                    break;
            }
            //case 0:
            //    break;
            //case 1:
            //    if (IsTileValid(EnemyTwoPOSX, (EnemyTwoPOSY - 1)) == '.')
            //    {
            //        EnemyTwoPOSY = EnemyTwoPOSY - 1;
            //    }
            //    break;
            //case 2:
            //    if (IsTileValid((EnemyTwoPOSX - 1), (EnemyTwoPOSY)) == '.')
            //    {
            //        EnemyTwoPOSX = EnemyTwoPOSX - 1;
            //    }
            //    break;
            //case 3:
            //    if(IsTileValid(EnemyTwoPOSX,(EnemyTwoPOSY + 1)) ==  '.')
            //    {
            //        EnemyTwoPOSY = EnemyTwoPOSY + 1;
            //    }
            //    break;
            //case 4:
            //    if(IsTileValid((EnemyTwoPOSX + 1),(EnemyTwoPOSY)) == '.')
            //    {
            //        EnemyTwoPOSX = EnemyTwoPOSX + 1;
            //    }
            //    break;
        }//<-- Moves the Enemy2
        static void EnemyTwoPOS(int x,int y)
        {
            if (EnemyTwoHealth > 0)
            {
                EnemyTwoPOSX += x;
                EnemyTwoPOSY += y;
                Combat(x, y);
                switch (IsTileValid(EnemyTwoPOSX, EnemyTwoPOSY))
                {
                    case '.':
                        break;
                    case '#':
                        EnemyTwoPOSX -= x;
                        EnemyTwoPOSY -= y;
                        break;
                    case '+':
                        EnemyTwoPOSX -= x;
                        EnemyTwoPOSY -= y;
                        break;
                    case '*':
                        break;
                }
            }
        }//<-- Checks Enemy2's position
        static char IsTileValid(int x, int y)// <-- Simply checks the tiles
        {
            return MapChar[y][x];
        }
        static void ShowPlayer() 
        {
            Console.SetCursorPosition(PlayerPOSX, PlayerPOSY);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("$");
            Console.ResetColor();
        }//<-- Shows the player on the map
        static void ShowEnemy1()
        {
            if(EnemyOneAlive == true)
            {
                Console.SetCursorPosition(EnemyOnePOSX, EnemyOnePOSY);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("G");
                Console.ResetColor();
            }
            else
            {
                
            }
            
        }//<-- Shows off the Enemy on the map
        static void ShowEnemy2()
        {
            if (EnemyTwoAlive)
            {
                Console.SetCursorPosition(EnemyTwoPOSX, EnemyTwoPOSY);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("C");
                Console.ResetColor();
            }
        }//Displays the Enemy2
        static void Combat( int x, int y) 
        {
            if(PlayerTurn == true)
            { 
                if (PlayerPOSX == EnemyOnePOSX && PlayerPOSY == EnemyOnePOSY)
                {
                   
                    EnemyOneHealth -= PlayerAttack;
                    if (EnemyOneHealth <= 0)
                    {
                        
                        EnemyOneHealth = 0;
                        EnemyOneAlive = false;
                        EnemyOnePOSX = 0;
                        EnemyOnePOSY = 0;
                        EnemyCount--;
                        score++;
                    }

                }//<-- During the players trun Enemy 1 takes damage
                if(PlayerPOSX == EnemyTwoPOSX && PlayerPOSY == EnemyTwoPOSY) 
                { 
                    EnemyTwoHealth -= PlayerAttack;
                    if(EnemyTwoHealth <= 0)
                    {
                        EnemyTwoHealth = 0;
                        EnemyTwoAlive = false;
                        EnemyTwoPOSX = 0;
                        EnemyTwoPOSY = 0;
                        EnemyCount--;
                        score++;
                    }
                }//<-- During the players turn Enemy 2 takes damage
                if (PlayerPOSX == EnemyOnePOSX && PlayerPOSY == EnemyOnePOSY)
                {
                    PlayerPOSX -= x;
                    PlayerPOSY -= y;
                }//<-- Player cannot move on top of Enemy 1
                else if (PlayerPOSX == EnemyTwoPOSX && PlayerPOSY == EnemyTwoPOSY)
                {
                    PlayerPOSX -= x;
                    PlayerPOSY -= y;
                }//<-- Player cannot move on top of Enemy 2

            }
            else
            {
                
                if (PlayerPOSX == EnemyOnePOSX && PlayerPOSY == EnemyOnePOSY)
                {

                    EnemyOnePOSX -= x;
                    EnemyOnePOSY -= y;
                    PlayerHealth -= EnemyOneAttack;
                    if (PlayerHealth <= 0)
                    {
                        PlayerHealth = 0;
                        GameOver();
                    }

                }//<-- During Enemy1 turn Player takes damage
                if (PlayerPOSX == EnemyTwoPOSX && PlayerPOSY == EnemyTwoPOSY)
                {
                    EnemyTwoPOSX -= x;
                    EnemyTwoPOSY -= y;
                    PlayerHealth -= EnemyTwoAttack;
                    if (PlayerHealth <= 0)
                    {
                        PlayerHealth = 0;
                        GameOver();
                    }
                }//<--During Enemy2 turn Player takes damage






                //if(EnemyOnePOSX == PlayerPOSX && EnemyOnePOSY == PlayerPOSY)
                //{
                //    PlayerHealth -= EnemyOneAttack;
                //    if(PlayerHealth <= 0)
                //    {
                //        PlayerHealth = 0;
                //        GameOver();
                //    }
                //}
                //if(PlayerPOSX == EnemyTwoPOSX && EnemyTwoPOSY == EnemyOnePOSY)
                //{ 
                //    PlayerHealth -= EnemyTwoAttack;
                //    if(PlayerHealth <= 0)
                //    {
                //        PlayerHealth = 0;
                //        GameOver();
                //    }
                //}
            }
        
        }//<-- Handles the combat
        static void GameOver()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Game Over");
            Console.Write("Press any key to quit");
            Console.ReadKey();
            Environment.Exit(1);
        }//<-- Whne the player dies, game ends 
        static void MapArray()
        {
            Console.SetCursorPosition(0, 0);
            path = @"TextRPGMap.txt";
            MapStr = File.ReadAllLines(path);
            int Mapy = MapStr.Length;
            int Mapx = MapStr[0].Length;
            MapChar = new char[Mapy][];
            
            for (int k = 0; k < Mapy; k++)
            {
                MapChar[k] = MapStr[k].ToCharArray();

                //foreach (char c in MapChar[k])
                //{

                        //    }
                        //}
                        //Console.Write('\n');
                }
            if (Collect1 == false)
            {
                MapChar[Collect1Y][Collect1X] = '.';
            }
            if (Collect2 == false)
            {
                MapChar[Collect2Y][Collect2X] = '.';
            }
            if (Collect3 == false)
            {
                MapChar[Collect3Y][Collect3X] = '.';
            }
            if (Collect4 == false)
            {
                MapChar[Collect4Y][Collect4X] = '.';
            }
            if (Collect5 == false)
            {
                MapChar[Collect5Y][Collect5X] = '.';
            }
            DisplayMap();
        }// <---Prints out the map 
        static void DisplayMap()
        {
            for( int i = 0; i < MapChar.Length; i++ )
            {
                for(int k= 0; k < MapChar[i].Length; k++ )
                {
                    Console.SetCursorPosition(k, i);
                    switch (MapChar[i][k])
                    {
                        case '#':
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write(MapChar[i][k]);
                            break;
                        case '+':
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.WriteLine(MapChar[i][k]);                            
                            break;
                        case '.':
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(MapChar[i][k]);
                            break;
                        case '*':
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write(MapChar[i][k]); 
                            break;
                    }
                           
                }
            }
        }//<-- Displays the Map to the player
        static void CollectCheck( int x, int y)
        {
            switch (PlayerCollectables)
            {
                case 0:
                    Collect1X = x;
                    Collect1Y = y;
                    Collect1 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    score = score + 2;
                    break;
                case 1:
                    Collect2X = x;
                    Collect2Y = y;
                    Collect2 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    score = score + 2;
                    break;
                case 2:
                    Collect3X = x;
                    Collect3Y = y;
                    Collect3 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    score = score + 2;
                    break;
                case 3:
                    Collect4X = x;
                    Collect4Y = y;
                    Collect4 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    score = score + 2;
                    break;
                case 4:
                    Collect5X = x;
                    Collect5Y = y;
                    Collect5 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    score = score + 2;
                    break;
            }
        }//<--Check to see if the player is on a gold slot
        static void ShowHUD()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Player Stats\n|Health:" + PlayerHealth + "    | Attack " + PlayerAttack + "    | Current Collectables " + PlayerCollectables +"\n");

            Console.Write("Grunt Stats\n|Health:" + EnemyOneHealth + "    | Attack " + EnemyOneAttack + "    |\n");

            Console.Write("Chaser Stats\n|Health:" + EnemyTwoHealth + "    | Attack " + EnemyTwoAttack + "    |\n");
            
        }//<-- Shows the players hud
        static void Win()
        {
            Console.Clear();
            Console.WriteLine("You win! Congrats!");
            if(PlayerHealth == 3)
            {
                score = score * 1.5f;
            }
            if(PlayerHealth == 2)
            {
                score = score * 1.2f;
            }
            else if(PlayerHealth == 1)
            {
                score = score * 1.0f;
            }
            Console.WriteLine("You got: " + score +" Score");
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }//<-- When the Player wins 
    }
}