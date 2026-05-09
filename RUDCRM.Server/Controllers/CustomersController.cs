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
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> GetAll()
    {
        var customers = await _context.Customers
            .Include(c => c.CreatedByUser)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Company = c.Company,
                Address = c.Address,
                City = c.City,
                State = c.State,
                ZipCode = c.ZipCode,
                Notes = c.Notes,
                CreatedAt = c.CreatedAt,
                CreatedByUserName = c.CreatedByUser != null ? c.CreatedByUser.FullName : null
            })
            .ToListAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        var c = await _context.Customers.Include(x => x.CreatedByUser).FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();

        return Ok(new CustomerDto
        {
            Id = c.Id, FirstName = c.FirstName, LastName = c.LastName, Email = c.Email,
            Phone = c.Phone, Company = c.Company, Address = c.Address, City = c.City,
            State = c.State, ZipCode = c.ZipCode, Notes = c.Notes, CreatedAt = c.CreatedAt,
            CreatedByUserName = c.CreatedByUser?.FullName
        });
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CustomerDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Company = dto.Company,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Notes = dto.Notes,
            CreatedByUserId = userId
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        dto.Id = customer.Id;
        dto.CreatedAt = customer.CreatedAt;
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;
        customer.Company = dto.Company;
        customer.Address = dto.Address;
        customer.City = dto.City;
        customer.State = dto.State;
        customer.ZipCode = dto.ZipCode;
        customer.Notes = dto.Notes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound();

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
