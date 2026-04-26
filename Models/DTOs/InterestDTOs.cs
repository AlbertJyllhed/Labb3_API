using System.Linq.Expressions;

namespace Labb3_API.Models.DTOs
{
    public record GetInterestResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        public static Expression<Func<Interest, GetInterestResponse>> FromEntity =>
            i => new GetInterestResponse
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
            };
    }

    public record AddInterestRequest(string Title, string Description);
}
