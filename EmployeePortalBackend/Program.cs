using EmployeePortalBackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register authentication services and specify JwtBearer as the authentication scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // Configure the JwtBearer handler that will validate incoming Authorization
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Ensure the token was issued by the expected issuer
            ValidateIssuer = true,
            // Ensure the token is intended for the expected audience
            ValidateAudience = true,
            // Ensure the token has not expired
            ValidateLifetime = true,
            // Ensure the signing key is valid and signature matches
            ValidateIssuerSigningKey = true,
            // Value used to validate the "iss" claim (must match the issuer used when creating the token)
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            // Value used to validate the "aud" claim (must match the audience used when creating the token)
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // The key used to validate the token's signature — must be the same secret used to sign tokens
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Register authorization (enables policy and role checks)
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    // Define a CORS policy to allow requests from the Angular frontend
    options.AddPolicy("AllowAngularClient",
        policy =>
        {
            // Adjust the URL to match the Angular app's URL and port
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader() // Allow any HTTP headers
                  .AllowAnyMethod(); // Allow any HTTP methods (GET, POST, etc.)
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS policy
app.UseCors("AllowAngularClient");

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
