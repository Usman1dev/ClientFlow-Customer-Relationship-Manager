using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUDCRM.Server.Data;
using RUDCRM.Server.DTOs;
using RUDCRM.Server.Models;

namespace RUDCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentDto>>> GetAll()
    {
        var payments = await _context.Payments
            .Include(p => p.Invoice)
            .OrderByDescending(p => p.PaymentDate)
            .Select(p => new PaymentDto
            {
                Id = p.Id,
                InvoiceId = p.InvoiceId,
                InvoiceNumber = p.Invoice != null ? p.Invoice.InvoiceNumber : null,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return Ok(payments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetById(int id)
    {
        var p = await _context.Payments.Include(x => x.Invoice).FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return NotFound();

        return Ok(new PaymentDto
        {
            Id = p.Id, InvoiceId = p.InvoiceId, InvoiceNumber = p.Invoice?.InvoiceNumber,
            Amount = p.Amount, PaymentDate = p.PaymentDate, PaymentMethod = p.PaymentMethod,
            ReferenceNumber = p.ReferenceNumber, Notes = p.Notes, CreatedAt = p.CreatedAt
        });
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create([FromBody] PaymentDto dto)
    {
        var payment = new Payment
        {
            InvoiceId = dto.InvoiceId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            PaymentMethod = dto.PaymentMethod,
            ReferenceNumber = dto.ReferenceNumber,
            Notes = dto.Notes
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        dto.Id = payment.Id;
        dto.CreatedAt = payment.CreatedAt;
        return CreatedAtAction(nameof(GetById), new { id = payment.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PaymentDto dto)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        payment.InvoiceId = dto.InvoiceId;
        payment.Amount = dto.Amount;
        payment.PaymentDate = dto.PaymentDate;
        payment.PaymentMethod = dto.PaymentMethod;
        payment.ReferenceNumber = dto.ReferenceNumber;
        payment.Notes = dto.Notes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
