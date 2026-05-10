using System.ComponentModel.DataAnnotations;

namespace RUDCRM.Shared.DTOs;

public class NotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "Info";
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UserId { get; set; }
}

public class AnnouncementDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required")]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;
}

public class UploadedDocumentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? UploadedByUserName { get; set; }
}

public class WeatherDto
{
    public string City { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public double FeelsLike { get; set; }
    public int Humidity { get; set; }
}

public class DashboardDto
{
    public int TotalCustomers { get; set; }
    public int TotalLeads { get; set; }
    public int TotalInvoices { get; set; }
    public int PendingTasks { get; set; }
    public decimal TotalRevenue { get; set; }
    public int UnreadNotifications { get; set; }
    public List<CustomerDto> RecentCustomers { get; set; } = new();
    public List<LeadDto> RecentLeads { get; set; } = new();
    public List<TaskItemDto> UpcomingTasks { get; set; } = new();
    public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    public List<LeadStatusCountDto> LeadsByStatus { get; set; } = new();
    public List<MonthlyCustomerDto> MonthlyCustomers { get; set; } = new();
}

public class MonthlyRevenueDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class LeadStatusCountDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class MonthlyCustomerDto
{
    public string Month { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
