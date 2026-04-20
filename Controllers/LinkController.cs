using Labb3_API.Models;
using Labb3_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}", Name = "GetLinkById")]
        public async Task<ActionResult<GetLinkResponse>> GetLinkById(int id)
        {
            var link = _ctx.Links
                .AsNoTracking()
                .Select(GetLinkResponse.FromEntity)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (link is null)
            {
                return NotFound($"Länken med ID: {id} hittades inte.");
            }

            return Ok(link);
        }
    }
}
