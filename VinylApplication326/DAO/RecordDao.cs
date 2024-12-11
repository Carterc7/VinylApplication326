using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using VinylApplication326.Models;

namespace VinylApplication326.DAO
{
    public class RecordDao
    {
        private string connString = @"Server=127.0.0.1;port=3306;uid=root;password=root;database=vinylapp";

        public List<RecordModel> GetRecords()
        {
            List<RecordModel> records = new List<RecordModel>();

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
                            records.Add(new RecordModel
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader["Name"].ToString(),
                                Image = reader["Image"] as string,
                                Video = reader["Video"] as string,
                                Favorite = reader.GetBoolean("Favorite"),
                                UsersId = reader.GetInt32("users_Id")
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error reading record: {ex.Message}");
                        }
                    }
                }
            }
            return records;
        }

        public void FavoriteToggle(int recordId)
        {
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
                string query = "INSERT INTO records (Name, Image, Video, Favorite, users_Id) VALUES (@Name, @Image, @Video, @Favorite, @UserId)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", record.Name);
                    command.Parameters.AddWithValue("@Image", record.Image ?? string.Empty);
                    command.Parameters.AddWithValue("@Video", record.Video ?? string.Empty);
                    command.Parameters.AddWithValue("@Favorite", record.Favorite);
                    command.Parameters.AddWithValue("@UserId", record.UsersId);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
        }

        public bool deleteRecord(int recordId, int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = "DELETE FROM records WHERE Id = @RecordId AND users_Id = @UserId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RecordId", recordId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return false;
                    }
                }
            }
        }

        public RecordModel getRecordByIdAndUserId(int recordId, int userId)
        {
            string query = @"
                SELECT Id, Name, Image, Video, Favorite, users_Id 
                FROM records 
                WHERE Id = @RecordId AND users_Id = @UserId";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RecordId", recordId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RecordModel
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader["Name"].ToString(),
                                Image = reader["Image"] as string,
                                Video = reader["Video"] as string,
                                Favorite = reader.GetBoolean("Favorite"),
                                UsersId = reader.GetInt32("users_Id")
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool doEdit(RecordModel model)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = @"
            UPDATE records
            SET 
                Name = @Name,
                Image = @Image,
                Video = @Video,
                Favorite = @Favorite
            WHERE Id = @Id AND users_Id = @UsersId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    Console.WriteLine($"UsersId in DAO: {model.UsersId}");
                    command.Parameters.AddWithValue("@Name", model.Name);
                    command.Parameters.AddWithValue("@Image", model.Image ?? string.Empty);
                    command.Parameters.AddWithValue("@Video", model.Video ?? string.Empty);
                    command.Parameters.AddWithValue("@Favorite", model.Favorite);
                    command.Parameters.AddWithValue("@UsersId", model.UsersId);
                    command.Parameters.AddWithValue("@Id", model.Id);

                    try
                    {
                        connection.Open();

                        // Debug SQL parameters
                        Console.WriteLine($"SQL Query: {query}");
                        Console.WriteLine($"@Name={model.Name}, @Image={model.Image}, @Video={model.Video}, @Favorite={model.Favorite}, @UsersId={model.UsersId}, @Id={model.Id}");

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Rows affected: {rowsAffected}");
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating record: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        public List<RecordModel> GetFavoriteRecords()
        {
            List<RecordModel> records = new List<RecordModel>();

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
                            records.Add(new RecordModel
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader["Name"].ToString(),
                                Image = reader["Image"] as string,
                                Video = reader["Video"] as string,
                                Favorite = reader.GetBoolean("Favorite"),
                                UsersId = reader.GetInt32("users_Id")
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error reading record: {ex.Message}");
                        }
                    }
                }
            }
            return records;
        }
    }
}

