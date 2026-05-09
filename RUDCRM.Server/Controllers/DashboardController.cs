using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUDCRM.Server.Data;
using RUDCRM.Shared.DTOs;
using System.Security.Claims;

namespace RUDCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public DashboardController(ApplicationDbContext context) { _context = context; }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

        var dashboard = new DashboardDto
        {
            TotalCustomers = await _context.Customers.CountAsync(),
            TotalLeads = await _context.Leads.CountAsync(),
            TotalInvoices = await _context.Invoices.CountAsync(),
            PendingTasks = await _context.TaskItems.CountAsync(t => t.Status == "Pending" || t.Status == "InProgress"),
            TotalRevenue = await _context.Payments.SumAsync(p => p.Amount),
            UnreadNotifications = await _context.Notifications.CountAsync(n => (n.UserId == userId || n.UserId == null) && !n.IsRead),
            RecentCustomers = await _context.Customers.OrderByDescending(c => c.CreatedAt).Take(5)
                .Select(c => new CustomerDto { Id = c.Id, FirstName = c.FirstName, LastName = c.LastName, Email = c.Email, Company = c.Company, CreatedAt = c.CreatedAt }).ToListAsync(),
            RecentLeads = await _context.Leads.OrderByDescending(l => l.CreatedAt).Take(5)
                .Select(l => new LeadDto { Id = l.Id, ContactName = l.ContactName, Email = l.Email, Company = l.Company, Status = l.Status, EstimatedValue = l.EstimatedValue, CreatedAt = l.CreatedAt }).ToListAsync(),
            UpcomingTasks = await _context.TaskItems.Where(t => t.Status != "Completed" && t.Status != "Cancelled").OrderBy(t => t.DueDate).Take(5)
                .Select(t => new TaskItemDto { Id = t.Id, Title = t.Title, DueDate = t.DueDate, Priority = t.Priority, Status = t.Status }).ToListAsync(),
            MonthlyRevenue = await _context.Payments.Where(p => p.PaymentDate >= sixMonthsAgo)
                .GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
                .Select(g => new MonthlyRevenueDto { Month = g.Key.Year + "-" + g.Key.Month, Revenue = g.Sum(p => p.Amount) }).OrderBy(m => m.Month).ToListAsync(),
            LeadsByStatus = await _context.Leads.GroupBy(l => l.Status)
                .Select(g => new LeadStatusCountDto { Status = g.Key, Count = g.Count() }).ToListAsync(),
            MonthlyCustomers = await _context.Customers.Where(c => c.CreatedAt >= sixMonthsAgo)
                .GroupBy(c => new { c.CreatedAt.Year, c.CreatedAt.Month })
                .Select(g => new MonthlyCustomerDto { Month = g.Key.Year + "-" + g.Key.Month, Count = g.Count() }).OrderBy(m => m.Month).ToListAsync()
        };
        return Ok(dashboard);
    }
}
