using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

/*
Project Coding Conventions
DO 
- camelCasing on everything, except methods which use PascalCasing

- capitalize functions
- curly brackets always on their own line


Comments:
- Capitalized first letter, no punctation except commas
- Always starts with a verb

DONT
- forget to have fun :)


 */

namespace TextAdventure
{
    class Program
    {
        static void Main(string[] args)
        {
            bool quit = false;
            Initialization();
            
            // Displays the intro based on the initialization
            Intro();

            string action = "";

            // The game runs until quit is true
            do
            {
                
                action = PlayerAction();
             //   PlayerAction(lastAction);
                Console.Clear();
                Console.WriteLine(action);
                

                if(action.Contains("quit"))
                {
                    quit = true;
                }


                Console.WriteLine("I cannot do that...");

            } while(quit == false);


     
            Console.ReadKey();
            Console.Clear();
        }

        public static void Intro()
        {
            Console.WriteLine();
        }

        public static bool CheckOS()
        {
            // Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == true)
            {
                return true;
            }

            // OSX
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true)
            {
                return true;
            }
            
            // Unsupported
            else
            {
                return false;
            }
        }

        public static void Initialization()
        {
            // Makes the cursor invisible
            Console.CursorVisible = false;

            int height = 30;
            int width = 120;

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            string title = "Title";
            string border = "Border";

            //string title = File.ReadAllText("Title.txt");
            // string border = File.ReadAllText("Border.txt");



            Console.WriteLine(border);
            Console.SetCursorPosition(1, 3);
            Console.WriteLine(title);



            // Sets a bool to true or false depending on what system you are running the game on
            bool runningOnWindows = CheckOS();
            bool runningOnOSX = CheckOS();

            // Does a setup based on the outcome of checking the OS
            if (runningOnWindows == true)
            {
                SetupWindows(); 
            }

            else if (runningOnOSX == true)
            {
                SetupOSX();
            }

            else
            {
                SetupOther();
            }
        }

        public static void SetupWindows()
        {
            // Changes the window title to the title of the game
            Console.Title = "Transfer C100%MPLETE (Windows)";
        }

        public static void SetupOSX()
        {
            // Changes the window title to the title of the game
            Console.Title = "Transfer C100%MPLETE (OSX)";
        }

        public static void SetupOther()
        {
            // Changes the window title to the title of the game
            Console.Title = "Transfer C100%MPLETE (Other)";
        }

        public static string PlayerAction()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("What do I want to do?");
            Console.CursorVisible = true;
            string action = Console.ReadLine().ToLower();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            return action;
        }
    }
}
