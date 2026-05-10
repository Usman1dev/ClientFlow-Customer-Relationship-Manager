using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ClientFlowCRM.Server.Data;
using ClientFlowCRM.Server.Hubs;
using ClientFlowCRM.Server.Services;
using ClientFlowCRM.Server.Models;
using System.Text;
using Blazored.LocalStorage;
namespace ClientFlowCRM.Server;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ── Entity Framework Core ──────────────────────────────────────
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // ── ASP.NET Core Identity ──────────────────────────────────────
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // ── JWT Authentication ─────────────────────────────────────────
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]!;

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            // Allow SignalR to receive the token via query string
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("EmployeeOrAdmin", policy => policy.RequireRole("Admin", "Employee"));
        });

        // ── Services ───────────────────────────────────────────────────        builder.Services.AddHttpClient<WeatherService>();
        builder.Services.AddScoped<WeatherService>();

        // ── Controllers & SignalR ──────────────────────────────────────
        builder.Services.AddControllers();
        builder.Services.AddSignalR();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // ── Blazor Server Hosting ────────────────────────────────────────
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        // ── Client Services ────────────────────────────────────────────
        // Configure HttpClient to call local APIs
        builder.Services.AddScoped(sp => {
            var navMan = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
            var handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            return new HttpClient(handler) { BaseAddress = new Uri(navMan.BaseUri) };
        });
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.AuthService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.CustomerService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.LeadService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.AppointmentService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.InvoiceService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.PaymentService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.TaskService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.NotificationService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.FileUploadService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.DashboardService>();
        builder.Services.AddScoped<ClientFlowCRM.Server.Services.AdminService>();
        builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, ClientFlowCRM.Server.Auth.CustomAuthStateProvider>();
        builder.Services.AddBlazoredLocalStorage();
        var app = builder.Build();

        // ── Seed Database ──────────────────────────────────────────────
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                // await context.Database.MigrateAsync(); // Removed because tables are managed via db_setup.sql                await SeedData.InitializeAsync(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        // ── Middleware Pipeline ────────────────────────────────────────
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();
        // Map API controllers
        app.MapControllers();

        // Map SignalR hub
        app.MapHub<NotificationHub>("/hubs/notifications");

        // Map Blazor Server
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.Run();
    }
}
