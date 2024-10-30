using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using VinylApplication326.Models;

namespace VinylApplication326.DAO
{
    public class LoginDAO
    {
        public string connectionString;
        public LoginDAO() 
        {
            this.connectionString = "Server=127.0.0.1;Database=vinylapp;User ID=root;Password=root;";

        }

        public void AuthenticateUser(UserModel user)
        {

        }

        public void RegisterNewUser(UserModel user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Users (Username, Password) VALUES (@Username, @Password)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.UserName);
                    command.Parameters.AddWithValue("@Password", user.Password);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
