using System.ComponentModel.DataAnnotations;

namespace VinylApplication326.Models
{
    public class UserModel
    {
        // declare properties
        //Constructor
        public UserModel()
        {
            UserName = string.Empty; 
            Password = string.Empty; 
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; } // = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
        // use ? to declare properties as nullable strings
        // or initialize to string.Empty
    }
}
