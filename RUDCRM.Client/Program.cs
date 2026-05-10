using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RUDCRM.Client.Auth;
using RUDCRM.Client.Services;

namespace RUDCRM.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
        });

        // Local Storage
        builder.Services.AddBlazoredLocalStorage();

        // Authentication
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

        // Application Services
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<CustomerService>();
        builder.Services.AddScoped<LeadService>();
        builder.Services.AddScoped<AppointmentService>();
        builder.Services.AddScoped<InvoiceService>();
        builder.Services.AddScoped<PaymentService>();
        builder.Services.AddScoped<TaskService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<FileUploadService>();
        builder.Services.AddScoped<WeatherService>();
        builder.Services.AddScoped<DashboardService>();
        builder.Services.AddScoped<AdminService>();

        await builder.Build().RunAsync();
    }
}
