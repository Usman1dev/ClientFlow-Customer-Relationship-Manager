using System.Net.Http.Json;
using ClientFlowCRM.Server.DTOs;

namespace ClientFlowCRM.Server.Services;

public class CustomerService
{
    private readonly HttpClient _http;
    public CustomerService(HttpClient http) { _http = http; }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<CustomerDto>>("api/customers") ?? new(); }
        catch { return new(); }
    }
    public async Task<CustomerDto?> GetByIdAsync(int id) =>
        await _http.GetFromJsonAsync<CustomerDto>($"api/customers/{id}");
    public async Task<bool> CreateAsync(CustomerDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/customers", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateAsync(int id, CustomerDto dto)
    {
        var r = await _http.PutAsJsonAsync($"api/customers/{id}", dto); return r.IsSuccessStatusCode;
    }
    public async Task<string?> DeleteAsync(int id)
    {
        var r = await _http.DeleteAsync($"api/customers/{id}");
        if (r.IsSuccessStatusCode) return null;
        var error = await r.Content.ReadAsStringAsync();
        return string.IsNullOrEmpty(error) ? "Operation failed." : error;
    }
}

public class LeadService
{
    private readonly HttpClient _http;
    public LeadService(HttpClient http) { _http = http; }

    public async Task<List<LeadDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<LeadDto>>("api/leads") ?? new(); }
        catch { return new(); }
    }
    public async Task<bool> CreateAsync(LeadDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/leads", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateAsync(int id, LeadDto dto)
    {
        var r = await _http.PutAsJsonAsync($"api/leads/{id}", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var r = await _http.DeleteAsync($"api/leads/{id}"); return r.IsSuccessStatusCode;
    }
}

public class AppointmentService
{
    private readonly HttpClient _http;
    public AppointmentService(HttpClient http) { _http = http; }

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<AppointmentDto>>("api/appointments") ?? new(); }
        catch { return new(); }
    }
    public async Task<bool> CreateAsync(AppointmentDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/appointments", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateAsync(int id, AppointmentDto dto)
    {
        var r = await _http.PutAsJsonAsync($"api/appointments/{id}", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var r = await _http.DeleteAsync($"api/appointments/{id}"); return r.IsSuccessStatusCode;
    }
}

public class InvoiceService
{
    private readonly HttpClient _http;
    public InvoiceService(HttpClient http) { _http = http; }

    public async Task<List<InvoiceDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<InvoiceDto>>("api/invoices") ?? new(); }
        catch { return new(); }
    }
    public async Task<bool> CreateAsync(InvoiceDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/invoices", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateAsync(int id, InvoiceDto dto)
    {
        var r = await _http.PutAsJsonAsync($"api/invoices/{id}", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var r = await _http.DeleteAsync($"api/invoices/{id}"); return r.IsSuccessStatusCode;
    }
}

public class PaymentService
{
    private readonly HttpClient _http;
    public PaymentService(HttpClient http) { _http = http; }

    public async Task<List<PaymentDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<PaymentDto>>("api/payments") ?? new(); }
        catch { return new(); }
    }
    public async Task<bool> CreateAsync(PaymentDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/payments", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateAsync(int id, PaymentDto dto)
    {
        var r = await _http.PutAsJsonAsync($"api/payments/{id}", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var r = await _http.DeleteAsync($"api/payments/{id}"); return r.IsSuccessStatusCode;
    }
}

public class TaskService
{
    private readonly HttpClient _http;
    public TaskService(HttpClient http) { _http = http; }

    public async Task<List<TaskItemDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<TaskItemDto>>("api/tasks") ?? new(); }
        catch { return new(); }
    }
    public async Task<bool> CreateAsync(TaskItemDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/tasks", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> UpdateAsync(int id, TaskItemDto dto)
    {
        var r = await _http.PutAsJsonAsync($"api/tasks/{id}", dto); return r.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var r = await _http.DeleteAsync($"api/tasks/{id}"); return r.IsSuccessStatusCode;
    }
}

public class NotificationService
{
    private readonly HttpClient _http;
    public NotificationService(HttpClient http) { _http = http; }

    public async Task<List<NotificationDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<NotificationDto>>("api/notifications") ?? new(); }
        catch { return new(); }
    }
    public async Task<int> GetUnreadCountAsync()
    {
        try { return await _http.GetFromJsonAsync<int>("api/notifications/unread-count"); }
        catch { return 0; }
    }
    public async Task MarkAsReadAsync(int id) => await _http.PutAsync($"api/notifications/{id}/read", null);
    public async Task MarkAllAsReadAsync() => await _http.PutAsync("api/notifications/read-all", null);
    public async Task DeleteAsync(int id) => await _http.DeleteAsync($"api/notifications/{id}");
}

public class FileUploadService
{
    private readonly HttpClient _http;
    public FileUploadService(HttpClient http) { _http = http; }

    public async Task<List<UploadedDocumentDto>> GetAllAsync()
    {
        try { return await _http.GetFromJsonAsync<List<UploadedDocumentDto>>("api/fileupload") ?? new(); }
        catch { return new(); }
    }
    public async Task<bool> UploadAsync(MultipartFormDataContent content)
    {
        var r = await _http.PostAsync("api/fileupload", content); return r.IsSuccessStatusCode;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var r = await _http.DeleteAsync($"api/fileupload/{id}"); return r.IsSuccessStatusCode;
    }
}


public class DashboardService
{
    private readonly HttpClient _http;
    public DashboardService(HttpClient http) { _http = http; }

    public async Task<DashboardDto?> GetDashboardAsync()
    {
        try { return await _http.GetFromJsonAsync<DashboardDto>("api/dashboard"); }
        catch { return null; }
    }
}

public class AdminService
{
    private readonly HttpClient _http;
    public AdminService(HttpClient http) { _http = http; }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try { return await _http.GetFromJsonAsync<List<UserDto>>("api/admin/users") ?? new(); }
        catch { return new(); }
    }
    public async Task<bool> UpdateRoleAsync(string userId, string role)
    {
        var r = await _http.PutAsJsonAsync($"api/admin/users/{userId}/role", role); return r.IsSuccessStatusCode;
    }
    public async Task<bool> ToggleActiveAsync(string userId)
    {
        var r = await _http.PutAsync($"api/admin/users/{userId}/toggle-active", null); return r.IsSuccessStatusCode;
    }
    public async Task<bool> SendAnnouncementAsync(AnnouncementDto dto)
    {
        var r = await _http.PostAsJsonAsync("api/admin/announcements", dto); return r.IsSuccessStatusCode;
    }
}
