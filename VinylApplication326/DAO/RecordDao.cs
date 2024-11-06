using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using VinylApplication326.Models;

namespace VinylApplication326.DAO
{
    public class RecordDao
    {
        private List<RecordModel> records;
        private string connString = @"Server=127.0.0.1;port=3306;uid=root;password=root;database=vinylapp";

        public RecordDao()
        {
            records = new List<RecordModel>
            {
                new RecordModel { Id = 1, Name = "Revolver By The Beatles", Image = "https://upload.wikimedia.org/wikipedia/en/e/ec/Revolver_%28album_cover%29.jpg", Video = "video1.mp4", Favorite = 1, UsersId = 101 },
                new RecordModel { Id = 2, Name = "Buring Desire", Image = "https://upload.wikimedia.org/wikipedia/en/8/8b/Mike_-_Burning_Desire.png", Video = "video2.mp4", Favorite = 0, UsersId = 102 },
                new RecordModel { Id = 3, Name = "Dots and Loops", Image = "https://upload.wikimedia.org/wikipedia/en/7/7b/Stereolabdotsandloops.png", Video = "video3.mp4", Favorite = 1, UsersId = 103 },
                new RecordModel { Id = 4, Name = "Heaven Or Las Vegas", Image = "https://upload.wikimedia.org/wikipedia/en/6/60/Cocteau_Twins—Heaven_or_Las_Vegas.jpg", Video = "video4.mp4", Favorite = 0, UsersId = 104 },
                new RecordModel { Id = 5, Name = "Record 5", Image = "image5.jpg", Video = "video5.mp4", Favorite = 1, UsersId = 105 },
                new RecordModel { Id = 6, Name = "Record 6", Image = "image6.jpg", Video = "video6.mp4", Favorite = 0, UsersId = 106 },
                new RecordModel { Id = 7, Name = "Record 7", Image = "image7.jpg", Video = "video7.mp4", Favorite = 1, UsersId = 107 },
                new RecordModel { Id = 8, Name = "Record 8", Image = "image8.jpg", Video = "video8.mp4", Favorite = 0, UsersId = 108 },
                new RecordModel { Id = 9, Name = "Record 9", Image = "image9.jpg", Video = "video9.mp4", Favorite = 1, UsersId = 109 },
                new RecordModel { Id = 10, Name = "Record 10", Image = "image10.jpg", Video = "video10.mp4", Favorite = 0, UsersId = 110 }
            };
        }

        public List<RecordModel> GetRecords()
        {
            List<RecordModel> searchResults = new List<RecordModel>();

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Image, Video, Favorite, users_Id FROM records";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            // Map data from the database to the RecordModel
                            RecordModel record = new RecordModel
                            {
                                Id = reader.GetInt32("Id"), // Ensures we get the integer value directly
                                Name = reader["Name"].ToString(),
                                Image = reader["Image"] as string, // Safely handle null values
                                Video = reader["Video"] as string, // Safely handle null values
                                Favorite = reader.GetInt32("Favorite"), // Assuming Favorite is stored as an integer (1 or 0)
                                UsersId = reader.GetInt32("users_Id") // Ensure UsersId matches the field in your table
                            };

                            // Add the record to the results list
                            searchResults.Add(record);
                        }
                        catch (Exception ex)
                        {
                            // Log any issues encountered while reading a record
                            Console.WriteLine($"Error reading record: {ex.Message}");
                        }
                    }
                }
            }
            return searchResults;
        }

        public void FavoriteToggle(int recordId)
        {
            foreach (RecordModel record in this.records)
            {
                if (record.Id == recordId)
                {
                    record.Favorite = record.Favorite == 0 ? 1 : 0;
                    break;
                }
            }

            // Update the favorite status in the database
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();
                string query = "UPDATE records SET Favorite = CASE WHEN Favorite = 1 THEN 0 ELSE 1 END WHERE Id = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", recordId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}