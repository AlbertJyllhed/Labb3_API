using System.Linq.Expressions;

namespace Labb3_API.Models.DTOs
{
    public record GetLinkResponseSimple
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;

        public static Expression<Func<Link, GetLinkResponseSimple>> FromEntity =>
            l => new GetLinkResponseSimple
            {
                Id = l.Id,
                Url = l.Url,
            };
    }

    public record GetLinkResponse
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public GetPersonResponseSimple? Person { get; set; }
        public GetInterestResponseSimple? Interest { get; set; }

        public static Expression<Func<Link, GetLinkResponse>> FromEntity =>
            l => new GetLinkResponse
            {
                Id = l.Id,
                Url = l.Url,
                Person = new GetPersonResponseSimple
                {
                    Id = l.Person.Id,
                    Name = l.Person.Name,
                    Phone = l.Person.Phone,
                    Email = l.Person.Email,
                },
                Interest = new GetInterestResponseSimple
                {
                    Id = l.Interest.Id,
                    Title = l.Interest.Title,
                    Description = l.Interest.Description,
                },
            };
    }

    public record AddLinkRequest(string Url, int interestId);
}
