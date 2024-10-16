namespace VinylApplication326.Models
{
    public class UserModel
    {
        // declare properties
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Password { get; set; }
        // use ? to declare properties as nullable strings
        // or initialize to string.Empty
    }
}
