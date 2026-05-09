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
public class InvoicesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InvoicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<InvoiceDto>>> GetAll()
    {
        var invoices = await _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Payments)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new InvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                CustomerId = i.CustomerId,
                CustomerName = i.Customer != null ? i.Customer.FirstName + " " + i.Customer.LastName : null,
                IssueDate = i.IssueDate,
                DueDate = i.DueDate,
                TotalAmount = i.TotalAmount,
                Status = i.Status,
                Notes = i.Notes,
                CreatedAt = i.CreatedAt,
                TotalPaid = i.Payments.Sum(p => p.Amount)
            })
            .ToListAsync();

        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> GetById(int id)
    {
        var i = await _context.Invoices.Include(x => x.Customer).Include(x => x.Payments).FirstOrDefaultAsync(x => x.Id == id);
        if (i == null) return NotFound();

        return Ok(new InvoiceDto
        {
            Id = i.Id, InvoiceNumber = i.InvoiceNumber, CustomerId = i.CustomerId,
            CustomerName = i.Customer != null ? $"{i.Customer.FirstName} {i.Customer.LastName}" : null,
            IssueDate = i.IssueDate, DueDate = i.DueDate, TotalAmount = i.TotalAmount,
            Status = i.Status, Notes = i.Notes, CreatedAt = i.CreatedAt,
            TotalPaid = i.Payments.Sum(p => p.Amount)
        });
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] InvoiceDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var invoice = new Invoice
        {
            InvoiceNumber = dto.InvoiceNumber,
            CustomerId = dto.CustomerId,
            IssueDate = dto.IssueDate,
            DueDate = dto.DueDate,
            TotalAmount = dto.TotalAmount,
            Status = dto.Status,
            Notes = dto.Notes,
            CreatedByUserId = userId
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        dto.Id = invoice.Id;
        dto.CreatedAt = invoice.CreatedAt;
        return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InvoiceDto dto)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return NotFound();

        invoice.InvoiceNumber = dto.InvoiceNumber;
        invoice.CustomerId = dto.CustomerId;
        invoice.IssueDate = dto.IssueDate;
        invoice.DueDate = dto.DueDate;
        invoice.TotalAmount = dto.TotalAmount;
        invoice.Status = dto.Status;
        invoice.Notes = dto.Notes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null) return NotFound();

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
