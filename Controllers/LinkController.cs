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

            return CreatedAtRoute("AddLink", new { id = link.Id }, new GetLinkResponseSimple
            {
                Id = link.Id,
                Url = link.Url,
            });
        }
    }
}
