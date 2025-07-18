using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // so this is the route i guess
    [Route("api/[controller]")] // localhost5001/api/members
    [ApiController]
    public  class MembersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await context.Users.ToListAsync();
            return members;
        }

        [HttpGet("{id}")] // localhost5001/api/members/bob-id
        // here the id inside the qoutes acts as a route parameter
        public async Task<ActionResult<AppUser>> GetMember(string id)
        {
            var member = await context.Users.FindAsync(id);
            // return 404 status code or error code when the user with given id is not found
            if (member == null) return NotFound();
            return member;
        }
    }
}
