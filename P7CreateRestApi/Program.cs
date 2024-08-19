using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Services;


var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/P7CreateRestApi-{Date}.txt");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LocalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LocalDbContext>();

builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<IdentityOptions>(options =>
{
        // Paramètres utilisateur.
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

        // Paramètres des mots de passe.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 9;

        // Paramètres de verrouillage.
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
});



builder.Services.AddScoped<BidRepository>();
builder.Services.AddScoped<CurveRepository>();
builder.Services.AddScoped<RatingRepository>();
builder.Services.AddScoped<RuleRepository>();
builder.Services.AddScoped<TradeRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Schemes:Bearer:ValidIssuer"],
        ValidAudiences = builder.Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<string[]>(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:Schemes:Bearer:Secret"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
