using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using VinylApplication326.Models;

namespace VinylApplication326.DAO
{
    public class LoginDAO
    {
        public string connectionString;
        public LoginDAO() 
        {
            connectionString = @"datasource=localhost;port=3306;username=root;password=root;database=vinylapp;";
        }

        public UserModel AuthenticateUser(UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = "SELECT * FROM `users` WHERE Username = @userName AND Password = @password";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Username", user.UserName);
                command.Parameters.AddWithValue("@Password", user.Password);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        // update the flag to return a found user
                        return new UserModel
                        {
                            Id = (int)reader["Id"],
                            UserName = reader["Username"].ToString(),
                        };
                    }
                    return new UserModel();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new UserModel();
                }

            }
        }

        public bool RegisterNewUser(UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

                MySqlCommand command = new MySqlCommand(query, connection);
                
                    command.Parameters.AddWithValue("@Username", user.UserName);
                    command.Parameters.AddWithValue("@Password", user.Password);
                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    //command.ExecuteNonQuery();
                    return true;
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                
            }
        }
    }
}
