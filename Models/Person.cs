using System.ComponentModel.DataAnnotations;

namespace Labb3_API.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        // Navigation properties
        public ICollection<Interest> Interests { get; set; } = [];
        public ICollection<Link> Links { get; set; } = [];
    }
}
