using Team34FinalAPI.Models;
using Team34FinalAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Builder;
using OfficeOpenXml;
using System.IO;
using Team34FinalAPI.Models;
using Team34FinalAPI.Data;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using User = Team34FinalAPI.Models.User;
using Org.BouncyCastle.Crypto.Tls;
using Microsoft.Extensions.FileProviders;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:4200",
                    "https://your-frontend-name.vercel.app" // ADD YOUR VERCEL URL HERE
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});



builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//My Changes For Jwt Auth for Login purposes

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Team 34 API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
        new string[] { }
    }
    });
});


//Inspection List exporting
//var context = new CustomAssemblyLoadContext();
//context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltox.dll"));

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


//Feedback config.

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


//Config DbContexts
//Configure AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configure BookingDbContext
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configure RateDbContext
builder.Services.AddDbContext<RateEEDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure TripDbContext
builder.Services.AddDbContext<TripDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configure VehicleDbContext
builder.Services.AddDbContext<VehicleDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure UserDbContext & Identity
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity config

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // User settings
    // options.User.RequireUniqueEmail = true;


    // Password settings (optional)
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

})
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    //options.User.RequireUniqueEmail = true;
});
// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// Configure authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("DriverPolicy", policy => policy.RequireRole("Driver", "Admin"));
});

//Add repositories and services
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IInspectionListRepository, InspectionRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IRefuelVehicleRepository, RefuelVehicleRepository>();
builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IRateRepo, RateRepository>();
builder.Services.AddScoped<IFAQRepo, FAQRepo>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ReportService, ReportService>();

builder.Services.AddScoped<IOTPRepository, OTPRepository>();
builder.Services.AddScoped<IOTPService, OTPService>();

builder.Services.AddScoped<OTPSettingsService>();
builder.Services.AddScoped<ISMS_Service, SMS_Service>();
builder.Services.AddScoped<BookingReminderService>();

builder.Services.AddHostedService<BookingReminderHostedService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();




// Configure MailJet settings
builder.Services.Configure<MailJetOptions>(builder.Configuration.GetSection("MailJet"));

//Configure OTP Settings'
builder.Services.Configure<OTPSettings>(builder.Configuration.GetSection("OtpSettings"));

// Register the MailJetService
builder.Services.AddScoped<IEmailService, MailJetService>();



builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


builder.Services.AddLogging();

var app = builder.Build();

//Mail Configs

// Temporary logging to check the API credentials

var mailJetSettings = builder.Configuration.GetSection("MailJet");
var apiKey = mailJetSettings["ApiKey"];
var secretKey = mailJetSettings["SecretKey"];
var senderEmail = mailJetSettings["SenderEmail"];
var senderName = mailJetSettings["SenderName"];


Console.WriteLine($"API Key: {apiKey}");
Console.WriteLine($"Secret Key: {secretKey}");
Console.WriteLine($"Sender Email: {senderEmail}");
Console.WriteLine($"Sender Name: {senderName}");

// Validate configuration values
if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(secretKey) ||
    string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderName))
{
    throw new Exception("MailJet settings are not properly configured.");
}


// Create Mailjet client
var client = new MailjetClient(apiKey, secretKey);


var request = new MailjetRequest
{
    Resource = Send.Resource,
}
.Property(Send.FromEmail, senderEmail)
.Property(Send.FromName, senderName)
.Property(Send.Subject, "Test Email")
.Property(Send.HtmlPart, "This is a test email.")
.Property(Send.Recipients, new JArray
{
    new JObject
    {
        {"Email", "www.sabzayj7@gmailcom"}
    }
})
.Property(Send.SandboxMode, false); // Enable sandbox mode

/*var response = await client.PostAsync(request);
if (!response.IsSuccessStatusCode)
{
    var errorMessage = response.GetErrorMessage();
    Console.WriteLine($"Failed to send email. Status: {response.StatusCode}, Message: {errorMessage}");
    throw new Exception($"Failed to send email. Status: {response.StatusCode}, Message: {errorMessage}");
}*/

try
{
    var response = await client.PostAsync(request);
    if (!response.IsSuccessStatusCode)
    {
        var errorMessage = response.GetErrorMessage();
        Console.WriteLine($"Failed to send email. Status: {response.StatusCode}, Message: {errorMessage}");
        throw new Exception($"Failed to send email. Status: {response.StatusCode}, Message: {errorMessage}");
    }

    Console.WriteLine("Email sent successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}

//Console.WriteLine("Email sent successfully.");
// Configure the HTTP request pipeline.
// Always enable Swagger (useful during Azure debugging)
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();



// Add this line to serve files from Images directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();


//Role seeding. Happens during runtime

// Seed roles and assign roles to users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    try
    {
        await RoleInitializer.SeedRoles(roleManager);
        await RoleInitializer.AssignRolesToUsers(userManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred while seeding roles: " + ex.Message);
    }
}
app.Run();

public static class RoleInitializer
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Admin", "User", "Driver" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task AssignRolesToUsers(UserManager<User> userManager)
    {
        // Add your usernames and roles here
        var userRoles = new[]
        {
            new { Username = "admin", Role = "Admin", Password = "Admin@123" },
            new { Username = "user", Role = "User", Password = "User@123" },
            new { Username = "driver", Role = "Driver", Password = "Driver@123" }
        };

        foreach (var userRole in userRoles)
        {
            var user = await userManager.FindByNameAsync(userRole.Username);
            if (user == null)
            {
                // Create a new user
                user = new User
                {
                    UserName = userRole.Username,
                    Email = $"{userRole.Username}@example.com",
                    Name = userRole.Username,
                    Surname = "User", // Set the surname appropriately
                    Role = userRole.Role,
                    PhoneNumber = "0123456789",
                    ProfilePhotoPath = "default.png"
                };
                await userManager.CreateAsync(user, userRole.Password);
            }

            if (!await userManager.IsInRoleAsync(user, userRole.Role))
            {
                await userManager.AddToRoleAsync(user, userRole.Role);
            }
        }
    }

}
