using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
namespace TransferCOMPLETE
{
    #region Data Types
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
        MainDoor,
    }
    enum ThingId
    {
        None,
        All,
        ColdKey,
        WarmKey,
        PieceOfPaper,
        Button,
        Keycard,
        JarOfButter,
        USBstick,

     

    }
    enum Direction
    {
        none,
        north,
        northeast,
        east,
        southeast,
        south,
        southwest,
        west,
        northwest,
    }
    class LocationData
    {
        public LocationId Id;
        public string Name;
        public string Description;
        public Dictionary<Direction, LocationId> Directions;
    }
    class ThingData
    {
        public ThingId Id;
        public string Name;
        public string Description;
        public LocationId StartingLocationId;
    }
    class ParsedData
    {
        public string Id;
        public string Name;
        public string Description;
        public Dictionary<Direction, LocationId> Directions;
        public LocationId StartingLocationId;
    }
    #endregion

    class Program
    {
        #region Fields
        // Data dictionaries
        static Dictionary<LocationId, LocationData> LocationsData = new Dictionary<LocationId, LocationData>();
        static Dictionary<ThingId, ThingData> ThingsData = new Dictionary<ThingId, ThingData>();

        // Thing helpers
        static Dictionary<string, ThingId> ThingIdsByName = new Dictionary<string, ThingId>();
        static ThingId[] ThingsYouCanGet = {ThingId.Keycard};

        // Current state
        static LocationId CurrentLocationId = LocationId.MainHall;
        static Dictionary<ThingId, LocationId> ThingsLocations = new Dictionary<ThingId, LocationId>();

        // Helper Variables and Constants
        static bool quit = false;
        const ConsoleColor narrativeColor = ConsoleColor.Cyan;
        const ConsoleColor promptColor = ConsoleColor.White;
        const int printPauseMilliseconds = 50;
        static string verb;
        static string noun;
        #endregion

        #region Program start
        static void Main()
        {
            // Sets the title of the window, makes it look neat
            Console.Title = "";

            // Makes the cursor invisible, it's nice to not have blinky boi visible all the time
            Console.CursorVisible = false;

            // Sets the color :3
            Console.ForegroundColor = narrativeColor;

            ReadLocationData();
            ReadThingData();
            InitializeThingHelpers();
            InitializeThingsState();

            // Keeping this as a method in order to skip the intro during testing
            //Intro();

            // Display starting location
            DisplayLocation(true);

            // The gameloop
            do
            {
                HandlePlayerInput();
                HandleGameRules();
            } while (!quit);       
        }
        static void Intro()
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
                        signature += pressedKey.KeyChar;
                    }
                } while (pressedKey.Key != ConsoleKey.Enter && (signature.Length < Console.BufferWidth - 10));
            } while (Regex.IsMatch(signature, regexSignature) == false);

            string signatureAccepted = "Signature accepted, press any key to initiate the transfer...";
            Console.SetCursorPosition(Console.BufferWidth / 2 - signatureAccepted.Length / 2, 29);
            Console.WriteLine(signatureAccepted);
            Console.ReadKey();
            Console.Clear();

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
        static void ReadLocationData()
        {
            // Parse the locations file.
            List<ParsedData> parsedDataList = ParseData("Locations.txt");

            // Transfer data from the parsed structures into locations data.
            foreach (ParsedData parsedData in parsedDataList)
            {
                LocationId locationId = Enum.Parse<LocationId>(parsedData.Id);
                LocationData locationData = new LocationData
                {
                    Id = locationId,
                    Name = parsedData.Name,
                    Description = parsedData.Description,
                    Directions = parsedData.Directions,
                };

                LocationsData[locationId] = locationData;
            }
        }
        static void ReadThingData()
        {
            // Parse the things file.
            List<ParsedData> parsedDataList = ParseData("Things.txt");

            // Transfer data from the parsed structures into things data.
            foreach (ParsedData parsedData in parsedDataList)
            {
                ThingId thingId = Enum.Parse<ThingId>(parsedData.Id);
                ThingData thingData = new ThingData
                {
                    Id = thingId,
                    Name = parsedData.Name,
                    Description = parsedData.Description,
                    StartingLocationId = parsedData.StartingLocationId
                };
                ThingsData[thingId] = thingData;
            }
        }
        static List<ParsedData> ParseData(string fileLocation)
        {
            var parsedDataList = new List<ParsedData>();

            // Splits the file to individual entries
            string[] dataEntries = File.ReadAllText(fileLocation).Split("\r\n\r\n");

            // Instead of a foreach I use a for-loop to skip the int that counts lines
            for (int entry = 0; entry < dataEntries.Length; entry++)
            {
                string[] dataEntry = dataEntries[entry].Split("\r\n");
                string id = dataEntry[0];

                // Initialize the structure that will hold parsed data.
                var parsedData = new ParsedData
                {
                    Id = id,
                    Directions = new Dictionary<Direction, LocationId>()
                };

                for (int entryLine = 1; entryLine < dataEntry.Length; entryLine++)
                {
                    string[] currentData = dataEntry[entryLine].Split(":");

                    // Extract property and potentially value.
                    string property = currentData[0];
                    string value;
                    try
                    {
                        value = currentData[1].TrimStart();
                    }
                    catch { value = ""; };

                    // Store value into data structure.
                    switch (property)
                    {
                        case "Name":
                            parsedData.Name = value;
                            break;

                        case "Description":
                            parsedData.Description = value;
                            break;

                        case "Directions":
                            // Directions are listed in separate lines with format "  direction: destination".
                            entryLine++;
                            while (entryLine < dataEntry.Length)
                            {
                                currentData = dataEntry[entryLine].Split(":".TrimStart());
                                currentData[0] = currentData[0].Trim();

                                // Store parsed data into the directions dictionary.
                                Direction direction = Enum.Parse<Direction>(currentData[0].ToLowerInvariant());
                                LocationId destination = Enum.Parse<LocationId>(currentData[1]);
                                parsedData.Directions[direction] = destination;
                                entryLine++;
                            }
                            break;

                        case "StartingLocation":
                            parsedData.StartingLocationId = Enum.Parse<LocationId>(value);
                            break;
                    }
                }
                parsedDataList.Add(parsedData);
            }

            return parsedDataList;
        }
        static void InitializeThingHelpers()
        {
            // Create a map of things by their name.
            foreach (KeyValuePair<ThingId, ThingData> thingEntry in ThingsData)
            {
                string name = thingEntry.Value.Name.ToLowerInvariant();

                // Allow to refer to a thing by any of its words.
                string[] nameParts = name.Split();

                foreach (string namePart in nameParts)
                {
                    // Don't override already assigned words.
                    if (ThingIdsByName.ContainsKey(namePart)) continue;

                    ThingIdsByName[namePart] = thingEntry.Key;
                }
            }
        }
        static void InitializeThingsState()
        {
            // Set all things to their starting locations.
            foreach (KeyValuePair<ThingId, ThingData> thingEntry in ThingsData)
            {
                ThingsLocations[thingEntry.Key] = thingEntry.Value.StartingLocationId;
            }
        }
        #endregion

        #region Output Helpers
        static void Print()
        { 
                Console.WriteLine();
                Thread.Sleep(printPauseMilliseconds);
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
        static string Capitalize(string text)
        {
            return text.Substring(0, 1).ToUpperInvariant() + text.Substring(1);;
        }
        #endregion

        #region Interaction helpers
        static List<ThingId> GetThingIdsFromWords(string[] words)
        {
            List<ThingId> thingIds = new List<ThingId>();

            // For each word, see if it's a name of a thing.
            foreach (string word in words)
            {
                if (ThingIdsByName.ContainsKey(word))
                {
                    thingIds.Add(ThingIdsByName[word]);
                }
            }

            return thingIds;
        }
        static IEnumerable<ThingId> GetThingsAtLocation(LocationId locationId)
        {
            return ThingsLocations.Keys.Where(thingId =>ThingsLocations[thingId] == locationId);
        }
        static string GetName(ThingId thingId)
        {
            return ThingsData[thingId].Name;
        }
        static IEnumerable<string> GetNames(IEnumerable<ThingId> thingIds)
        {
            return thingIds.Select(thingId => ThingsData[thingId].Name);
        }
        #endregion

        #region Interaction
        static void HandlePlayerInput()
        {
            Console.SetCursorPosition(0, 38);
            Print("What will I do?");
            Console.Write(">");
            Console.ForegroundColor = promptColor;
            Console.CursorVisible = true;
            string input = Console.ReadLine().ToLowerInvariant();
            Console.CursorVisible = false;
            Console.ForegroundColor = narrativeColor;
            Console.Clear();
            DisplayLocation(false);
            if (input == "")
                return;

            // Analyse the command by assuming the first word is a verb (or similar instruction).
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
            {
                Reply("Try typing a command, type help for a list of commands.");
                return;
            }

            verb = words[0];
            try
            {
                noun = words[1];
            }
            catch { noun = ""; }

            // Some commands are performed with things so see if any are being mentioned.
            List<ThingId> thingIds = GetThingIdsFromWords(words);

            // Reacts based on the player input
            switch (verb)
            {
                case "move":
                case "go":
                case "head":
                case "walk":
                case "run":
                case "wander":
                case "jog":
                case "sneak":
                case "crawl":
                case "moonwalk":
                    HandleMovement(Direction.none);
                    break;

                case "north":
                case "n":
                    HandleMovement(Direction.north);
                    break;

                case "northeast":
                case "ne":
                    HandleMovement(Direction.northeast);
                    break;

                case "east":
                case "e":
                    HandleMovement(Direction.east);
                    break;

                case "southwest":
                case "sw":
                    HandleMovement(Direction.southwest);
                    break;

                case "south":
                case "s":
                    HandleMovement(Direction.south);
                    break;

                case "southeast":
                case "se":
                    HandleMovement(Direction.southeast);
                    break;

                case "northwest":
                case "nw":
                    HandleMovement(Direction.northwest);
                    break;

                case "west":
                case "w":
                    HandleMovement(Direction.west);
                    break;

                // Looking
                case "look":
                case "watch":
                case "see":
                case "look at":
                case "view":
                    Print("Looking");
                    //  HandleLook();
                    break;

                // Getting
                case "take":
                case "get":
                case "accuire":
                case "steal":
                    HandleGet(words, thingIds);
                    break;

                // Dropping
                case "drop":
                case "dispose":
                case "trash":
                case "throw":
                    HandleDrop(words, thingIds);
                    break;

                // Using
                case "use":
                    Print("Using");
                    //HandleUse();
                    break;

 

                // Helping
                case "help":
                    //HandleHelp();
                    Print("Helping");
                    break;

                // Exiting the game
                case "end":
                case "quit":
                    Reply("See you soon...");
                    quit = true;
                    break;

                default:
                    Console.SetCursorPosition(0, 37);
                    Reply("Hmm...");
                    break;
            }

        }
        static void HandleGameRules()
        {

        }
        static void HandleMovement(Direction direction)
        {
            if (direction == Direction.none && noun == "")
            {
                Print($"Where do you want to {verb}?");
                return;
            }

            else if (direction == Direction.none)
            {
                switch (noun)
                {
                    case "north":
                    case "n":
                        noun = "north";
                        break;

                    case "northeast":
                    case "ne":
                        direction = Direction.northeast;
                        break;

                    case "east":
                    case "e":
                        direction = Direction.east;
                        break;

                    case "southeast":
                    case "se":
                        direction = Direction.southeast;
                        break;

                    case "south":
                    case "s":
                        direction = Direction.south;
                        break;

                    case "southwest":
                    case "sw":
                        direction = Direction.southwest;
                        break;

                    case "west":
                    case "w":
                        direction = Direction.west;
                        break;

                    case "northwest":
                    case "nw":
                        direction = Direction.northwest;
                        break;

                    default:
                        direction = Direction.none;
                        break;
                }
            }

            // See if the current location has the desired direction.
            LocationData currentLocationData = LocationsData[CurrentLocationId];

            if (!currentLocationData.Directions.ContainsKey(direction))
            {
                if (verb == "")
                {
                    verb = "go";
                }
                Reply("I cannot go that way.");
                return;
            }

            else
            {
                // Move the player to that location.
                CurrentLocationId = currentLocationData.Directions[direction];
            }
        }
        static void HandleLook(string[] words, List<ThingId> thingIds)
        {

        }
        static void HandleGet(string[] words, List<ThingId> thingIds)
        {
            if (words.Length == 1)
            {
                Reply($"What do you want to {words[0]}?");
                return;
            }












            foreach (ThingId thingId in thingIds)
            {
                string thingName = Capitalize(GetName(thingId));

                // Make sure the thing can be picked up.
                if (!ThingsYouCanGet.Contains(thingId))
                {
                    Reply($"{thingName} can't be picked up.");
                }

                // Check if you already have the thing.
                if (ThingsLocations[thingId] == LocationId.Inventory)
                {
                    Reply($"{thingName} is already in your possession.");
                }

                // Make sure the thing is at this location.
                if (ThingsLocations[thingId] != CurrentLocationId)
                {
                    Reply($"{thingName} is not here.");
                }

                ThingsLocations[thingId] = LocationId.Inventory;
            }
           
        }
        static void HandleDrop(string[] words, List<ThingId> thingIds)
        {
            // Handle edge cases.
            if (words.Length == 1)
            {
                Reply($"What do you want to {words[0]}?");
                return;
            }

            foreach (ThingId thingId in thingIds)
            {
                string thingName = Capitalize(GetName(thingId));

                // Check if you have the thing.

                if(!thingIds.Contains(thingId))
                {
                    Reply($"I do not know what you want me to drop...");
                }

                if (ThingsLocations[thingId] != LocationId.Inventory)
                {
                    Reply($"I do not have {thingName} in my inventory.");
                }

                else
                {
                    Reply($"I dropped the {thingName}.");
                }

                // Everything seems to be OK, drop the thing.
                ThingsLocations[thingId] = CurrentLocationId;
            }
        }
        #endregion

        #region Display helpers
        static void DisplayLocation(bool showDescription)
        {
            //Console.Clear();

            Console.WriteLine(File.ReadAllText("Artbox.txt"));

            //Display current location description
            LocationData currentLocationData = LocationsData[CurrentLocationId];

            if (showDescription == true)
                Print(currentLocationData.Description);

            else
                Print("I find myself in the " + currentLocationData.Name + ".");

            String directionsDescription;

            if (currentLocationData.Directions.Count > 0)
            {
                if (currentLocationData.Directions.Count == 1)
                {
                    directionsDescription = "There is only 1 way to go:";
                }

                else
                {
                    directionsDescription = ($"There are {currentLocationData.Directions.Count} ways to go:");
                }

                foreach (KeyValuePair<Direction, LocationId> directionEntry in currentLocationData.Directions)
                {
                    string directionName = directionEntry.Key.ToString();

                    try
                    {
                        directionName = Capitalize(directionName);
                    }
                    catch { }

                    directionsDescription += $" {directionName},";
                }

                directionsDescription = directionsDescription.Remove(directionsDescription.Length - 1);

            }

            else
            {
                directionsDescription = "There is nowhere to go.";
            }

            Print(directionsDescription);


            // Display things that are at the current location.
            Print("I see:");

            IEnumerable<ThingId> thingsAtCurrentLocation = GetThingsAtLocation(CurrentLocationId);

            if (thingsAtCurrentLocation.Count() == 0)
            {
                Print("    " + Capitalize("nothing of intrest"));
            }
            else
            {
                foreach (ThingId thingId in thingsAtCurrentLocation)
                {
                    Print($"    {Capitalize(GetName(thingId))}");
                }
            }

            Print();
        }

        static void DisplayThing(ThingId thing)
        {

        }

        static void DisplayInventory(List<ThingId> thingIds)
        {

            foreach (ThingId thingId in thingIds)
            {
                string thingName = Capitalize(GetName(thingId));

                // Make sure the thing can be picked up.
                if (!ThingsYouCanGet.Contains(thingId))
                {
                    Reply($"{thingName} can't be picked up.");
                }

                // Check if you already have the thing.
                if (ThingsLocations[thingId] == LocationId.Inventory)
                {
                    Reply($"{thingName} is already in your possession.");
                }

                // Make sure the thing is at this location.
                if (ThingsLocations[thingId] != CurrentLocationId)
                {
                    Reply($"{thingName} is not here.");
                }

                // Everything seems to be OK, take the thing.
                ThingsLocations[thingId] = LocationId.Inventory;
            }
        }
        #endregion

        #region Event helpers
        static bool ThingAt(ThingId thingId, LocationId locationId)
        {
            if (!ThingsLocations.ContainsKey(thingId)) 
            {
                return false;
            }

            return ThingsLocations[thingId] == locationId;
        }

        static bool ThingIsHere(ThingId thingId)
        {
            return ThingAt(thingId, CurrentLocationId);
        }

        static bool ThingAvailable(ThingId thingId)
        {
            return ThingIsHere(thingId) || HaveThing(thingId);
        }

        // Checks if the chosen thing is in inventory
        static bool HaveThing(ThingId thing)
        {
            if(ThingsLocations[thing] == LocationId.Inventory)
            {
                return true;
            }

            else
            {
                return false;
            } 
        }

        // Moves chosen thing
        static void MoveThing(ThingId thing, LocationId locationId)
        {
            ThingsLocations[thing] = locationId;
        }

        // Swaps the items chosen
        static void SwapThings(ThingId thing1, ThingId thing2)
        {
            ThingId thingMemory = thing1;
            thing1 = thing2;
            thing2 = thingMemory;
        }

        // Gets chosen thing and puts it in inventory
        static bool GetThing(ThingId thing)
        {
            MoveThing(thing, LocationId.Inventory);
            return true;
        }

        // Drops chosen thing at current location
        static bool DropThing(ThingId thing)
        {
            MoveThing(thing, CurrentLocationId);
            return true;
        }
        #endregion
    }
}   