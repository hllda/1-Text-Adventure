using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

/*
Coding Conventions
- camelCasing for everything except Methods
- curly brackets on their own line
- 
*/

/*
   namespace TransferC100MPLETE
{

    // Here the program starts :D
    class Program
    {
        const ConsoleColor narrativeColor = ConsoleColor.DarkCyan;
        const ConsoleColor promptColor = ConsoleColor.Cyan;
        const int printPauseTime = 50;
        static bool shouldQuit = false;
        static string input;

        LocationId location = Enum.Parse<LocationId>(File.ReadAllText("Locations.txt"));

        string locations = File.ReadAllText("Locations.txt");



       // LocationId locatione = Enum.Parse<LocationId>(locationIdText);

        // Data dictionaries
        static Dictionary<LocationId, LocationData> LocationsData = new Dictionary<LocationId, LocationData>();

        // Current state
        static LocationId CurrentLocationId = LocationId.Vat;

        static void Main()
        {
            // Initialization
            Initialization();

            // Displays the intro
            Intro();

            Console.ReadKey();

            // Gameloop
            do
            {
                HandlePlayerAction();
                HandleGameRules();
            } while (!shouldQuit);
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
            Console.ForegroundColor = promptColor;
            // Reads the file containing the title art
            string title = File.ReadAllText("Title.txt");

            // Displays the title
            Console.Write(title);
            Console.ReadKey();
            Console.Clear();

            Console.ForegroundColor = narrativeColor;
            Print("This is a story about a man named Sta...");
            Print("Story");
            Print("More story");
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

            // Analyze the command by assuming the first word is a verb (or similar instruction).
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string verb = words[0];


           
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

        static void DisplayLocation()
        {
            Console.Clear();

            // Display current location description.
            LocationData currentLocationData = LocationsData[CurrentLocationId];
            Print(currentLocationData.Description);
        }

      
    }
}
*/

namespace TransferC100MPLETE
{
    class Program
    {
        static bool quit = false;
        const ConsoleColor NarrativeColor = ConsoleColor.DarkCyan;
        const ConsoleColor PromptColor = ConsoleColor.Cyan;
        const int PrintPauseMilliseconds = 50;
      
        static void Main()
        {
            
            Initialize();
            Intro();

            // Gameloop
            do
            {
                HandlePlayerAction();
                HandleGameRules();
            } while (!quit);
        }

        static void Initialize()
        {
            // Hides the cursor
            Console.CursorVisible = false;

        }

        static void Intro()
        {
            // Display title screen.
            string titleArt = File.ReadAllText("TitleArt.txt");
            Console.Write(titleArt);
            Console.ReadKey();
            Console.Clear();
        }

        static void HandlePlayerAction()
        {
            Print("What will I do?");
            Console.Write(">");

            Console.CursorVisible = true;
            string command = Console.ReadLine().ToLowerInvariant();
            Console.CursorVisible = false;

            Console.Clear();

            string[] words = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string verb = words[0];

            // Reacts based on the player input
            switch (verb)
            {
                case "move":
                case "go":
                case "head":
                    HandleMove("");
                    break;

                case "north":
                case "n":
                    HandleMove("n");
                    break;

                case "south":
                case "s":
                    HandleMove("");
                    break;

                case "west":
                case "w":
                    HandleMove("");
                    break;

                case "east":
                case "e":
                    HandleMove("");
                    break;

                case "northwest":
                case "nw":

                    HandleMove("");
                    break;

                case "northeast":
                case "ne":
                    HandleMove("");
                    break;

                case "southwest":
                case "sw":
                    HandleMove("");
                    break;

                case "southeast":
                case "se":
                    HandleMove("");
                    break;

                // Looking
                case "look":
                case "watch":
                case "see":
                case "look at":
                case "view":
                    HandleLook();
                    break;

                // Taking
                case "take":
                case "get":
                case "accuire":
                    HandleTake();
                    break;

                // Using
                case "use":
                    HandleUse();
                    break;

                // Dropping
                case "drop":
                case "dispose":
                case "trash":
                    HandleDrop();
                    break;

                // Helping
                case "help":
                    HandleHelp();
                    break;

                // Exiting the game
                case "end":
                case "quit":
                case "exit":
                    Reply("See you soon...");
                    quit = true;
                    break;

                default:
                    Reply("Hmm...");
                    break;

            }
        }

        static void HandleGameRules()
        {

        }       

        static void Print(string text)
        {
            // Split text into lines that don't exceed the window width.
            int maximumLineLength = Console.WindowWidth - 1;
            MatchCollection lineMatches = Regex.Matches(text, @"(.{1," +maximumLineLength + @"})(?:\s|$)");

            // Output each line with a small delay.
            foreach (Match match in lineMatches)
            {
                Console.WriteLine(match.Groups[0].Value);
                Thread.Sleep(PrintPauseMilliseconds);
            }
        }

        static void Reply(string text)
        {
            Print(text);
            Print("");
        }
            
        static void HandleMove(string direction)
        {
                Print("Moving " + direction);
        }

        public static void HandleUse()
        {
            Print("Using");
        }

        public static void HandleLook()
        {
            Print("Looking");
        }

        public static void HandleTake()
        {
            Print("Taking");
        }

        public static void HandleDrop()
        {
            Print("Dropping");
        }

        public static void HandleTalk()
        {
            Print("Talking");
        }

        public static void HandleHelp()
        {
            Print("Helping");
        }
    }
}
