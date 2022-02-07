using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

/*
Project Coding Conventions
DO 
- camelCasing on everything, except methods which use PascalCasing

- capitalize functions
- curly brackets always on their own line


Comments:
- Capitalized first letter, no punctation except commas
- Almost always starts with a verb

DONT
- forget to have fun :)
 */

// PLACEHOLDING OF CODE
/*
       
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
        const ConsoleColor narrativeColor = ConsoleColor.Gray;
        const ConsoleColor promptColor = ConsoleColor.White;
        const int printPauseTime = 50;
        static bool shouldQuit = false;
        static string input;
        static void Main()
        {
            //
            Initialization();

            //
            Intro();

            //Gameloop
            do
            {
                HandlePlayerAction();

                HandleGameRules();

            } while (shouldQuit == false);

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
        }

        public static void Intro()
        {
            // Reads the file containing the title art
            string title = File.ReadAllText("Title.txt");

            // Displays the title
            Console.Write(title);
            Console.ReadKey();
            Console.Clear();

            Console.ForegroundColor = narrativeColor;
            Print("This is a story about a man named Sta...");
        }

        public static void HandlePlayerAction()
        {
            // Asks the player what they want to do
            Print("What will I do?");
            Print("");

            // The fancy lil symbol showing the player they can write
            Console.Write("> ");

            // Shows cursor to indicate input for the player
            Console.CursorVisible = true;

            // Reads the input from the player
            input = Console.ReadLine().ToLowerInvariant();

            // Prevents input from being empty, aka idiot proofing for my own sake
            if (input == "")
            {
                return;
            }

            // Hides cursor
            Console.CursorVisible = false;

            string[] inputSplit = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string verb = inputSplit[0];

            // Reacts based on the player input
            switch (verb)
            {
                case "move":
                case "go":
                case "head":
                    HandleMoving("");
                    break;

                case "north":
                case "n":
                    HandleMoving("n");
                    break;

                case "south":
                case "s":
                    HandleMoving("s");
                    break;

                case "west":
                case "w":
                    HandleMoving("w");
                    break;

                case "east":
                case "e":
                    HandleMoving("e");
                    break;

                case "northwest":
                case "nw":
                    
                    HandleMoving("nw");
                    break;

                case "northeast":
                case "ne":
                    HandleMoving("ne");
                    break;

                case "southwest":
                case "sw":
                    HandleMoving("sw");
                    break;

                case "southeast":
                case "se":
                    HandleMoving("se");
                    break;

                // Looking
                case "look":
                case "watch":
                case "see":
                case "look at":
                case "view":
                    HandleLooking();
                    break;

                // Taking
                case "take":
                case "get":
                case "accuire":
                    HandleTaking();
                    break;

                // Using
                case "use":
                    HandleUsing();
                    break;

                // Dropping
                case "drop":
                case "dispose":
                case "trash":
                    HandleDropping();
                    break;

                // Helping
                case "help":
                    HandleHelping();
                    break;

                // Exiting the game
                case "end":
                case "quit":
                case "exit":
                    Print("See you later...");
                    shouldQuit = true;
                    break;

                default:
                    Print("Hmm...");
                    break;
            }
        }

        public static void HandleGameRules()
        {

        }

        // Using WriteLine
        static void Print(string text)
        {
            // Splits the lines based on the width of the window
            int maximumLine = Console.WindowWidth - 1;
            MatchCollection lineMatches = Regex.Matches(text, @"(.{1," + maximumLine + @"})(?:\s|$)");

            // Output each line with a small delay
            foreach (Match match in lineMatches)
            {
                Console.WriteLine(match.Groups[0].Value);
                Thread.Sleep(printPauseTime);
            }

        }

        // Handles movement
        public static void HandleMoving(string direction)
        {
            Print("Moving");
        }

        public static void HandleUsing()
        {
            Print("Using");
        }

        public static void HandleLooking()
        {
            Print("Looking");
        }

        public static void HandleTaking()
        {
            Print("Taking");
        }

        public static void HandleDropping()
        {
            Print("Dropping");
        }

        public static void HandleTalking()
        {
            Print("Talking");
        }

        public static void HandleHelping()
        {
            Print("Helping");
        }
    }
}