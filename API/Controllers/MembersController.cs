using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // // so this is the route i guess
    // [Route("api/[controller]")] // localhost5001/api/members
    // [ApiController]
    // the above line is commented because the baseApiContoller already has it
    [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        {
            return Ok(await memberRepository.GetMembersAsync());

        }


        [HttpGet("{id}")] // localhost5001/api/members/bob-id
        // here the id inside the qoutes acts as a route parameter
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);
            // return 404 status code or error code when the user with given id is not found
            if (member == null) return NotFound();
            return member;
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
        {
            return Ok(await memberRepository.GetPhotosForMemberAsync(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            Console.WriteLine("Update request recieved *********************************************************************");
            // here the user is from Controler base , it is genereted by asp.net after authorizaation 
            // here the user is claimprincipler create by jwt bearer
            /*
            JwtBearer validates the JWT, then builds a ClaimsPrincipal from the token’s claims.
            app.UseAuthentication() assigns that principal to HttpContext.User for the request.
            Controllers expose it as ControllerBase.User so authorization and your code can read claims uniformly.
            */
            // this GetMemberID returns the nameidentifier from the claimprinciple 
            var memberId = User.GetMemberId();

            var member = await memberRepository.GetMemberForUpdates(memberId);

            if (member == null) return BadRequest("Couldnt find member with id");

            // update the value of the member
            member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdateDto.Description ?? member.Description;
            member.City = memberUpdateDto.City ?? member.City;
            member.Country = memberUpdateDto.Country ?? member.Country;

            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;
            memberRepository.Update(member); // optional 

            if (await memberRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update member");
        }
    }
}
