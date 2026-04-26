using System.Linq.Expressions;

namespace Labb3_API.Models.DTOs
{
    public record GetLinkResponse
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;

        public static Expression<Func<Link, GetLinkResponse>> FromEntity =>
            l => new GetLinkResponse
            {
                Id = l.Id,
                Url = l.Url,
            };
    }

    public record AddLinkRequest(string Url, int interestId);
}
