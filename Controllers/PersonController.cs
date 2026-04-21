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

        [HttpPost("{id}/interests", Name = "AddInterestToPerson")]
        public async Task<IActionResult> AddInterestToPerson(
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

            return CreatedAtAction(nameof(GetPersonById), new { id }, new GetInterestResponseSimple
            {
                Id = interest.Id,
                Title = interest.Title,
                Description = interest.Description,
            });
        }
    }
}
