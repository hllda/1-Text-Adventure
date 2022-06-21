using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

/*
Coding Conventions
- camelCasing for everything
- curly brackets on their own line
*/

namespace TransferCOMPLETE
{
    // Names of all locations
    enum LocationId
    {
        Void,
        Inventory,
        MainHall,
        EntranceHall,
        MessHall,
        SecurityRoom,
        Bathroom,
        SouthLab,
        WestLab,
        StorageRoom,
        Freezer,
        EastLab,
        Heater,
        Greenhouse,
        Patio,
        NorthLab,
        VatHall,
        Vat,
    }

    // Names of all things, items and objects
    enum ThingId
    {

    }

    // Possible directions
    enum Direction
    {
        North,
        South,
        West,
        East,
        NorthWest,
        NorthEast,
        SouthWest,
        SouthEast,
    }

    // Goals to finish the game
    enum Goal
    {

    }

    // Data about locations
    class LocationData
    {
        public LocationId Id;
        public string Name;
        public string Description;
        public Dictionary<Direction, LocationId> Directions;
        public string Art;
    }

    // Data about things
    class ThingData
    {
        public ThingId Id;
        public string Name;
        public string Description;
        public LocationId StartingLocationId;
        public string Art;
    }



    class Program
    {
        // Lets the game know when to quit
        static bool quit = false;

        // Data dictionaries
        static Dictionary<LocationId, LocationData> LocationsData = new Dictionary<LocationId, LocationData>();
        static Dictionary<ThingId, ThingData> ThingsData = new Dictionary<ThingId, ThingData>();

        // Current location
        static LocationId CurrentLocationId = LocationId.Vat;

        
        static string verb;
        static string noun;
        

        const ConsoleColor narrativeColor = ConsoleColor.Cyan;
        const ConsoleColor promptColor = ConsoleColor.White;
        const int printPauseMilliseconds = 50;




        LocationId location = Enum.Parse<LocationId>(File.ReadAllText("Locations.txt"));
        // LocationId location = Enum.Parse<LocationId>(locationIdText);
        // LocationId[] locations = 

        static string signature = "Basic Name";

        //static Dictionary<LocationId, LocationData> LocationsData = new Dictionary<LocationId, LocationData>();
        //static Dictionary<ThingId, ThingData> ThingsData = new Dictionary<ThingId, ThingData>();

        static void Main()
        {
            // Makes sure stuff is working as it should
            Initialize();

            ReadLocations();
            ReadThings();

            // Intro sequence
            //signature = Intro();

            // Shows the titlescreen
            //TitleScreen();

            Console.ForegroundColor = narrativeColor;
            Console.Write(File.ReadAllText("Border.txt"));

            // Gameloop
            do
            {
                HandlePlayerAction();
                HandleGameRules();
            } while (!quit);
        }

        static void Initialize()
        {
            // Sets the title of the window
            Console.Title = "";

            // Makes the cursor invisible
            Console.CursorVisible = false;
            
            int width = 100;
            int height = 40;

            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            Console.BufferHeight = height;

            // Console.BufferWidth = Console.WindowWidth = width;
            // Console.BufferHeight = Console.WindowHeight = height;


        }

        static string Intro()
        {
            string signature = "";
            string regexSignature = @"\S+";
            do
            {
                Console.Clear();
                Console.Write(File.ReadAllText("TermsOfService.txt"));
                Console.SetCursorPosition(5, 25);
                Console.CursorVisible = true;

                ConsoleKeyInfo pressedKey;

                Thread.Sleep(100);

                Console.CursorVisible = false;
                do
                {
                    pressedKey = Console.ReadKey();
                    if (pressedKey.Key == ConsoleKey.Backspace)
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }

                    else
                    {
                        signature = signature + pressedKey.KeyChar;
                    }
                } while (pressedKey.Key != ConsoleKey.Enter && (signature.Length < Console.BufferWidth - 10));
            } while (Regex.IsMatch(signature, regexSignature) == false);

            string signatureAccepted = "Signature accepted, press any key to start 'The Transfer'...";
            Console.SetCursorPosition(Console.BufferWidth / 2 - signatureAccepted.Length / 2, 29);
            Console.WriteLine(signatureAccepted);
            Console.ReadKey();
            Console.Clear();

            return signature;
        }

        static void TitleScreen()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = narrativeColor;
            Console.SetCursorPosition(0, 0);

            // Reads the file containing the title art
            Console.Write(File.ReadAllText("Title0.txt"));
            int progressBarLeft = 0;

            // A small animated titlescreen
            for (int i = 0; i < 100; ++i)
            {
                if (i % 2 == 0 && i != 0)
                {
                    progressBarLeft += 1;
                    Console.SetCursorPosition(24 + progressBarLeft, 23);
                    Console.Write("▓");
                }

                if (i < 10)
                {
                Console.SetCursorPosition(50, 25);
                    Console.Write(i + "%");
                    Thread.Sleep(20);
                }

                else if (i == 99)
                {
                    Console.SetCursorPosition(49, 25);
                    Console.Write(i + "%");
                    Thread.Sleep(1500);
                }

                else
                {
                    Console.SetCursorPosition(49, 25);
                    Console.Write(i + "%");
                    Thread.Sleep(20);
                }
            }


            // Displays the final title screen
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write(File.ReadAllText("Title100.txt"));
            Console.ReadKey();
            
            Console.Clear();
        }

        static void HandlePlayerAction()
        {
            Console.SetCursorPosition(3, 28);
            Print("What will I do?");
            Print("");
            Console.SetCursorPosition(3, 29);
            Console.Write(">");

            Console.CursorVisible = true;
            string input = Console.ReadLine().ToLowerInvariant();
            Console.CursorVisible = false;
            Console.Clear();

            // Prevents input from being empty, aka idiot proofing for my own sake
            if (input == "")
            {
                return;
            }

            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            verb = words[0];


            // Reacts based on the player input
            switch (verb)
            {
                case "move":
                case "go":
                case "head":
                case "walk":
                    HandleMove("d");
                    break;

                case "north":
                case "n":
                    HandleMove("n");
                    break;

                case "south":
                case "s":
                    HandleMove("s");
                    break;

                case "west":
                case "w":
                    HandleMove("w");
                    break;

                case "east":
                case "e":
                    HandleMove("e");
                    break;

                case "northwest":
                case "nw":

                    HandleMove("nw");
                    break;

                case "northeast":
                case "ne":
                    HandleMove("ne");
                    break;

                case "southwest":
                case "sw":
                    HandleMove("sw");
                    break;

                case "southeast":
                case "se":
                    HandleMove("se");
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
                case "steal":
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
                case "throw":
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

            Console.SetCursorPosition(0, 0);
            Console.Write(File.ReadAllText("Border.txt"));
        }

        static void HandleGameRules()
        {

        }

        static void Print(string text)
        {
            // Split text into lines that don't exceed the window width.
            int maximumLineLength = Console.WindowWidth - 1;
            MatchCollection lineMatches = Regex.Matches(text, @"(.{1," + maximumLineLength + @"})(?:\s|$)");

            // Output each line with a small delay.
            foreach (Match match in lineMatches)
            {
                Console.WriteLine(match.Groups[0].Value);
                Thread.Sleep(printPauseMilliseconds);
            }
        }

        static void Reply(string text)
        {
            Print(text);
            Print("");
        }

        static void DisplayLocation()
        {
            Console.Clear();

            // Display current location description.
            //LocationData currentLocationData = LocationsData[CurrentLocationId];
           // Print(currentLocationData.Description);
        }

        static void HandleMove(string direction)
        {
            if (direction == "d")
            {
                Print("Move where?");
            }

            else
            {
                Print("Moving " + direction);
            }

            switch (noun)
            {
                case "help":
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

        static void HandleUse()
        {
            Print("Using");
        }

        static void HandleLook()
        {
            Print("Looking");
        }

        static void HandleTake()
        {
            Print("Taking");
        }

        static void HandleDrop()
        {
            Print("Dropping");
        }

        static void HandleTalk()
        {
            Print("Talking");
        }

        static void HandleHelp()
        {
            Print("Helping");
        }

        static void ReadLocations()
        {
            string locations = File.ReadAllText("Locations.txt");
            String[] location = locations.Split("\r\n\r\n");

            for (int i = 0; i < location.Length; i++)
            {
                String[] locationData = location[i].Split("\r\n");

                for (int j = 0; j < locationData.Length; j++)
                {
                    String[] locationDatas = locationData[j].Split(": ");

                    string property = locationDatas[0];

                    string value;
                    try
                    {
                        value = locationDatas[1];
                    } 
                    catch 
                    {value = "";
                    }

                    switch (property)
                    {
                        case "Name":
                           // LocationData id = Enum.Parse<LocationId>(value);
                            break;

                        case "Description":

                            break;

                        case "Directions":

                            break;

                        case "StartingLocation":

                            break;

                        case "Art":

                            break;
                    }
                }
            }
            //LocationId location = Enum.Parse<LocationId>(locationIdText);
        }

        static void ReadThings()
        {


        }
    }
}