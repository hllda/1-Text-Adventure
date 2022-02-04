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

// PLACEHOLDING OF CODE
/*
 
 bool quit = false;
            
            string border = File.ReadAllText("Border.txt");
            File.ReadAllText("Border.txt");
            // Displays the intro based on the initialization
            Intro();

            string action = "";

            // The game runs until quit is true



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

 */


namespace TextAdventure
{
    class Program
    { 
        const ConsoleColor NarrativeColor = ConsoleColor.Gray;
        const ConsoleColor PromptColor = ConsoleColor.White;
        const int PrintPauseMilliseconds = 50;

        static void Main()
        {
            bool exit = false;
            
            //
            Initialization();

            //
            Introduction();

            //
           // Start();

            //Gameloop
            do
            {
                HandlePlayerAction();
                Console.SetCursorPosition(0, 0);

            } while (exit == false);

            Console.ReadKey();
            Console.Clear();
        }
        public static void Initialization()
        {
            // Makes the cursor invisible
            Console.CursorVisible = false;

            int height = 45;
            int width = 120;

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            string title = "Title";


            //string title = File.ReadAllText("Title.txt");





            Console.SetCursorPosition(1, 3);
            Console.WriteLine(title);




        }

        public static void Introduction()
        {
            Console.WriteLine();
        }

        public static void HandlePlayerAction()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("What will I do?");
            Console.CursorVisible = true;
            string action = Console.ReadLine().ToLower();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;



            Console.Clear();

            Console.WriteLine("I cannot do that...");


        }

        public static void HandleGameRules()
        {

        }

        static void Print(string text)
        {

        }

        public static void HandleMoving()
        {
        }

        public static void HandleLooking()
        {
        }

        public static void HandleTaking()
        {
        }

        public static void HandleDropping()
        {
        }

        public static void HandleTalking()
        {
        }

        public static void HandleHelping()
        {
        }

    }
}
