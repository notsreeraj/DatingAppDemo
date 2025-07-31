using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        /*
            jwt token model
            header
                token type 
                info on algo used to sign for eg "hs512"
              +
            Payload
                Claims including the token expiration time

              +
            signature  
        */

     // to get the token key from the config file
        //  that would be from "C:\Users\sreer\Demo\DatingApp\API\appsettings.Development.json"
        // Get secret key from config
        var tokenkey = config["Tokenkey"] ?? throw new Exception("Cannot Get Token Key");
        if (tokenkey.Length < 64) throw new Exception("Your token Key Needs to be >= 64 characters");

        #region 1. Prepare Signing Key (for Header & Signature)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        #endregion

        #region 2. Create Claims (for Payload)
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        #endregion

        #region 3. Build Token Descriptor (Header + Payload + Signature)
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),    // → Payload: claims data
            Expires = DateTime.UtcNow.AddDays(7),    // → Payload: expiration
            SigningCredentials = creds               // → Header: algorithm + Signature: key
        };
        #endregion

        #region 4. Generate Final JWT Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
        #endregion
    }
}
