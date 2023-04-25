using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter file name: ");
        string searchFileName = Console.ReadLine();

        // Use a helper method to search directories and handle exceptions
        SearchDirectories("C:\\", $"*{searchFileName}*", SearchOption.AllDirectories);

        Console.WriteLine("Search complete.");
        Console.ReadKey();
    }

    static void SearchDirectories(string directory, string searchPattern, SearchOption searchOption)
    {
        try
        {
            string[] files = Directory.GetFiles(directory, searchPattern);

            if (files.Length > 0)
            {
                Console.WriteLine("Found {0} files in {1}:", files.Length, directory);
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                }
            }

            if (searchOption == SearchOption.AllDirectories)
            {
                string[] subdirectories = Directory.GetDirectories(directory);
                int totalSubdirectories = subdirectories.Length;
                int completedSubdirectories = 0;

                foreach (string subdirectory in subdirectories)
                {
                    Console.Write($"Searching directory {subdirectory}... ");

                    try
                    {
                        SearchDirectories(subdirectory, searchPattern, searchOption);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("Access to directory {0} was denied.", subdirectory);
                    }

                    completedSubdirectories++;
                    UpdateProgressBar(completedSubdirectories, totalSubdirectories);
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Access to directory {0} was denied.", directory);
        }
    }

    static void UpdateProgressBar(int completedItems, int totalItems)
    {
        const int ProgressBarWidth = 40;
        int progress = (int)((float)completedItems / totalItems * ProgressBarWidth);

        Console.CursorLeft = 0;
        Console.Write("[");
        for (int i = 0; i < ProgressBarWidth; i++)
        {
            Console.Write(i < progress ? "#" : "-");
        }
        Console.Write("] {0}/{1}", completedItems, totalItems);
    }
}
