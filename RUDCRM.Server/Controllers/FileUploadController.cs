using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RUDCRM.Server.Data;
<<<<<<< HEAD
using RUDCRM.Server.DTOs;
using RUDCRM.Server.Models;
=======
using RUDCRM.Shared.DTOs;
using RUDCRM.Shared.Models;
>>>>>>> f1f16b05775f1962e046e7a92be03b9421eef765
using System.Security.Claims;

namespace RUDCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileUploadController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".jpeg", ".gif", ".txt", ".csv" };
    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    public FileUploadController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<ActionResult<List<UploadedDocumentDto>>> GetAll()
    {
        var docs = await _context.UploadedDocuments
            .Include(d => d.Customer)
            .Include(d => d.UploadedByUser)
            .OrderByDescending(d => d.UploadedAt)
            .Select(d => new UploadedDocumentDto
            {
                Id = d.Id,
                FileName = d.FileName,
                OriginalFileName = d.OriginalFileName,
                ContentType = d.ContentType,
                FileSize = d.FileSize,
                UploadedAt = d.UploadedAt,
                CustomerId = d.CustomerId,
                CustomerName = d.Customer != null ? d.Customer.FirstName + " " + d.Customer.LastName : null,
                UploadedByUserName = d.UploadedByUser != null ? d.UploadedByUser.FullName : null
            })
            .ToListAsync();

        return Ok(docs);
    }

    [HttpPost]
    public async Task<ActionResult<UploadedDocumentDto>> Upload([FromForm] IFormFile file, [FromForm] int? customerId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file selected.");

        if (file.Length > MaxFileSize)
            return BadRequest("File size exceeds 10 MB limit.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return BadRequest($"File type '{extension}' is not allowed.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var uploadsDir = Path.Combine(_env.ContentRootPath, "Uploads");
        Directory.CreateDirectory(uploadsDir);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsDir, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var doc = new UploadedDocument
        {
            FileName = uniqueFileName,
            OriginalFileName = file.FileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            CustomerId = customerId,
            UploadedByUserId = userId
        };

        _context.UploadedDocuments.Add(doc);
        await _context.SaveChangesAsync();

        return Ok(new UploadedDocumentDto
        {
            Id = doc.Id,
            FileName = doc.FileName,
            OriginalFileName = doc.OriginalFileName,
            ContentType = doc.ContentType,
            FileSize = doc.FileSize,
            UploadedAt = doc.UploadedAt,
            CustomerId = doc.CustomerId
        });
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(int id)
    {
        var doc = await _context.UploadedDocuments.FindAsync(id);
        if (doc == null) return NotFound();

        var filePath = Path.Combine(_env.ContentRootPath, "Uploads", doc.FileName);
        if (!System.IO.File.Exists(filePath)) return NotFound("File not found on disk.");

        var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(bytes, doc.ContentType, doc.OriginalFileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var doc = await _context.UploadedDocuments.FindAsync(id);
        if (doc == null) return NotFound();

        var filePath = Path.Combine(_env.ContentRootPath, "Uploads", doc.FileName);
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        _context.UploadedDocuments.Remove(doc);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
