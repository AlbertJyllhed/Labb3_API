using Labb3_API.Models;
using Labb3_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labb3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly InterestsDbContext _ctx;

        public PersonController(InterestsDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet(Name = "GetAllPeople")]
        public async Task<ActionResult<IEnumerable<GetPersonResponseSimple>>> GetAllPeople()
        {
            return Ok(await _ctx.People
                .AsNoTracking()
                .Select(GetPersonResponseSimple.FromEntity)
                .ToListAsync());
        }

        [HttpGet("{id}", Name = "GetPersonById")]
        public async Task<ActionResult<GetPersonResponse>> GetPersonById(
            int id, bool includeInterests = false, bool includeLinks = false)
        {
            var person = await _ctx.People
                .AsNoTracking()
                .Select(p => new GetPersonResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Phone = p.Phone,
                    Email = p.Email,
                    Interests = includeInterests
                        ? p.Interests.Select(i => new GetInterestResponseSimple
                        {
                            Id = i.Id,
                            Title = i.Title,
                            Description = i.Description,
                        }).ToList()
                        : null,
                    Links = includeLinks
                        ? p.Links.Select(l => new GetLinkResponseSimple
                        {
                            Id = l.Id,
                            Url = l.Url,
                        }).ToList()
                        : null,
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person is null)
            {
                return NotFound($"Could not find person with ID: {id}");
            }

            return Ok(person);
        }

        [HttpGet("{id}/interests", Name = "GetPersonInterests")]
        public async Task<ActionResult<ICollection<GetInterestResponseSimple>>> GetPersonInterests(int id)
        {
            var interests = await _ctx.People
                .AsNoTracking()
                .Where(p => p.Id == id)
                .SelectMany(p => p.Interests)
                .Select(GetInterestResponseSimple.FromEntity)
                .ToListAsync();

            if (!interests.Any())
            {
                return NotFound($"Personen med ID: {id} har inget intresse.");
            }

            return Ok(interests);
        }

        [HttpPost("{id}/interests", Name = "AddPersonInterest")]
        public async Task<IActionResult> AddPersonInterest(
            int id, AddInterestRequest request)
        {
            if (!await _ctx.People.AnyAsync(p => p.Id == id))
            {
                return NotFound($"Could not find person with ID: {id}");
            }

            var interest = new Interest
            {
                Title = request.Title,
                Description = request.Description,
                PersonId = id
            };

            await _ctx.Interests.AddAsync(interest);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonById), 
                new { id = interest.Id }, new GetInterestResponseSimple
            {
                Id = interest.Id,
                Title = interest.Title,
                Description = interest.Description,
            });
        }

        [HttpGet("{id}/links", Name = "GetPersonLinks")]
        public async Task<ActionResult<ICollection<GetLinkResponseSimple>>> GetPersonLinks(int id)
        {
            var links = await _ctx.People
                .AsNoTracking()
                .Where(p => p.Id == id)
                .SelectMany(p => p.Links)
                .Select(GetLinkResponseSimple.FromEntity)
                .ToListAsync();

            if (!links.Any())
            {
                return NotFound($"Personen med ID: {id} har inga länkar.");
            }

            return Ok(links);
        }

        [HttpPost("{id}/links", Name = "AddPersonLink")]
        [ProducesResponseType(typeof(GetLinkResponseSimple), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPersonLink(int id, AddLinkRequest request)
        {
            if (!await _ctx.People.AnyAsync(p => p.Id == id))
            {
                return NotFound($"Could not find person with ID: {id}");
            }

            bool couldCreate = Uri.TryCreate(request.Url, UriKind.Absolute, out var uriResult);
            bool urlValid = uriResult != null
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!couldCreate || !urlValid)
            {
                return BadRequest($"Incorrect Url format.");
            }

            var link = new Link
            {
                Url = request.Url,
                PersonId = id,
                InterestId = request.interestId
            };

            await _ctx.Links.AddAsync(link);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonById), 
                new { id = link.Id }, new GetLinkResponseSimple
            {
                Id = link.Id,
                Url = link.Url,
            });
        }
    }
}
