using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Middleware;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// here the AppDbContext is registered as a service . Dependancy injection
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
// here the controller will know which class to instantaite when we use token services
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
// a new package is installed for this "jwt bearer"
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    // getting the token key from the config file and use it to valiadate the token
    var tokenKey = builder.Configuration["TokenKey"]
        ?? throw new Exception("Token key not found - Program.cs");
    // setting up the options with token validation parameters
   options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,  // "Check the signature is valid"
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)), // "Use this key to verify"
        ValidateIssuer = false,   // "Don't check who issued the token"
        ValidateAudience = false  // "Don't check who the token is for"
    };
 });

var app = builder.Build();

// middleware

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().
WithOrigins("https://localhost:4200", "http://localhost:4200"));

// setting up the authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region  Seed data (Service Locator Pattern)

// seed the data 
// service locator pattern
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    // adding migration through code rather than through cli
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);

}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during Migration");
};
#endregion




app.Run();

// this is how the authenticatio works , mainly the order of methods 
/*
Received Token → Split into 3 parts
    ↓
Decode header and payload (Base64 decode - anyone can do this)
    ↓
Recreate signature: HMAC512(header + payload, YOUR_secret_key)
    ↓
Compare: Does recreated signature = received signature?
    ↓
If YES → Token is valid (created by you)
If NO → Token is invalid/tampered
*/
