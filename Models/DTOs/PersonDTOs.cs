using System.Linq.Expressions;

namespace Labb3_API.Models.DTOs
{
    public record GetPersonResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Phone { get; init; }
        public string? Email { get; init; }
        public IReadOnlyList<GetInterestResponse>? Interests { get; init; }
        public IReadOnlyList<GetLinkResponse>? Links { get; init; }

        public static Expression<Func<Person, GetPersonResponse>> FromEntity(
            bool includeInterests, bool includeLinks) =>
            p => new GetPersonResponse
            {
                Id = p.Id,
                Name = p.Name,
                Phone = p.Phone,
                Email = p.Email,
                Interests = includeInterests
                    ? p.Interests.Select(i => new GetInterestResponse
                    {
                        Id = i.Id,
                        Title = i.Title,
                        Description = i.Description,
                    }).ToList()
                    : null,
                Links = includeLinks
                    ? p.Links.Select(l => new GetLinkResponse
                    {
                        Id = l.Id,
                        Url = l.Url,
                    }).ToList()
                    : null,
            };
    }
}
