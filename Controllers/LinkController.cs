using Labb3_API.Models;
using Labb3_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labb3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly InterestsDbContext _ctx;

        public LinkController(InterestsDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet(Name = "GetAllLinks")]
        public async Task<ActionResult<GetLinkResponse>> GetAllLinks()
        {
            return Ok(await _ctx.Links
                .AsNoTracking()
                .Select(GetLinkResponse.FromEntity)
                .ToListAsync());
        }

        [HttpGet("{id}", Name = "GetLinkById")]
        public async Task<ActionResult<GetLinkResponse>> GetLinkById(int id)
        {
            var link = await _ctx.Links
                .AsNoTracking()
                .Select(GetLinkResponse.FromEntity)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (link is null)
            {
                return NotFound($"Länken med ID: {id} hittades inte.");
            }

            return Ok(link);
        }

        [HttpPost(Name = "AddLink")]
        [ProducesResponseType(typeof(GetLinkResponseSimple), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddLink(AddLinkRequest request)
        {
            if (!await _ctx.People.AnyAsync(p => p.Id == request.personId) ||
                !await _ctx.Interests.AnyAsync(i => i.Id == request.interestId))
            {
                return BadRequest("Could not find the specified person or interest.");
            }

            var link = new Link
            {
                Url = request.Url,
                PersonId = request.personId,
                InterestId = request.interestId
            };

            await _ctx.Links.AddAsync(link);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLinkById), new { id = link.Id }, new GetLinkResponseSimple
            {
                Id = link.Id,
                Url = link.Url,
            });
        }
    }
}
