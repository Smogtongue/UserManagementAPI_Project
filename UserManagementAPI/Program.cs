using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen; // Add this using directive
using Microsoft.AspNetCore.Authentication.JwtBearer; // Add this using directive
using Microsoft.IdentityModel.Tokens; // Add this using directive
using System.Text; // Add this using directive
using System.IdentityModel.Tokens.Jwt; // Add this using directive
using System.Security.Claims; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Read JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];
var secretKey = jwtSettings["SecretKey"];

// Add null checks for JWT settings
if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JWT settings are not configured properly.");
}

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Generate a valid JWT token for testing purposes
string GenerateJwtToken()
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(secretKey);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim("sub", "test-user") }),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = issuer,
        Audience = audience,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root

        // Set the default token in Swagger
        var token = GenerateJwtToken();
        c.ConfigObject.AdditionalItems["Authorization"] = $"Bearer {token}";
    });

    // Log the generated token to the console
    var generatedToken = GenerateJwtToken();
    Console.WriteLine($"Generated JWT Token: {generatedToken}");
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization(); // Add authorization middleware

// Add the error handling middleware
app.UseErrorHandlingMiddleware();

// Add the logging middleware
app.UseLoggingMiddleware();

app.MapControllers();

app.Run();