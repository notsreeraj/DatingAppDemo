using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
// a new package is installed for this "jwt bearer"
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    // getting the token key from the config file and use it to decrypt the token
    var tokenKey = builder.Configuration["TokenKey"]
        ?? throw new Exception("Token key not found - Program.cs");
    // setting up the options with token validation parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // validate the signing key of the token
        ValidateIssuerSigningKey = true,
        // give it the issuer sign in key 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        // 
        ValidateIssuer = false,
        ValidateAudience = false
    };
 });

var app = builder.Build();

// middleware
// Configure the HTTP request pipeline.
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().
WithOrigins("https://localhost:4200", "http://localhost:4200"));

// setting up the authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
