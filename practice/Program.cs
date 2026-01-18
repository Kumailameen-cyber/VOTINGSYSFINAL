using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using practice.Data;
using practice.Models;
using practice.Repository;
using practice.Repository.Implementation;
using practice.Repository.Interface;
using practice.Services;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };

    // For MVC views, we'll use cookie-based auth
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Try to get token from cookie
            if (context.Request.Cookies.ContainsKey("AuthToken"))
            {
                context.Token = context.Request.Cookies["AuthToken"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Register services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// ADD THESE
// 1. Connect JSON settings to your C# class
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// 2. Register EmailService as Transient (Better for MailKit/SMTP connections)
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVoterService, VoterService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();

builder.Services.AddScoped<IElectionRepository, ElectionRepository>();
builder.Services.AddScoped<IVoterRepository, VoterRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();



// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// TEMP: Generate a hash to see it in the console
Console.WriteLine("Generated Hash: " + BCrypt.Net.BCrypt.HashPassword("Pass@123"));
// AUTOMATIC MIGRATION
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // REPLACE 'OnlineVotingSystem.Data.ApplicationDbContext' with your actual Context name if different
    var context = services.GetRequiredService<practice.Data.ApplicationDbContext>();
    context.Database.Migrate();
}
app.Run();
