namespace VinylApplication326.Models
{
    public class RecordModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Video { get; set; }
        public bool Favorite { get; set; }
        public int UsersId { get; set; }
    }
}
