using System.Linq.Expressions;

namespace Labb3_API.Models.DTOs
{
    public record GetInterestResponseSimple
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        public static Expression<Func<Interest, GetInterestResponseSimple>> FromEntity =>
            i => new GetInterestResponseSimple
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
            };
    }

    public record GetInterestResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public GetPersonResponseSimple? Person { get; init; }
        public IReadOnlyList<GetLinkResponseSimple>? Links { get; init; }

        public static Expression<Func<Interest, GetInterestResponse>> FromEntity =>
            i => new GetInterestResponse
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Person = new GetPersonResponseSimple
                {
                    Id = i.Person.Id,
                    Name = i.Person.Name,
                    Phone = i.Person.Phone,
                    Email = i.Person.Email,
                },
                Links = i.Links.Select(l => new GetLinkResponseSimple
                {
                    Id = l.Id,
                    Url = l.Url,
                }).ToList(),
            };
    }

    public record AddInterestRequest(string Title, string Description);
}
