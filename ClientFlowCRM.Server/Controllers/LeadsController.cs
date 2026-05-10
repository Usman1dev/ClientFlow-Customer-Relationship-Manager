using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientFlowCRM.Server.Data;
using ClientFlowCRM.Server.DTOs;
using ClientFlowCRM.Server.Models;using System.Security.Claims;

namespace ClientFlowCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LeadsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LeadsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeadDto>>> GetAll()
    {
        var leads = await _context.Leads
            .Include(l => l.AssignedToUser)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LeadDto
            {
                Id = l.Id,
                ContactName = l.ContactName,
                Email = l.Email,
                Phone = l.Phone,
                Company = l.Company,
                Source = l.Source,
                Status = l.Status,
                EstimatedValue = l.EstimatedValue,
                Notes = l.Notes,
                CreatedAt = l.CreatedAt,
                AssignedToUserId = l.AssignedToUserId,
                AssignedToUserName = l.AssignedToUser != null ? l.AssignedToUser.FullName : null
            })
            .ToListAsync();

        return Ok(leads);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeadDto>> GetById(int id)
    {
        var l = await _context.Leads.Include(x => x.AssignedToUser).FirstOrDefaultAsync(x => x.Id == id);
        if (l == null) return NotFound();

        return Ok(new LeadDto
        {
            Id = l.Id, ContactName = l.ContactName, Email = l.Email, Phone = l.Phone,
            Company = l.Company, Source = l.Source, Status = l.Status,
            EstimatedValue = l.EstimatedValue, Notes = l.Notes, CreatedAt = l.CreatedAt,
            AssignedToUserId = l.AssignedToUserId, AssignedToUserName = l.AssignedToUser?.FullName
        });
    }

    [HttpPost]
    public async Task<ActionResult<LeadDto>> Create([FromBody] LeadDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var lead = new Lead
        {
            ContactName = dto.ContactName,
            Email = dto.Email,
            Phone = dto.Phone,
            Company = dto.Company,
            Source = dto.Source,
            Status = dto.Status,
            EstimatedValue = dto.EstimatedValue,
            Notes = dto.Notes,
            AssignedToUserId = string.IsNullOrEmpty(dto.AssignedToUserId) ? userId : dto.AssignedToUserId
        };

        _context.Leads.Add(lead);
        await _context.SaveChangesAsync();

        dto.Id = lead.Id;
        dto.CreatedAt = lead.CreatedAt;
        return CreatedAtAction(nameof(GetById), new { id = lead.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LeadDto dto)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        lead.ContactName = dto.ContactName;
        lead.Email = dto.Email;
        lead.Phone = dto.Phone;
        lead.Company = dto.Company;
        lead.Source = dto.Source;
        lead.Status = dto.Status;
        lead.EstimatedValue = dto.EstimatedValue;
        lead.Notes = dto.Notes;
        lead.AssignedToUserId = dto.AssignedToUserId;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var lead = await _context.Leads.FindAsync(id);
        if (lead == null) return NotFound();

        _context.Leads.Remove(lead);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
