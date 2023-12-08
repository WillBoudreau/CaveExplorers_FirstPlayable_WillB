using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        static int Enemy1Health;
        static int Enemy1Attack;
        static int Enemy1POSX;
        static int Enemy1POSY;
        static bool Enemy1Alive;
        //Enemy 2 Variables
        static int Enemy2Health;
        static int Enemy2Attack;
        static int Enemy2POSX;
        static int Enemy2POSY;
        //Random Number Selec
        static Random rnd = new Random();
        //Game
        static bool EndGame;
        static bool PlayerTurn;
        static int EnemyCount;
        static int CollectMax;
        static bool Collactables;
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
        static void Main(string[] args)
        {
            OnStartUp();
            while (EnemyCount >= 0 | CollectMax <= PlayerCollectables)
            {
                
                MapArray();
                ShowHUD();
                ShowPlayer();
                ShowEnemy1();
                if (Enemy1Alive)
                {
                    Enemy1POSMove();
                }
                else
                {
                    PlayerTurn = true;
                }
                PlayerPOSMove();
                

            }
        }
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
            Enemy1POSX = 15;
            Enemy1POSY = 15;
            Enemy1Health = 3;
            Enemy1Attack = 1;
            // Enemy 2 initialization
            Enemy2POSX = 6;
            Enemy2POSY = 6;
            Enemy2Attack = 3;
            Enemy2Health = 1;
            //Game initialization
            EndGame = false;
            PlayerTurn = true;
            Enemy1Alive = true;
            EnemyCount = 1;
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
                        PlayerTurn = false;
                        break;
                    case 'a':
                        PlayerPOS(-1,0);
                        PlayerTurn = false;
                        break;
                    case 's':
                        PlayerPOS(0,1);
                        PlayerTurn = false;
                        break;
                    case 'd':
                        PlayerPOS(1, 0);
                        PlayerTurn = false;
                        break;
                }

            }
        }//<-- Player moves based off of the input
        static void PlayerPOS(int x, int y)
        {
            PlayerPOSX += x;
            PlayerPOSY += y;
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
                    break;
                case '*':
                    CollectCheck(x,y);
                    break;
            }
            

        }//<-- Checks the tiles on the map before the player moves on to them
        static int Enemy1Input()
        { if(Enemy1Alive == true)
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
        static void Enemy1POSMove()
        {
            if(PlayerTurn == false)
            {
                PlayerTurn = true;
                switch (Enemy1Input())
                {
                    case 1:
                        Enemy1POS(0, -1);
                        
                        break;
                    case 2:
                        Enemy1POS(-1, 0);
                        
                        break;
                    case 3:
                        Enemy1POS(0, 1);
                        
                        break;
                    case 4:
                        Enemy1POS(1, 0);
                        
                        break;
                }
            }
            
        }//<--Actually moves the Enemy1 based off of the RNG Generator
        static void Enemy1POS( int x, int y)
        {
            if(Enemy1Health > 0)
            {
                Enemy1POSX += x;
                Enemy1POSY += y;
                Combat(x, y);
                switch (IsTileValid(Enemy1POSX, Enemy1POSY))
                {
                    case '.':
                        break;
                    case '#':
                        Enemy1POSX -= x;
                        Enemy1POSY -= y;
                        break;
                    case '+':
                        Enemy1POSX -= x;
                        Enemy1POSY -= y;
                        break;
                    case '*':
                        break;
                }
            }
            
        }//<-- Checks the tiles before the Enemy 1 moves on to them
        //static int Enemy2Input()
        //{

        //}
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
            if(Enemy1Alive == true)
            {
                Console.SetCursorPosition(Enemy1POSX, Enemy1POSY);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("@");
                Console.ResetColor();
            }
            else
            {
                
            }
            
        }//<-- Shows off the Enemy on the map
        static void Combat( int x, int y) 
        {
            if(PlayerTurn == true)
            { 
                if (PlayerPOSX == Enemy1POSX && PlayerPOSY == Enemy1POSY)
                {
                   
                    Enemy1Health -= PlayerAttack;
                    if (Enemy1Health <= 0)
                    {
                        
                        Enemy1Health = 0;
                        Enemy1Alive = false;
                        Enemy1POSX = 0;
                        Enemy1POSY = 0;
                        EnemyCount--;
                        
                    }

                }
                if(PlayerPOSX == Enemy2POSX && PlayerPOSY == Enemy2POSY) 
                { 
                    Enemy2Health -= PlayerAttack;
                    if(Enemy2Health <= 0)
                    {
                        Enemy2Health = 0;
                    }
                }
                if (PlayerPOSX == Enemy1POSX && PlayerPOSY == Enemy1POSY)
                {
                    PlayerPOSX -= x;
                    PlayerPOSY -= y;
                }
                else if (PlayerPOSX == Enemy2POSX && PlayerPOSY == Enemy2POSY)
                {
                    PlayerPOSX -= x;
                    PlayerPOSY -= y;
                }

            }
            else
            {
                if(Enemy1POSX == PlayerPOSX && Enemy1POSY == PlayerPOSY)
                {
                    PlayerHealth -= Enemy1Attack;
                    if(PlayerHealth <= 0)
                    {
                        PlayerHealth = 0;
                        GameOver();
                    }
                }
                if(PlayerPOSX == Enemy2POSX &&  Enemy2POSY == PlayerPOSY)
                { 
                    PlayerHealth -= Enemy2Attack;
                    if(PlayerHealth <= 0)
                    {
                        PlayerHealth = 0;
                        GameOver();
                    }
                }
            }
        
        }//<-- Handles the combat
        static void GameOver()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Game Over");
            Console.Write("Press any key to quit");
            Console.ReadKey();
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
                if(Collect1 == false)
                {
                    MapChar[Collect1Y][Collect1X] = '.';
                }
                if (Collect2 == false)
                {
                    MapChar[Collect2Y][Collect2X] = '.';
                }
                if(Collect3 == false)
                {
                    MapChar[Collect3Y][Collect3X] = '.';
                }
                if (Collect4 == false)
                {
                    MapChar[Collect4Y][Collect4X] = '.';
                }
                if(Collect5 == false)
                {
                    MapChar[Collect5Y][Collect5X] = '.';
                }
                foreach (char c in MapChar[k])
                {
                    switch (c)
                    {
                        case '#':
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write(c);
                            break;
                        case '+':
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(c);
                            break;
                        case '.':
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write(c);
                            break;
                        case '*':
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write(c);
                            break;
                    }
                }
                Console.Write('\n');
            }
        }// <---Prints out the map 
        static void CollectCheck( int x, int y)
        {
            switch (PlayerCollectables)
            {
                case 0:
                    Collect1X = x;
                    Collect1Y = y;
                    Collect1 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    break;
                case 1:
                    Collect2X = x;
                    Collect2Y = y;
                    Collect2 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    break;
                case 2:
                    Collect3X = x;
                    Collect3Y = y;
                    Collect3 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    break;
                case 3:
                    Collect4X = x;
                    Collect4Y = y;
                    Collect4 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    break;
                case 4:
                    Collect5X = x;
                    Collect5Y = y;
                    Collect5 = false;
                    PlayerCollectables = PlayerCollectables + 1;
                    break;
            }
        }
        static void ShowHUD()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor. Cyan;
            Console.Write("Player Stats\n|Player Health " + PlayerHealth + "|Player Attack " + PlayerAttack + "|Current Collectables " + PlayerCollectables +"\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Enemy1 Stats\n|Enemy1 Health " + Enemy1Health + "|Enemy1 Attack " + Enemy1Attack + "|\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Enemy2 Stats\n|Enemy2 Health " + Enemy2Health + "|Enemy2 Attack " + Enemy2Attack + "|");
            
        }//<-- Shows the players hud
        static void Win()
        {

        }//<-- When the Player wins 
    }
}