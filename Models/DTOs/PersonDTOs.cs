using System.Linq.Expressions;

namespace Labb3_API.Models.DTOs
{
    public record GetPersonResponseSimple
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Phone { get; init; }
        public string? Email { get; init; }

        public static Expression<Func<Person, GetPersonResponseSimple>> FromEntity =>
            p => new GetPersonResponseSimple
            {
                Id = p.Id,
                Name = p.Name,
                Phone = p.Phone,
                Email = p.Email,
            };
    }

    public record GetPersonResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Phone { get; init; }
        public string? Email { get; init; }
        public IReadOnlyList<GetInterestResponseSimple>? Interests { get; init; }
        public IReadOnlyList<GetLinkResponseSimple>? Links { get; init; }

        public static Expression<Func<Person, GetPersonResponse>> FromEntity =>
            p => new GetPersonResponse
            {
                Id = p.Id,
                Name = p.Name,
                Phone = p.Phone,
                Email = p.Email,
                Interests = p.Interests.Select(i => new GetInterestResponseSimple
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                }).ToList(),
                Links = p.Links.Select(l => new GetLinkResponseSimple
                {
                    Id = l.Id,
                    Url = l.Url,
                }).ToList()
            };
    }
}
