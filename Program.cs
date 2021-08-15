using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TextAdventure
{
    class Program
    {
        public static bool CheckOS()
        {  
            string checkOS = "System.Runtime.InteropServices.OSPlatform";

            // OSX
            if (checkOS.Contains("OSX");
            {
            return;
            }
       
            // Windows
            else if (checkOS.Contains("Windows");
            {
            return true;
            }

            // Unsupported
            else
            {
            return;
            }

        }

        static void Main(string[] args)
        {   
           

            
            Console.Title = "I found the command to change the title of the console window :D";
            Console.CursorVisible = false;
            Console.Beep();
            

            int height = 50;
            int width = 120;
            Console.WindowHeight = height;
            Console.WindowWidth = width;
            
           

            string title = File.ReadAllText("Title.txt");
            string border = File.ReadAllText("Border.txt");



            Console.WriteLine(border);
            Console.SetCursorPosition(1, 3);
            Console.WriteLine(title);

 
            bool checkSystem = CheckOS();
            

            Console.ReadKey();
            Console.Clear();
            
        }
    }
}
