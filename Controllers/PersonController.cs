using Labb3_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labb3_API.Controllers
{
    [Route("api/people")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly InterestsDbContext _ctx;

        public PersonController(InterestsDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet(Name = "GetAllPeople")]
        public async Task<ActionResult<IEnumerable<Person>>> GetAll()
        {
            return Ok(await _ctx.People
                .AsNoTracking()
                .ToListAsync());
        }
    }
}
