using Labb3_API.Models;
using Labb3_API.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labb3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly InterestsDbContext _ctx;

        public InterestController(InterestsDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet(Name = "GetAllInterests")]
        public async Task<ActionResult<GetInterestResponse>> GetAllInterests()
        {
            return Ok(await _ctx.Interests
                .AsNoTracking()
                .Select(GetInterestResponse.FromEntity)
                .ToListAsync());
        }
    }
}
