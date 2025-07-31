using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Extensions;

namespace API.Controllers;

public class AccountController(AppDbContext context , ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        // here we use await so that the EmailExists method returns the boolean within the task rather than the task istself.
        // which makes sense as the method returns a task
        if (await EmailExists(registerDto.Email))
        {
            return BadRequest("Email Taken");
        }

        Console.WriteLine("Debug ResisterDto : " + registerDto);
        // the using key word helps to dispose the instance of hmacsha512 object after using it in the scope
        using var hmac = new HMACSHA512(); // this is to get the random salt
        // the salr would be the key property of hmac

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        // this line adds the new user to the database , 
        // we are referencing context here which is our DbContext at this point
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // here we dont have to pass in use as parameter because we are already using it on user which is an object of AppUse
        // and also in the method definition the parameter of appuser has this keyword
        return user.ToDto(tokenService);

        // using dto - Data transfer object , to transfer data between two layers
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      // confirming email   
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email);
   
        if (user == null) return Unauthorized("Invalid email address");

        // after getting the user with the same email address
        // we use the salt from the user reference above
        // here the using key word allows the variable only to available in this code block ,
        // once the method is done it is disposed
        // the hash generator is instantiated here
        using var hmac = new HMACSHA512(user.PasswordSalt);

        // the passord from the dto is hashed using the hasher created above
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        // confirming password 
        // here we verify each character of the hashed pass from user and from the dto  
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        // then we return the userdto if valid
        return user.ToDto(tokenService);
    }


    /// <summary>
    /// helper method to check if an email exists
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
}
