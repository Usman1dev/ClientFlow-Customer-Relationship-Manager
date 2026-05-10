using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RUDCRM.Server.Data;
using RUDCRM.Server.Hubs;
<<<<<<< HEAD
using RUDCRM.Server.DTOs;
using RUDCRM.Server.Models;
=======
using RUDCRM.Shared.DTOs;
using RUDCRM.Shared.Models;
>>>>>>> f1f16b05775f1962e046e7a92be03b9421eef765

namespace RUDCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _userManager = userManager;
        _context = context;
        _hubContext = hubContext;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await _context.Users.OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserDto
            {
                Id = u.Id, FullName = u.FullName, Email = u.Email!,
                Role = u.Role, IsActive = u.IsActive, CreatedAt = u.CreatedAt
            }).ToListAsync();
        return Ok(users);
    }

    [HttpPut("users/{id}/role")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] string newRole)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, newRole);
        user.Role = newRole;
        await _userManager.UpdateAsync(user);
        return NoContent();
    }

    [HttpPut("users/{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        user.IsActive = !user.IsActive;
        await _userManager.UpdateAsync(user);
        return Ok(new { user.IsActive });
    }

    [HttpPost("announcements")]
    public async Task<IActionResult> SendAnnouncement([FromBody] AnnouncementDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Save as notification for all users
        var notification = new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            Type = "Announcement",
            UserId = null // broadcast
        };
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Broadcast via SignalR
        await _hubContext.Clients.All.SendAsync("ReceiveAnnouncement", dto.Title, dto.Message);
        return Ok(new { message = "Announcement sent successfully." });
    }
}
