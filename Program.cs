using System;
using System.Collections.Generic;

namespace ADO.Net5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-GE7UVHJ\\SQLEXPRESS;Initial Catalog=MusicDB1;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            MusicDB db = new MusicDB(connectionString);

            bool continueRunning = true;

            while (continueRunning)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Create a new playlist");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewPlaylist(db);
                        break;
                    case "0":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1 or 0.");
                        break;
                }
            }
        }

        static void AddNewPlaylist(MusicDB db)
        {
            Console.WriteLine("\nEnter details for the new playlist:");

            Console.Write("Playlist Name: ");
            string playlistName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(playlistName))
            {
                Console.WriteLine("Playlist name cannot be empty.");
                return;
            }

            Console.Write("Category: ");
            string category = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(category))
            {
                Console.WriteLine("Category cannot be empty.");
                return;
            }

            Console.WriteLine("Enter track Ids separated by commas (e.g., 1,2,3): ");
            List<int> trackIds = new List<int>();
            string[] trackIdsStr = Console.ReadLine().Split(',');
            foreach (string idStr in trackIdsStr)
            {
                if (int.TryParse(idStr.Trim(), out int trackId))
                {
                    trackIds.Add(trackId);
                }
                else
                {
                    Console.WriteLine($"Invalid track Id: {idStr}");
                }
            }

            try
            {
                db.CreatePlaylist(playlistName, category, trackIds);
                Console.WriteLine("Playlist created with tracks.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating playlist: " + ex.Message);
            }
        }
    }
}
