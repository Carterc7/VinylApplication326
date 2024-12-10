using Microsoft.CodeAnalysis.Elfie.Diagnostics;
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
            records = GetRecords(); // Load all records from the database
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
                                Video = reader["Video"] as string,// Safely handle null values
                                Favorite = reader.GetBoolean("Favorite"), // Assuming Favorite is stored as an integer (1 or 0)
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
            //return records;
        }

        public void FavoriteToggle(int recordId)
        {
            if (records == null)
            {
                records = GetRecords(); // Load records if not already loaded
            }

            foreach (RecordModel record in records)
            {
                if (record.Id == recordId)
                {
                    // Toggle the favorite status in memory
                    record.Favorite = !record.Favorite;
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

        public bool createRecord(RecordModel record)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                
                string query = "INSERT INTO records(Name, Image, Video, Favorite, users_Id) VALUES (@Name, @Image, @Video, @Favorite, @userId)";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", record.Name);
                command.Parameters.AddWithValue("@Image", record.Image);
                command.Parameters.AddWithValue("@Video", record.Video);
                command.Parameters.AddWithValue("@Favorite", record.Favorite);
                command.Parameters.AddWithValue("@UserId", record.UsersId);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    //command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
            
        }

        public bool deleteRecord(int recordId, int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {

                string query = "DELETE FROM records WHERE Id = @RecordId && users_Id = @UserId";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@RecordId", recordId);
                command.Parameters.AddWithValue("@UserId", userId);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    //command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
        }

        public RecordModel getRecordByIdAndUserId(int recordId, int userId)
        {
            // Define the query with parameter placeholders
            string query = @"
        SELECT Id, Name, Image, Video, Favorite, users_Id 
        FROM records 
        WHERE Id = @RecordId AND users_Id = @UserId";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@RecordId", recordId);
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Map data from the database to the RecordModel
                                return new RecordModel
                                {
                                    Id = reader.GetInt32("Id"),
                                    Name = reader["Name"].ToString(),
                                    Image = reader["Image"] as string, // Safely handle null values
                                    Video = reader["Video"] as string, // Safely handle null values
                                    Favorite = reader.GetBoolean("Favorite"),
                                    UsersId = reader.GetInt32("users_Id")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error fetching record: {ex.Message}");
            }

            // Return a default value if no record is found
            return null;
        }

        public bool doEdit(RecordModel model)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connString))
                {
                    connection.Open();

                    string query = @"
                UPDATE records
                SET 
                    Name = @Name,
                    Image = @Image,
                    Video = @Video,
                    Favorite = @Favorite
                WHERE Id = @Id AND 
                    users_Id = @UsersId";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Image", model.Image);
                        command.Parameters.AddWithValue("@Video", model.Video);
                        command.Parameters.AddWithValue("@Favorite", model.Favorite);
                        command.Parameters.AddWithValue("@UsersId", model.UsersId);
                        command.Parameters.AddWithValue("@Id", model.Id);

                        // Execute the command and check if a row was updated
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error updating record: {ex.Message}");
                return false;
            }
        }

        public List<RecordModel> GetFavoriteRecords()
        {
            List<RecordModel> searchResults = new List<RecordModel>();

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();
                string query = "SELECT Id, Name, Image, Video, Favorite, users_Id FROM records WHERE Favorite = 1";

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
                                Video = reader["Video"] as string,// Safely handle null values
                                Favorite = reader.GetBoolean("Favorite"), // Assuming Favorite is stored as an integer (1 or 0)
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
    }
}