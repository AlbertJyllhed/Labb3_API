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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get All People")]
        public async Task<ActionResult<IEnumerable<GetPersonResponse>>> GetAllPeople(
            bool includeInterests = false, bool includeLinks = false)
        {
            return Ok(await _ctx.People
                .AsNoTracking()
                .Select(GetPersonResponse.FromEntity(includeInterests, includeLinks))
                .ToListAsync());
        }

        [HttpGet("{id}", Name = "GetPersonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get Person by ID")]
        public async Task<ActionResult<GetPersonResponse>> GetPersonById(
            int id, bool includeInterests = false, bool includeLinks = false)
        {
            var person = await _ctx.People
                .AsNoTracking()
                .Select(GetPersonResponse.FromEntity(includeInterests, includeLinks))
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person is null)
            {
                return NotFound($"Could not find person with ID: {id}");
            }

            return Ok(person);
        }

        [HttpGet("{id}/interests", Name = "GetPersonInterests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get Person Interests")]
        public async Task<ActionResult<ICollection<GetInterestResponse>>> GetPersonInterests(int id)
        {
            var interests = await _ctx.People
                .AsNoTracking()
                .Where(p => p.Id == id)
                .SelectMany(p => p.Interests)
                .Select(GetInterestResponse.FromEntity)
                .ToListAsync();

            if (!interests.Any())
            {
                return NotFound($"Personen med ID: {id} har inget intresse.");
            }

            return Ok(interests);
        }

        [HttpPost("{id}/interests", Name = "AddPersonInterest")]
        [ProducesResponseType(typeof(GetInterestResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Add Person Interest")]
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
                new { id = interest.Id }, new GetInterestResponse
            {
                Id = interest.Id,
                Title = interest.Title,
                Description = interest.Description,
            });
        }

        [HttpGet("{id}/links", Name = "GetPersonLinks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get Person Links")]
        public async Task<ActionResult<ICollection<GetLinkResponse>>> GetPersonLinks(int id)
        {
            var links = await _ctx.People
                .AsNoTracking()
                .Where(p => p.Id == id)
                .SelectMany(p => p.Links)
                .Select(GetLinkResponse.FromEntity)
                .ToListAsync();

            if (!links.Any())
            {
                return NotFound($"Personen med ID: {id} har inga länkar.");
            }

            return Ok(links);
        }

        [HttpPost("{id}/links", Name = "AddPersonLink")]
        [ProducesResponseType(typeof(GetLinkResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Add Person Link")]
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
                new { id = link.Id }, new GetLinkResponse
            {
                Id = link.Id,
                Url = link.Url,
            });
        }
    }
}
