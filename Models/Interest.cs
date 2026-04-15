using System.ComponentModel.DataAnnotations;

namespace Labb3_API.Models
{
    public class Interest
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PersonId { get; set; }

        // Navigation properties
        public Person Person { get; set; } = null!;
        public ICollection<Link> Links { get; set; } = [];
    }
}
