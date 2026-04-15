using System.ComponentModel.DataAnnotations;

namespace Labb3_API.Models
{
    public class Link
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int InterestId { get; set; }

        [Url]
        [Required]
        public string Url { get; set; } = string.Empty;

        // Navigation properties
        public Person Person { get; set; } = null!;
        public Interest Interest { get; set; } = null!;
    }
}
