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
            JWT Token Structure:
            header.payload.signature
            
            Example: eyJhbGciOiJIUzUxMiJ9.eyJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20ifQ.signature_hash
            
            Header:    {"alg":"HS512","typ":"JWT"}
            Payload:   {"email":"user@example.com","nameid":"123","exp":1692883200}
            Signature: HMAC_SHA512(header + payload, secret_key)
        */

        // Step 0: Get and validate the secret key from configuration
        // This key is used to sign the token and later verify its authenticity
        var tokenkey = config["Tokenkey"] ?? throw new Exception("Cannot Get Token Key");
        
        // Security requirement: Key must be at least 64 characters for HMAC-SHA512
        if (tokenkey.Length < 64) throw new Exception("Your token Key Needs to be >= 64 characters");

        #region 1. Prepare Signing Key (for Header & Signature)
        // Convert the string key to bytes - cryptographic functions work with bytes
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));
        
        // Create signing credentials that specify:
        // - What key to use (our secret key)
        // - What algorithm to use (HMAC-SHA512)
        // This will set the "alg" field in the JWT header to "HS512"
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        #endregion

        #region 2. Create Claims (for Payload)
        // Claims are the actual data/information stored in the JWT payload
        // These will be available when the token is validated later
        var claims = new List<Claim>
        {
            // Store user's email - can be accessed later with ClaimTypes.Email
            new(ClaimTypes.Email, user.Email),
            
            // Store user's ID - can be accessed later with ClaimTypes.NameIdentifier
            // This is like the "primary key" to identify which user this token belongs to
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        #endregion

        #region 3. Build Token Descriptor (Header + Payload + Signature)
        // This is the "recipe" that tells the JWT library how to create the token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Subject contains the claims (user data) that go into the JWT payload
            Subject = new ClaimsIdentity(claims),
            
            // Set token expiration - this creates the "exp" claim in the payload
            // Token will be invalid after 7 days from now
            Expires = DateTime.UtcNow.AddDays(7),
            
            // Tell the library how to sign the token (algorithm + key)
            // This affects both the header ("alg": "HS512") and signature creation
            SigningCredentials = creds
        };
        #endregion

        #region 4. Generate Final JWT Token
        // Create the JWT handler - this is the "factory" that builds JWT tokens
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // THIS IS WHERE THE MAGIC HAPPENS:
        // 1. Creates header: {"alg":"HS512","typ":"JWT"}
        // 2. Creates payload with claims + expiration
        // 3. Encodes header and payload to Base64
        // 4. Creates signature: HMAC_SHA512(header + payload, secret_key)
        // 5. Combines all: header.payload.signature
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        // Convert the token object to a string format that can be sent over HTTP
        // Returns something like: "eyJhbGciOiJIUzUxMiJ9.eyJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20ifQ.signature"
        return tokenHandler.WriteToken(token);
        #endregion
    }
}
