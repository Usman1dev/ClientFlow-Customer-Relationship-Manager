using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUDCRM.Server.Data;
using RUDCRM.Shared.DTOs;
using RUDCRM.Shared.Models;
using System.Security.Claims;

namespace RUDCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AppointmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AppointmentDto>>> GetAll()
    {
        var items = await _context.Appointments
            .Include(a => a.Customer)
            .OrderByDescending(a => a.StartTime)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Location = a.Location,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer != null ? a.Customer.FirstName + " " + a.Customer.LastName : null,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id)
    {
        var a = await _context.Appointments.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id);
        if (a == null) return NotFound();

        return Ok(new AppointmentDto
        {
            Id = a.Id, Title = a.Title, Description = a.Description,
            StartTime = a.StartTime, EndTime = a.EndTime, Location = a.Location,
            CustomerId = a.CustomerId,
            CustomerName = a.Customer != null ? $"{a.Customer.FirstName} {a.Customer.LastName}" : null,
            CreatedAt = a.CreatedAt
        });
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Create([FromBody] AppointmentDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var appointment = new Appointment
        {
            Title = dto.Title,
            Description = dto.Description,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Location = dto.Location,
            CustomerId = dto.CustomerId,
            CreatedByUserId = userId
        };

        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();

        dto.Id = appointment.Id;
        dto.CreatedAt = appointment.CreatedAt;
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AppointmentDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return NotFound();

        appointment.Title = dto.Title;
        appointment.Description = dto.Description;
        appointment.StartTime = dto.StartTime;
        appointment.EndTime = dto.EndTime;
        appointment.Location = dto.Location;
        appointment.CustomerId = dto.CustomerId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return NotFound();

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
