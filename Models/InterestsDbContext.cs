using Microsoft.EntityFrameworkCore;

namespace Labb3_API.Models
{
    public class InterestsDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Link> Links { get; set; }

        public InterestsDbContext(DbContextOptions<InterestsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>()
                .HasOne(l => l.Person)
                .WithMany(p => p.Links)
                .HasForeignKey(l => l.PersonId)
                .OnDelete(DeleteBehavior.NoAction);

            // Persons
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "Alice Svensson", Phone = "+46701234567", Email = "alice@example.com" },
                new Person { Id = 2, Name = "Bob Karlsson", Phone = "+46709876543", Email = "bob@example.com" },
                new Person { Id = 3, Name = "Clara Nilsson", Phone = "+46701112233", Email = "clara@example.com" }
            );

            // Interests
            modelBuilder.Entity<Interest>().HasData(
                new Interest { Id = 1, Title = "Photography", Description = "Capturing landscapes and portraits.", PersonId = 1 },
                new Interest { Id = 2, Title = "Hiking", Description = "Exploring trails and nature.", PersonId = 1 },
                new Interest { Id = 3, Title = "Game Dev", Description = "Building indie games with Unity.", PersonId = 2 },
                new Interest { Id = 4, Title = "Open Source", Description = "Contributing to OSS projects.", PersonId = 2 },
                new Interest { Id = 5, Title = "Baking", Description = "Bread, pastries, and sourdough.", PersonId = 3 }
            );

            // Links
            modelBuilder.Entity<Link>().HasData(
                new Link { Id = 1, PersonId = 1, InterestId = 1, Url = "https://www.flickr.com" },
                new Link { Id = 2, PersonId = 1, InterestId = 1, Url = "https://www.500px.com" },
                new Link { Id = 3, PersonId = 1, InterestId = 2, Url = "https://www.alltrails.com" },
                new Link { Id = 4, PersonId = 2, InterestId = 3, Url = "https://www.unity.com" },
                new Link { Id = 5, PersonId = 2, InterestId = 3, Url = "https://itch.io" },
                new Link { Id = 6, PersonId = 2, InterestId = 4, Url = "https://github.com" },
                new Link { Id = 7, PersonId = 3, InterestId = 5, Url = "https://www.kingarthurbaking.com" },
                new Link { Id = 8, PersonId = 3, InterestId = 5, Url = "https://www.theperfectloaf.com" }
            );
        }
    }
}
