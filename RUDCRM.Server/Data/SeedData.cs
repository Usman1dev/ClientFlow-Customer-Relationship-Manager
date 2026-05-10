using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
using RUDCRM.Server.Models;
=======
using RUDCRM.Shared.Models;
>>>>>>> f1f16b05775f1962e046e7a92be03b9421eef765

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

<<<<<<< HEAD
=======
        // ── Seed Sample Customers ──────────────────────────────────────
        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new() { FirstName = "Sarah", LastName = "Johnson", Email = "sarah.johnson@techcorp.com", Phone = "555-0101", Company = "TechCorp Solutions", Address = "123 Innovation Drive", City = "Austin", State = "TX", ZipCode = "73301", Notes = "Premium client since 2023", CreatedByUserId = adminUser.Id },
                new() { FirstName = "Michael", LastName = "Chen", Email = "m.chen@designhub.io", Phone = "555-0102", Company = "DesignHub Studio", Address = "456 Creative Avenue", City = "San Francisco", State = "CA", ZipCode = "94102", Notes = "Interested in enterprise plan", CreatedByUserId = adminUser.Id },
                new() { FirstName = "Emily", LastName = "Rodriguez", Email = "emily.r@greenleaf.org", Phone = "555-0103", Company = "GreenLeaf Industries", Address = "789 Sustainability Blvd", City = "Portland", State = "OR", ZipCode = "97201", Notes = "Non-profit organization", CreatedByUserId = adminUser.Id },
                new() { FirstName = "David", LastName = "Kim", Email = "david.kim@datapulse.com", Phone = "555-0104", Company = "DataPulse Analytics", Address = "321 Data Center Road", City = "Seattle", State = "WA", ZipCode = "98101", Notes = "Looking for custom CRM integration", CreatedByUserId = adminUser.Id },
                new() { FirstName = "Lisa", LastName = "Thompson", Email = "lisa.t@skybridge.net", Phone = "555-0105", Company = "SkyBridge Consulting", Address = "654 Corporate Plaza", City = "Denver", State = "CO", ZipCode = "80202", Notes = "Referred by Michael Chen", CreatedByUserId = adminUser.Id },
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();
        }

        // ── Seed Sample Leads ──────────────────────────────────────────
        if (!context.Leads.Any())
        {
            var leads = new List<Lead>
            {
                new() { ContactName = "Alex Morgan", Email = "alex@startupventure.io", Phone = "555-0201", Company = "StartupVenture Inc", Source = "Web", Status = "New", EstimatedValue = 15000, Notes = "Filled out contact form", AssignedToUserId = adminUser.Id },
                new() { ContactName = "Rachel Green", Email = "rachel@mediapro.com", Phone = "555-0202", Company = "MediaPro Agency", Source = "Referral", Status = "Contacted", EstimatedValue = 25000, Notes = "Referred by existing client", AssignedToUserId = adminUser.Id },
                new() { ContactName = "Tom Wilson", Email = "tom@buildright.co", Phone = "555-0203", Company = "BuildRight Construction", Source = "Social", Status = "Qualified", EstimatedValue = 50000, Notes = "Met at trade show", AssignedToUserId = adminUser.Id },
                new() { ContactName = "Nina Patel", Email = "nina@healthtech.io", Phone = "555-0204", Company = "HealthTech Solutions", Source = "Advertisement", Status = "New", EstimatedValue = 35000, Notes = "Saw our ad on LinkedIn" },
                new() { ContactName = "James Carter", Email = "james@logisticspro.com", Phone = "555-0205", Company = "LogisticsPro", Source = "Web", Status = "Contacted", EstimatedValue = 20000, Notes = "Needs fleet management CRM" },
            };

            context.Leads.AddRange(leads);
            await context.SaveChangesAsync();
        }

        // ── Seed Sample Tasks ──────────────────────────────────────────
        if (!context.TaskItems.Any())
        {
            var tasks = new List<TaskItem>
            {
                new() { Title = "Follow up with TechCorp", Description = "Schedule a demo call with Sarah Johnson", DueDate = DateTime.UtcNow.AddDays(2), Priority = "High", Status = "Pending", AssignedToUserId = adminUser.Id, CreatedByUserId = adminUser.Id },
                new() { Title = "Prepare Q2 sales report", Description = "Compile revenue data for quarterly review", DueDate = DateTime.UtcNow.AddDays(7), Priority = "Medium", Status = "InProgress", AssignedToUserId = adminUser.Id, CreatedByUserId = adminUser.Id },
                new() { Title = "Update pricing guide", Description = "Revise pricing tiers for 2024", DueDate = DateTime.UtcNow.AddDays(14), Priority = "Low", Status = "Pending", CreatedByUserId = adminUser.Id },
                new() { Title = "Client onboarding checklist", Description = "Create standardized onboarding process", DueDate = DateTime.UtcNow.AddDays(5), Priority = "Urgent", Status = "Pending", AssignedToUserId = adminUser.Id, CreatedByUserId = adminUser.Id },
            };

            context.TaskItems.AddRange(tasks);
            await context.SaveChangesAsync();
        }

        // ── Seed Sample Invoices ───────────────────────────────────────
        if (!context.Invoices.Any())
        {
            var customers = context.Customers.Take(3).ToList();
            if (customers.Count >= 3)
            {
                var invoices = new List<Invoice>
                {
                    new() { InvoiceNumber = "INV-2024-001", CustomerId = customers[0].Id, IssueDate = DateTime.UtcNow.AddDays(-30), DueDate = DateTime.UtcNow, TotalAmount = 5000, Status = "Paid", CreatedByUserId = adminUser.Id },
                    new() { InvoiceNumber = "INV-2024-002", CustomerId = customers[1].Id, IssueDate = DateTime.UtcNow.AddDays(-15), DueDate = DateTime.UtcNow.AddDays(15), TotalAmount = 12500, Status = "Sent", CreatedByUserId = adminUser.Id },
                    new() { InvoiceNumber = "INV-2024-003", CustomerId = customers[2].Id, IssueDate = DateTime.UtcNow.AddDays(-5), DueDate = DateTime.UtcNow.AddDays(25), TotalAmount = 3200, Status = "Draft", CreatedByUserId = adminUser.Id },
                };

                context.Invoices.AddRange(invoices);
                await context.SaveChangesAsync();

                // Seed a payment for the paid invoice
                var paidInvoice = invoices[0];
                context.Payments.Add(new Payment
                {
                    InvoiceId = paidInvoice.Id,
                    Amount = 5000,
                    PaymentDate = DateTime.UtcNow.AddDays(-10),
                    PaymentMethod = "BankTransfer",
                    ReferenceNumber = "TXN-001234"
                });
                await context.SaveChangesAsync();
            }
        }

>>>>>>> f1f16b05775f1962e046e7a92be03b9421eef765
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
