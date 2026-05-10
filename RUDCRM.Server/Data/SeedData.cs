using Microsoft.AspNetCore.Identity;
using RUDCRM.Server.Models;

namespace RUDCRM.Server.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // ── Seed Roles ─────────────────────────────────────────────────
        string[] roles = { "Admin", "Employee" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // ── Seed Admin User ────────────────────────────────────────────
        var adminEmail = "admin@rudcrm.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Administrator",
                Role = "Admin",
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // ── Seed Employee User ─────────────────────────────────────────
        var employeeEmail = "employee@rudcrm.com";
        var employeeUser = await userManager.FindByEmailAsync(employeeEmail);
        if (employeeUser == null)
        {
            employeeUser = new ApplicationUser
            {
                UserName = employeeEmail,
                Email = employeeEmail,
                FullName = "John Employee",
                Role = "Employee",
                EmailConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(employeeUser, "Employee@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(employeeUser, "Employee");
            }
        }

        // ── Seed Welcome Notification ──────────────────────────────────
        if (!context.Notifications.Any())
        {
            context.Notifications.Add(new Notification
            {
                Title = "Welcome to RUD-CRM!",
                Message = "Your CRM system is ready. Start by adding customers and managing your leads.",
                Type = "Success",
                UserId = adminUser.Id
            });
            await context.SaveChangesAsync();
        }
    }
}
